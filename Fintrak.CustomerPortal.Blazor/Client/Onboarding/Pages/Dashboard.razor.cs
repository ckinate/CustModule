using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Pages
{
	public partial class Dashboard
	{
		[Inject]
		public IOnboardingService? OnboardingService { get; set; }

		private DotNetObjectReference<Dashboard>? dotNetRef;

		public DashboardDto PageModel { get; set; } = new();

		protected override async void OnInitialized()
		{
			SpinnerService.Show();

			dotNetRef = DotNetObjectReference.Create(this);

			await LoadPageData();

			StateHasChanged();

			SpinnerService.Hide();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{

		}

		private async Task LoadPageData()
		{
			var response = await OnboardingService.GetDashboard();
			if (response != null && response.Success)
			{
				if (response.Result != null)
				{
					PageModel = response.Result;
				}
			}
		}
	}
}
