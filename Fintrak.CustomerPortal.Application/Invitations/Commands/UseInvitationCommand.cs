using MediatR;
using FluentValidation;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Domain.Entities;
using Fintrak.CustomerPortal.Application.Common.Exceptions;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Invitations;

namespace Fintrak.InvitationPortal.Application.Invitations.Commands
{
	public class UseInvitationCommand : IRequest<BaseResponse<bool>>
	{
		public UseInvitationDto Item { get; set; }
	}

	public class UseInvitationCommandValidator : AbstractValidator<UseInvitationCommand>
	{
		public UseInvitationCommandValidator()
		{
			RuleFor(v => v.Item).NotNull();
			RuleFor(v => v.Item.Code).MaximumLength(200).NotEmpty();
		}
	}

	public class UseInvitationCommandHandler : IRequestHandler<UseInvitationCommand, BaseResponse<bool>>
	{
		private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public UseInvitationCommandHandler(IApplicationDbContext context, IIdentityService identityService)
		{
			_context = context;
			_identityService = identityService;

        }

		public async Task<BaseResponse<bool>> Handle(UseInvitationCommand request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<bool>();

			var entity = await _context.Invitations.FirstOrDefaultAsync(c => c.Code == request.Item.Code);
			if(entity == null)
			{
				throw new NotFoundException(nameof(Invitation), request.Item.Code);
			}

			entity.Used = true;
			entity.UsedDate = DateTime.Now;

			if (entity.ReplaceAdmin)
			{
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Name == entity.CompanyName);
                if (customer == null)
                {
                    response.Success = false;
                    response.Message = $"Customer with Name '{entity.CompanyName}' not found.";

                    return response;
                }

				customer.LoginId = request.Item.LoginId;
            }

			//entity.AddDomainEvent(new InvitationUsedEvent(entity));

			await _context.SaveChangesAsync(cancellationToken);

			response.Result = true;

			return response;
		}
	}
}

