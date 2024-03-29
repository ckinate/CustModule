using Fintrak.CustomerPortal.Blazor.Client.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;
using Microsoft.AspNetCore.Components;

namespace Fintrak.CustomerPortal.Blazor.Client.Components
{
    public partial class PageHeader : ComponentBase
    {
        [Inject]
        public IOnboardingService? OnboardingService { get; set; }

        public UserDto CurrentUser { get; set; }
        public bool ShowQuickMenu { get; set; }

        protected override async Task OnInitializedAsync()
		{
			SpinnerService.Show();

			await LoadCurrentUser();
			var onboardingStatuResult = await OnboardingService.GetOnboardingStatus();

			SpinnerService.Hide();

			if (onboardingStatuResult != null && onboardingStatuResult.Success)
			{
				if (onboardingStatuResult.Result.Status == OnboardingStatus.NotStarted && !onboardingStatuResult.Result.AcceptTerms)
				{
					ShowQuickMenu = false;
				}
				else if (onboardingStatuResult.Result.Status == OnboardingStatus.NotStarted && onboardingStatuResult.Result.AcceptTerms)
				{
					ShowQuickMenu = false;
				}
				else
				{
					ShowQuickMenu = true;
				}
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
    }
}
