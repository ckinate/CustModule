using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Onboarding.Queries;

[Authorize]
public record class GetMyLegalDocumentsQuery () : IRequest<BaseResponse<List<CustomerOnboardingDocumentDto>>>;

public class GetMyLegalDocumentsQueryHandler : IRequestHandler<GetMyLegalDocumentsQuery, BaseResponse<List<CustomerOnboardingDocumentDto>>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly ICustomerIntegrationService _customerIntegrationService;
	private readonly IMapper _mapper;

	public GetMyLegalDocumentsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, ICustomerIntegrationService customerIntegrationService, IMapper mapper)
	{
		_context = context;
		_currentUserService = currentUserService;
		_customerIntegrationService = customerIntegrationService;
		_mapper = mapper;
	}

	public async Task<BaseResponse<List<CustomerOnboardingDocumentDto>>> Handle(GetMyLegalDocumentsQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<List<CustomerOnboardingDocumentDto>> {  Result = new() };

		var loginId = _currentUserService.UserId;

		var customer = await _context.Customers.FirstOrDefaultAsync(c => c.LoginId == loginId);

		//get invoices
		if (!string.IsNullOrEmpty(customer.Code))
		{
			var documentsResponse = await _customerIntegrationService.GetCustomerLegalDocuments(customer.Code);
			if (documentsResponse != null && documentsResponse.Success)
			{
				response.Result = documentsResponse.Result;
			}
		}
		
		return response;
	}
}