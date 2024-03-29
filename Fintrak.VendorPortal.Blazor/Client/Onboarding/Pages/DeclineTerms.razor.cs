using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Fintrak.VendorPortal.Blazor.Client.Onboarding.Pages
{
	public partial class DeclineTerms
	{
		private void BeginLogOut()
		{
			Navigation.NavigateToLogout("authentication/logout");
		}
	}
}
