using MediatR;
using FluentValidation;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Domain.Entities;
using Fintrak.CustomerPortal.Application.Common.Exceptions;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Invitations;
using Fintrak.CustomerPortal.Domain.Events.Invitations;
using Microsoft.Extensions.Configuration;
using Fintrak.CustomerPortal.Application.Utilities;

namespace Fintrak.InvitationPortal.Application.Invitations.Commands
{
	public class ReplacementInvitationCommand  : IRequest<BaseResponse<string>>
	{
		public ReplacementInvitationDto Item { get; set; }
	}

	public class ReplacementInvitationCommandValidator : AbstractValidator<ReplacementInvitationCommand>
	{
		public ReplacementInvitationCommandValidator()
		{
			RuleFor(v => v.Item).NotNull();
			RuleFor(v => v.Item.AdminName).MaximumLength(200).NotEmpty();
			RuleFor(v => v.Item.AdminName).MaximumLength(250).NotEmpty();
		}
	}

	public class ReplacementInvitationCommandHandler : IRequestHandler<ReplacementInvitationCommand , BaseResponse<string>>
	{
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IConfiguration _configuration;
        private readonly IIdentityService _identityService;

        public ReplacementInvitationCommandHandler(IApplicationDbContext context, 
			ICurrentUserService currentUserService, 
			IConfiguration configuration, IIdentityService identityService)
		{
			_context = context;
			_currentUserService = currentUserService;
			_configuration = configuration;
            _identityService = identityService;
        }

		public async Task<BaseResponse<string>> Handle(ReplacementInvitationCommand  request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<string>();

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == request.Item.CustomerId);
            if (customer == null)
            {
                response.Success = false;
                response.Message = $"Customer with Id '{request.Item.CustomerId}' not found.";

                return response;
            }

            var entity = await _context.Invitations.FirstOrDefaultAsync(c => c.AdminEmail == request.Item.AdminEmail);
			if(entity != null && entity.CompanyName != customer.Name)
			{
				response.Message = "Email already in use.";
				response.Success = false;
				return response;
			}

            var result = await _identityService.LockUserAsync(customer.LoginId, true);
            if (!result)
            {
                response.Success = false;
                response.Message = "Fail to lock previous user at this time.";
            }

            if (entity != null)
			{
				if (entity.Used)
				{
					response.Message = "Customer already signup.";
					response.Success = false;
					return response;
				}
				else if (entity.CompanyName == customer.Name && entity.AdminEmail == request.Item.AdminEmail)
				{
                    entity.AddDomainEvent(new ReplacementInvitationCreatedEvent(entity));
                }		
			}
			else
			{
				var invitationCode = UniqueKeyGenerator.AlphaNumericRNGCharacterMask(50, 50);

				entity = new Invitation
				{
					Code = invitationCode,
					CompanyName = customer.Name,
					AdminName = request.Item.AdminName,
					AdminEmail = request.Item.AdminEmail,
					EntryDate = DateTime.Now,
					ReplaceAdmin = true,
					Used = false
				};

				entity.AddDomainEvent(new ReplacementInvitationCreatedEvent(entity));

				_context.Invitations.Add(entity);
			}

            await _context.SaveChangesAsync(cancellationToken);



			response.Result = entity.Code;

			return response;
		}

		private bool ValidateInvitationCommand(ReplacementInvitationCommand  command)
		{
			var hashMode = _configuration["HashSettings:Mode"].ToString();
			var hashKey = _configuration["HashSettings:Key"].ToString();

			var hashInput = $"{command.Item.CustomerId}{command.Item.AdminName}{command.Item.AdminEmail}{hashKey}";

			if (hashMode == "Sha256")
			{
				return HashUtility.ValidateSha256Hash(command.Item.Hash, hashInput);
			}
			else if (hashMode == "MD5")
			{
				return HashUtility.ValidateMD5Hash(command.Item.Hash, hashInput);
			}
			else
			{
				throw new NotImplementedException("Hash mode not implemented.");
			}
		}
	}
}

