using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Extensions;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Onboarding.Queries;

[Authorize]
public record class GetOnboardingStateQuery : IRequest<BaseResponse<OnboardingStatusDto>>;

public class GetOnboardingStateQueryHandler : IRequestHandler<GetOnboardingStateQuery, BaseResponse<OnboardingStatusDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

	public GetOnboardingStateQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService identityService, IMapper mapper)
	{
		_context = context;
		_currentUserService = currentUserService;
		_identityService = identityService;
        _mapper = mapper;
	}

	public async Task<BaseResponse<OnboardingStatusDto>> Handle(GetOnboardingStateQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<OnboardingStatusDto> { Result = new() };

		var loginId = _currentUserService.UserId;
        var user = await _identityService.GetUserAsync(loginId);
        if (user != null)
        {
			response.Result.AcceptTerms = user.AcceptTerms.HasValue ? user.AcceptTerms.Value : false;
        }

        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.LoginId == loginId);

		if (customer == null)
			response.Result.Status = OnboardingStatus.NotStarted;
		else
			response.Result.Status = customer.Status.GetCustomerStatus();

		return response;
	}
}