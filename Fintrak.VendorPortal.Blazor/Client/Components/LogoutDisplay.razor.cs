using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Fintrak.VendorPortal.Blazor.Client.Components
{
    public partial class LogoutDisplay
    {
        [Inject]
        NavigationManager Navigation { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            //Navigation.NavigateToLogin("authentication/login");
        }
    }
}
