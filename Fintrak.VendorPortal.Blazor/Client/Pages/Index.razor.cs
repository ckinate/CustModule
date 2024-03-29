using BlazorSpinner;
using Fintrak.VendorPortal.Blazor.Client.Invitations;
using Fintrak.VendorPortal.Blazor.Client.Onboarding;
using Fintrak.VendorPortal.Blazor.Shared.Models.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Fintrak.VendorPortal.Blazor.Client.Pages
{
    public partial class Index
    {
        [Inject]
        public ILogger<Index> Logger { get; set; }

		[Inject]
		public IOnboardingService OnboardingService { get; set; }

		private DotNetObjectReference<Index>? dotNetRef;

        protected override async Task OnInitializedAsync()
        {
			SpinnerService.Show();

			dotNetRef = DotNetObjectReference.Create(this);

			var result = await OnboardingService.GetOnboardingStatus();

			SpinnerService.Hide();

			if (result != null && result.Success)
            {
                if(result.Result == OnboardingStatus.NotStarted)
                {
					NavManager.NavigateTo("/onboarding/terms");
				}
                else
                {
					NavManager.NavigateTo("/onboarding/profile");
				}
            }

			
		}
    }
}
