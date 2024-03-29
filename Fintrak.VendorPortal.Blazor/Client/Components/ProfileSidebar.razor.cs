using Fintrak.VendorPortal.Blazor.Client.Onboarding;
using Fintrak.VendorPortal.Blazor.Shared.Models.Users;
using Microsoft.AspNetCore.Components;
using System.Reflection.Metadata;

namespace Fintrak.VendorPortal.Blazor.Client.Components
{
    public partial class ProfileSidebar : ComponentBase
    {
		[Inject]
		public IOnboardingService? OnboardingService { get; set; }

		[Parameter]
        public string? ActivePage { get; set; }

		[Parameter]
		public UserDto? PageModel { get; set; }

		public string QueriesPageIsActive { get; set; }
        public string ChangePasswordPageIsActive { get; set; }

		protected override async Task OnInitializedAsync()
		{
			MakeActive();
			StateHasChanged();
		}

		private void MakeActive()
        {
            if (ActivePage == "Queries")
                QueriesPageIsActive = "active";
            else if (ActivePage == "Change Password")
                ChangePasswordPageIsActive = "active";
        }
    }
}
