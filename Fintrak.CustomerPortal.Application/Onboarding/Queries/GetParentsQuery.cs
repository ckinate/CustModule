using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Onboarding.Queries;

[Authorize]
public record class GetParentsQuery : IRequest<BaseResponse<List<LookupModel>>>;

public class GetParentsQueryHandler : IRequestHandler<GetParentsQuery, BaseResponse<List<LookupModel>>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly ICustomerIntegrationService _customerIntegrationService;

	public GetParentsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, ICustomerIntegrationService customerIntegrationService)
	{
		_context = context;
		_currentUserService = currentUserService;
		_customerIntegrationService = customerIntegrationService;
	}

	public async Task<BaseResponse<List<LookupModel>>> Handle(GetParentsQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<List<LookupModel>>();

		var loginId = _currentUserService.UserId;

		var customer = await _context.Customers.FirstOrDefaultAsync(c => c.LoginId == loginId);

		var parentsResponse = await _customerIntegrationService.GetSubsidiaryHeadsLookUp(customer.Code ?? "");
		if (parentsResponse != null && parentsResponse.Success)
		{
			response.Result = parentsResponse.Result;
		}

		return response;
	}
}