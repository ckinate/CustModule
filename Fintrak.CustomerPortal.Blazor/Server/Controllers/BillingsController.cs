using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;
using Fintrak.CustomerPortal.Application.Onboarding.Queries;
using Fintrak.CustomerPortal.Application.Billing.Commands;

namespace Fintrak.CustomerPortal.Blazor.Server.Controllers
{
	[Authorize]
	public class BillingsController : ApiControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly ICustomerIntegrationService _customerIntegrationService;

		public BillingsController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet("GetMyInvoices")]
		public async Task<ActionResult<BaseResponse<List<BillInvoiceDto>>>> GetInvoices([FromQuery] GetMyInvoicesQuery query)
		{
			var result = await Mediator.Send(query);
			return result;
		}

        [HttpGet("GetInvoice")]
        public async Task<ActionResult<BaseResponse<BillInvoiceDto>>> GetInvoice([FromQuery] GetInvoiceQuery query)
        {
            var result = await Mediator.Send(query);
            return result;
        }

        [HttpGet("CreateInvoicePaymentRequest")]
        public async Task<ActionResult<BaseResponse<CentralPayLogDto>>> CreateInvoicePaymentRequest([FromQuery] int invoiceId)
        {
            var result = await Mediator.Send(new CreateInvoicePaymentRequestCommand { InvoiceId = invoiceId });
            return result;
        }

        [HttpGet("VerifyPaymentRequest")]
        public async Task<ActionResult<BaseResponse<CentralPayLogDto>>> VerifyPaymentRequest([FromQuery] string requestId)
        {
            var result = await Mediator.Send(new VerifyPaymentRequestCommand { RequestId = requestId });
            return result;
        }
    }
}
