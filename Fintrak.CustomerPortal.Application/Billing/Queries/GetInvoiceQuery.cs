using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Onboarding.Queries;

[Authorize]
public record class GetInvoiceQuery (int InvoiceId) : IRequest<BaseResponse<BillInvoiceDto>>;

public class GetMyInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, BaseResponse<BillInvoiceDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly ICustomerIntegrationService _customerIntegrationService;
	private readonly IMapper _mapper;

	public GetMyInvoiceQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, ICustomerIntegrationService customerIntegrationService, IMapper mapper)
	{
		_context = context;
		_currentUserService = currentUserService;
		_customerIntegrationService = customerIntegrationService;
		_mapper = mapper;
	}

	public async Task<BaseResponse<BillInvoiceDto>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<BillInvoiceDto> { Result = new() };

		var loginId = _currentUserService.UserId;

		var customer = await _context.Customers.FirstOrDefaultAsync(c => c.LoginId == loginId);

		//get invoices
		var invoiceResponse = await _customerIntegrationService.GetInvoice(request.InvoiceId);
		if (invoiceResponse != null && invoiceResponse.Success)
		{
			response.Result = invoiceResponse.Result;
		}

		return response;
	}
}