using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Users.Queries;

[Authorize]
public record class GetUserDetailQuery : IRequest<BaseResponse<UserDto>>;

public class GetUserDetailQueryHandler : IRequestHandler<GetUserDetailQuery, BaseResponse<UserDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IIdentityService _identityService;

	public GetUserDetailQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService identityService)
	{
		_context = context;
		_currentUserService = currentUserService;
		_identityService = identityService;
	}

	public async Task<BaseResponse<UserDto>> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<UserDto>();

		var loginId = _currentUserService.UserId;
		var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.LoginId == loginId);

		var user = await _identityService.GetUserAsync(loginId);

		response.Result = user;

		return response;
	}
}