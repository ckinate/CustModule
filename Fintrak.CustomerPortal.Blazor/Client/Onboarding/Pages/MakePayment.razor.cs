using Fintrak.CustomerPortal.Blazor.Client.Services;
using Fintrak.CustomerPortal.Blazor.Shared.Extensions;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;
using Microsoft.AspNetCore.Components;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Pages
{
	public partial class MakePayment
	{
		[Parameter]
		public int InvoiceId { get; set; }

		[Inject]
		public IBillingService BillingService { get; set; }

		public CentralPayLogDto PageModel { get; set; }

		protected override async void OnInitialized()
		{
			SpinnerService.Show();

			await LoadData();

			SpinnerService.Hide();
			StateHasChanged();
		}

		private async Task LoadData()
		{
			var response = await BillingService.CreatePaymentRequest(InvoiceId);
			if (response != null && response.Success)
			{
				PageModel = response.Result;
			}
		}
	}
}
