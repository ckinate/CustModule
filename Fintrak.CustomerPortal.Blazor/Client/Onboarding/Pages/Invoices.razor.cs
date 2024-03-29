using Fintrak.CustomerPortal.Blazor.Client.Services;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Queries;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Pages
{
	public partial class Invoices
	{
		[Inject]
		public IOnboardingService OnboardingService { get; set; }

		[Inject]
		public IBillingService BillingService { get; set; }

        [Inject]
        IConfiguration Configuration { get; set; }

        public List<BillInvoiceDto> PageModel { get; set; } = default!;

		public UserDto CurrentUser { get; set; }

        public string PortalUrl { get; set; }

        protected override async void OnInitialized()
		{
			SpinnerService.Show();

			PortalUrl =  Configuration["PortalUrl"];

            //PageModel = new();

            await LoadCurrentUser();
			await LoadData();

			SpinnerService.Hide();
			StateHasChanged();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if(firstRender)
			{
				await JSRuntime.InvokeVoidAsync("invoiceInterop.init");
			}
		}

		private async Task LoadCurrentUser()
		{
			var userResponse = await OnboardingService.GetCurrentUser();
			if (userResponse != null && userResponse.Success)
			{
				CurrentUser = userResponse.Result;
			}
		}

		private async Task LoadData()
		{
			var response = await BillingService.GetMyInvoices(1, 100);
			if (response != null && response.Success)
			{
				PageModel = response.Result;
			}
		}

	}
}
