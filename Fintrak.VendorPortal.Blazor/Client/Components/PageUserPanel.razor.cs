using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Fintrak.VendorPortal.Blazor.Client.Components
{
	public partial class PageUserPanel
	{
		private void BeginLogOut()
		{
			Navigation.NavigateToLogout("authentication/logout");
		}
	}
}
