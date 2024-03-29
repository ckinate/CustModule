using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;

namespace Fintrak.CustomerPortal.Application.Users.Commands
{
	public class ChangeAdminCommand : IRequest<BaseResponse<bool>>
	{
		public ChangeAdminDto Item { get; set; }
	}

	public class ChangeAdminCommandValidator : AbstractValidator<ChangeAdminCommand>
	{
		public ChangeAdminCommandValidator()
		{
			RuleFor(v => v.Item).NotNull();
		}
	}

	public class ChangeAdminCommandHandler : IRequestHandler<ChangeAdminCommand, BaseResponse<bool>>
	{
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IIdentityService _identityService;
		private readonly IConfiguration _configuration;

		public ChangeAdminCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService identityService)
		{
			_context = context;
			_currentUserService = currentUserService;
			_identityService = identityService;
		}

		public async Task<BaseResponse<bool>> Handle(ChangeAdminCommand request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<bool>();

			var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == request.Item.CustomerId);
			if(customer == null)
			{
				response.Success = false;
				response.Message = $"Customer with Id '{request.Item.CustomerId}' not found.";

				return response;
			}

			var result = await _identityService.LockUserAsync(customer.LoginId, true);
			if (!result)
			{
				response.Success = false;
				response.Message = "Fail to lock previous user at this time.";
			}
			
			//create new user

			return response;
		}
	}
}
