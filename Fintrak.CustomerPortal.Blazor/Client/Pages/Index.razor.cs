using BlazorSpinner;
using Fintrak.CustomerPortal.Blazor.Client.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Fintrak.CustomerPortal.Blazor.Client.Pages
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

			var onboardingStatuResult = await OnboardingService.GetOnboardingStatus();

            SpinnerService.Hide();

			if (onboardingStatuResult != null && onboardingStatuResult.Success)
            {
                if(onboardingStatuResult.Result.Status == OnboardingStatus.NotStarted && !onboardingStatuResult.Result.AcceptTerms)
                {
					NavManager.NavigateTo("/onboarding/terms");
				}
                else if (onboardingStatuResult.Result.Status == OnboardingStatus.NotStarted && onboardingStatuResult.Result.AcceptTerms)
                {
                    NavManager.NavigateTo("/onboarding/registration");
                }
                else
                {
					NavManager.NavigateTo("/onboarding/profile");
				}
            }

			
		}
    }
}
