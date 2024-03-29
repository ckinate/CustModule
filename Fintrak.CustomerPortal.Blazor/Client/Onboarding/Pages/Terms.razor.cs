using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Pages
{
    public partial class Terms
    {
        [Inject]
        public IOnboardingService? OnboardingService { get; set; }

        public async Task Accept()
        {
            SpinnerService.Show();

            var response = await OnboardingService.AcceptTerms(new AcceptTermsDto { });

            SpinnerService.Hide();

            if (response != null && response.Success && response.Result != null)
            {
                NavManager.NavigateTo("/onboarding/gettingstarted");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", $"Unable to onboard customer at this time. {response.Message}", "error", "Ok");
            }
        }
    }
}
