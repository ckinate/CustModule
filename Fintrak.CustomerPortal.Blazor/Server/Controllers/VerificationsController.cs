using Fintrak.CustomerPortal.Application.Onboarding.Queries;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Fintrak.CustomerPortal.Blazor.Server.Controllers
{
    public class VerificationsController : Controller
    {
        private readonly IConfiguration _configuration;

		private ISender _mediator = null!;

		protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

		public VerificationsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Index()
        {
            var cpayTxnRef = HttpContext.Request.Form.ContainsKey("cpayTxnRef") ? HttpContext.Request.Form["cpayTxnRef"].ToString() : "";
            var transactionId = HttpContext.Request.Form.ContainsKey("transactionId") ? HttpContext.Request.Form["transactionId"].ToString() : "";
            var merchantId = HttpContext.Request.Form.ContainsKey("merchantId") ? HttpContext.Request.Form["merchantId"].ToString() : "";

			var responseUrl = _configuration["CentralPay:ResponseUrl"].ToString();

			return RedirectPermanent($"{responseUrl}?requestId={transactionId}&cpayTxnRef={cpayTxnRef}");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadInvoice([FromQuery] int invoiceId)
        {
			var result = await Mediator.Send(new GetBasicInvoiceQuery(invoiceId));

			return View(result.Result);
        }

	}
}
