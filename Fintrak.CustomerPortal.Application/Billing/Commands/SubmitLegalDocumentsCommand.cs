using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Billing.Commands;

public record SubmitPaymentReceiptCommand : IRequest<BaseResponse<bool>>
{
	public PortalInvoicePaymentReceiptDto Item { get; set; }
}

public class SubmitPaymentReceiptCommandHandler : IRequestHandler<SubmitPaymentReceiptCommand, BaseResponse<bool>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly ICustomerIntegrationService _customerIntegrationService;

	public SubmitPaymentReceiptCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, ICustomerIntegrationService customerIntegrationService)
	{
		_context = context;
		_currentUserService = currentUserService;
		_customerIntegrationService = customerIntegrationService;
	}

	public async Task<BaseResponse<bool>> Handle(SubmitPaymentReceiptCommand request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<bool>();

		var loginId = _currentUserService.UserId;
		var customer = await _context.Customers.FirstOrDefaultAsync(c => c.LoginId == loginId);

		//get invoices
		if (!string.IsNullOrEmpty(customer.Code))
		{
			var documentResponse = await _customerIntegrationService.SubmitPaymentReceipt(request.Item);
			if (documentResponse != null && documentResponse.Success)
			{
				response.Result = documentResponse.Result;
				response.Success = true;
			}
		}

		return response;
	}
}
