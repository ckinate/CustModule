using Fintrak.CustomerPortal.Blazor.Client.Services;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;
using Microsoft.AspNetCore.Components;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Pages
{
	public partial class Invoice
    {
		[Inject]
		public IBillingService BillingService { get; set; }

		[Parameter]
        public int InvoiceId { get; set; }

		public BillInvoiceDto PageModel { get; set; } = default!;
		public string PaymentVerificationUrl { get; set; } = $"https://localhost:7293/Verifications/Index";

		protected PaymentReceiptyDialog PaymentReceiptyDialog { get; set; }

		protected override async void OnInitialized()
		{
			SpinnerService.Show();

			await LoadData();

			SpinnerService.Hide();
			StateHasChanged();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				//await JSRuntime.InvokeVoidAsync("invoiceInterop.init");
			}
		}

		private async Task LoadData()
		{
			var response = await BillingService.GetInvoice(InvoiceId);
			if (response != null && response.Success)
			{
				PageModel = response.Result;
			}
		}

		private async Task SubmitPaymentReceipt(string invoiceCode)
		{
			PaymentReceiptyDialog.Show(invoiceCode);
		}

		public async Task ReceiptSubmittedHandler()
		{
			SpinnerService.Hide();

			await LoadData();
			StateHasChanged();

			SpinnerService.Hide();
		}
	}
}
