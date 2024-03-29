using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;
using MediatR;

namespace Fintrak.CustomerPortal.Application.Onboarding.Queries;

public record class GetBasicInvoiceQuery (int InvoiceId) : IRequest<BaseResponse<BillInvoiceDto>>;

public class GetBasicInvoiceQueryHandler : IRequestHandler<GetBasicInvoiceQuery, BaseResponse<BillInvoiceDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICustomerIntegrationService _customerIntegrationService;
	private readonly IMapper _mapper;

	public GetBasicInvoiceQueryHandler(IApplicationDbContext context, ICustomerIntegrationService customerIntegrationService, IMapper mapper)
	{
		_context = context;
		_customerIntegrationService = customerIntegrationService;
		_mapper = mapper;
	}

	public async Task<BaseResponse<BillInvoiceDto>> Handle(GetBasicInvoiceQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<BillInvoiceDto> { Result = new() };

		//get invoices
		var invoiceResponse = await _customerIntegrationService.GetInvoice(request.InvoiceId);
		if (invoiceResponse != null && invoiceResponse.Success)
		{
			response.Result = invoiceResponse.Result;
		}

		return response;
	}
}