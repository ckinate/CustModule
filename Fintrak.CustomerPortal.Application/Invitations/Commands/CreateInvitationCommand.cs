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
	public class CreateInvitationCommand : IRequest<BaseResponse<string>>
	{
		public CreateInvitationDto Item { get; set; }
	}

	public class CreateInvitationCommandValidator : AbstractValidator<CreateInvitationCommand>
	{
		public CreateInvitationCommandValidator()
		{
			RuleFor(v => v.Item).NotNull();
			RuleFor(v => v.Item.CompanyName).MaximumLength(200).NotEmpty();
			RuleFor(v => v.Item.AdminName).MaximumLength(200).NotEmpty();
			RuleFor(v => v.Item.AdminName).MaximumLength(250).NotEmpty();
		}
	}

	public class CreateInvitationCommandHandler : IRequestHandler<CreateInvitationCommand, BaseResponse<string>>
	{
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IConfiguration _configuration;

		public CreateInvitationCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IConfiguration configuration)
		{
			_context = context;
			_currentUserService = currentUserService;
			_configuration = configuration;
		}

		public async Task<BaseResponse<string>> Handle(CreateInvitationCommand request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<string>();

            //if (!ValidateInvitationCommand(request))
            //{
            //	throw new Exception("[CreateInvitationCommand] - Request parameters not properly formated.");
            //}

            var entity = await _context.Invitations.FirstOrDefaultAsync(c => c.AdminEmail == request.Item.AdminEmail);
			if(entity != null && entity.CompanyName != request.Item.CompanyName)
			{
				response.Message = "Email already in use.";
				response.Success = false;
				return response;
			}

            //var entity = await _context.Invitations.FirstOrDefaultAsync(c => c.CompanyName == request.Item.CompanyName);
			if(entity != null)
			{
				if (entity.Used)
				{
					response.Message = "Customer already signup.";
					response.Success = false;
					return response;
				}
				else if (entity.CompanyName == request.Item.CompanyName && entity.AdminEmail == request.Item.AdminEmail)
				{
                    entity.AddDomainEvent(new InvitationCreatedEvent(entity));
                }		
			}
			else
			{
				var invitationCode = UniqueKeyGenerator.AlphaNumericRNGCharacterMask(50, 50);

				entity = new Invitation
				{
					Code = invitationCode,
					CompanyName = request.Item.CompanyName,
					AdminName = request.Item.AdminName,
					AdminEmail = request.Item.AdminEmail,
					EntryDate = DateTime.Now,
					Used = false
				};

				entity.AddDomainEvent(new InvitationCreatedEvent(entity));

				_context.Invitations.Add(entity);
			}
			
			await _context.SaveChangesAsync(cancellationToken);

			response.Result = entity.Code;

			return response;
		}

		private bool ValidateInvitationCommand(CreateInvitationCommand command)
		{
			var hashMode = _configuration["HashSettings:Mode"].ToString();
			var hashKey = _configuration["HashSettings:Key"].ToString();

			var hashInput = $"{command.Item.CompanyName}{command.Item.AdminName}{command.Item.AdminEmail}{hashKey}";

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

