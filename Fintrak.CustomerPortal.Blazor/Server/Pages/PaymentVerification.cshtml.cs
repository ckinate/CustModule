using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fintrak.CustomerPortal.Blazor.Server.Pages
{
    public class PaymentVerificationModel : PageModel
    {
        private readonly ILogger<PaymentVerificationModel> _logger;

        public PaymentVerificationModel(ILogger<PaymentVerificationModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public PaymentInputModel Input { get; set; }

        public class PaymentInputModel
        {
            public string CpayTxnRef { get; set; }
            public string TransactionId { get; set; }
            public string MerchantId { get; set; }
        }

        public async Task OnGetAsync()
        {
            Input = new PaymentInputModel();

            Input.CpayTxnRef = this.Request.Form["cpayTxnRef"];
            Input.TransactionId = this.Request.Form["transactionId"];
            Input.MerchantId = this.Request.Form["merchantId"];
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Input = new PaymentInputModel();

            Input.CpayTxnRef = this.Request.Form["cpayTxnRef"];
            Input.TransactionId = this.Request.Form["transactionId"];
            Input.MerchantId = this.Request.Form["merchantId"];

            if (!string.IsNullOrEmpty(Input.CpayTxnRef))
            {
                return LocalRedirect($"https://localhost:7293?cpayRef={Input.CpayTxnRef}");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
