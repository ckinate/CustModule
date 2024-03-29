using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;

namespace Fintrak.CustomerPortal.Application.Users.Commands
{
	public class LockUserCommand : IRequest<BaseResponse<bool>>
	{
		public LockUserDto Item { get; set; }
	}

	public class LockUserCommandValidator : AbstractValidator<LockUserCommand>
	{
		public LockUserCommandValidator()
		{
			RuleFor(v => v.Item).NotNull();
		}
	}

	public class LockUserCommandHandler : IRequestHandler<LockUserCommand, BaseResponse<bool>>
	{
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IIdentityService _identityService;
		private readonly IConfiguration _configuration;

		public LockUserCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService identityService)
		{
			_context = context;
			_currentUserService = currentUserService;
			_identityService = identityService;
		}

		public async Task<BaseResponse<bool>> Handle(LockUserCommand request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<bool>();

			var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == request.Item.CustomerId);
			if(customer == null)
			{
				response.Success = false;
				response.Message = $"Customer with Id '{request.Item.CustomerId}' not found.";

				return response;
			}

			var result = await _identityService.LockUserAsync(customer.LoginId, request.Item.Lock);
			if (!result)
			{
				response.Success = false;
				response.Message = "Fail to lock user at this time.";
			}
			else
			{
				customer.IsLock = request.Item.Lock;
				await _context.SaveChangesAsync(cancellationToken);

				response.Result = true;
			}

			return response;
		}
	}
}
