using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Onboarding.Queries;

[Authorize]
public record class GetMyInvoicesQuery (int PageIndex, int PageSize) : IRequest<BaseResponse<List<BillInvoiceDto>>>;

public class GetMyInvoicesQueryHandler : IRequestHandler<GetMyInvoicesQuery, BaseResponse<List<BillInvoiceDto>>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly ICustomerIntegrationService _customerIntegrationService;
	private readonly IMapper _mapper;

	public GetMyInvoicesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, ICustomerIntegrationService customerIntegrationService, IMapper mapper)
	{
		_context = context;
		_currentUserService = currentUserService;
		_customerIntegrationService = customerIntegrationService;
		_mapper = mapper;
	}

	public async Task<BaseResponse<List<BillInvoiceDto>>> Handle(GetMyInvoicesQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<List<BillInvoiceDto>> {  Result = new() };

		var loginId = _currentUserService.UserId;

		var customer = await _context.Customers.FirstOrDefaultAsync(c => c.LoginId == loginId);

		//get invoices
		if (!string.IsNullOrEmpty(customer.Code))
		{
			var invoicesResponse = await _customerIntegrationService.GetInvoices(customer.Code, request.PageIndex, request.PageSize);
			if (invoicesResponse != null && invoicesResponse.Success)
			{
				response.Result = invoicesResponse.Result;
			}
		}
		
		return response;
	}
}