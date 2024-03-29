using Fintrak.CustomerPortal.Blazor.Client.Extensions;
using Fintrak.CustomerPortal.Blazor.Client.Services;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;
using Microsoft.AspNetCore.Components;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Pages
{
    public partial class PaymentVerification
    {
        [Inject]
        public IBillingService BillingService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public CentralPayLogDto PageModel { get; set; } = default!;

        public bool Successful { get; set; } = false;
        
        protected override async void OnInitialized()
        {
            SpinnerService.Show();

			NavigationManager.TryGetQueryString("requestId", out string requestId);
			NavigationManager.TryGetQueryString("cpayTxnRef", out string cpayReference);

			PageModel = new();

            await LoadData(requestId, cpayReference);

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

        private async Task LoadData(string requestId, string cpayTxnRef)
        {
            var response = await BillingService.VerifyPaymentRequest(requestId);
            if (response != null && response.Success)
            {
                PageModel = response.Result;
            }
        }
    }
}
