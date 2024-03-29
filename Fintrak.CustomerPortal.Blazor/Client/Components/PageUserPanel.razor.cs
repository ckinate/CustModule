using Fintrak.CustomerPortal.Blazor.Client.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Fintrak.CustomerPortal.Blazor.Client.Components
{
	public partial class PageUserPanel
	{
		[Inject]
		public IOnboardingService? OnboardingService { get; set; }

		public UserDto CurrentUser { get; set; }

		protected override async void OnInitialized()
		{
			SpinnerService.Show();

			await LoadCurrentUser();

			SpinnerService.Hide();
			StateHasChanged();
		}

		private async Task LoadCurrentUser()
		{
			var userResponse = await OnboardingService.GetCurrentUser();
			if (userResponse != null && userResponse.Success)
			{
				CurrentUser = userResponse.Result;
			}
		}

		private void BeginLogOut()
		{
			Navigation.NavigateToLogout("authentication/logout");
		}
	}
}
