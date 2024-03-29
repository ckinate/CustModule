using Fintrak.VendorPortal.Blazor.Shared.Models.Queries;
using Fintrak.VendorPortal.Blazor.Shared.Models.Users;
using Microsoft.AspNetCore.Components;

namespace Fintrak.VendorPortal.Blazor.Client.Onboarding.Pages
{
	public partial class Profile
	{
		[Inject]
		public IOnboardingService OnboardingService { get; set; }

		protected RespondToQueryDialog RespondToQueryDialog { get; set; }

		public List<QueryDto> PageModel { get; set; } = default!;
		public UserDto CurrentUser { get; set; }

		protected override async void OnInitialized()
		{
			SpinnerService.Show();

			PageModel = new();

			await LoadCurrentUser();
			await LoadData();

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

		private async Task LoadData()
		{
			var response = await OnboardingService.GetQueries();
			if (response != null && response.Success)
			{
				PageModel = response.Result;
			}
		}

		private void ResponseToQuery(int? queryId)
		{
			var selectedQuery = PageModel.FirstOrDefault(c=> c.Id == queryId);
			RespondToQueryDialog.Show(selectedQuery);
		}

		public async Task OnQueryResponse()
		{
			SpinnerService.Hide();

			await LoadData();
			StateHasChanged();

			SpinnerService.Hide();
		}
	}
}
