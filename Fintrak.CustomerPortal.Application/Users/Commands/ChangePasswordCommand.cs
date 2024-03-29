using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Queries;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Application.Common.Exceptions;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;
using FluentValidation.Results;

namespace Fintrak.CustomerPortal.Application.Users.Commands
{
	public class ChangePasswordCommand : IRequest<BaseResponse<bool>>
	{
		public ChangePasswordDto Item { get; set; }
	}

	public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
	{
		public ChangePasswordCommandValidator()
		{
			RuleFor(v => v.Item).NotNull();
			RuleFor(v => v.Item.OldPassword).MaximumLength(200).NotEmpty();
			RuleFor(v => v.Item.NewPassword).MaximumLength(200).NotEmpty();
			RuleFor(v => v.Item.ConfirmPassword).MaximumLength(200).NotEmpty();
		}
	}

	public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, BaseResponse<bool>>
	{
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IIdentityService _identityService;
		private readonly IConfiguration _configuration;

		public ChangePasswordCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService identityService)
		{
			_context = context;
			_currentUserService = currentUserService;
			_identityService = identityService;
		}

		public async Task<BaseResponse<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<bool>();

			var result = await _identityService.ChangePasswordAsync(_currentUserService.UserId, request.Item.OldPassword, request.Item.NewPassword);
			if (!result.Result.Succeeded)
			{
				response.Success = false;
				response.ValidationErrors = result.Result.Errors.ToList();
			}
			else
			{
				response.Result = true;
			}

			return response;
		}
	}
}
