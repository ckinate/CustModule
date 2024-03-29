using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Fintrak.CustomerPortal.Blazor.Client.Components
{
    public partial class LogoutDisplay
    {
        [Inject]
        NavigationManager Navigation { get; set; }

        [Parameter]
        public bool SessionTimeout { get; set; }

        public string Message { get; set; } = "You are logged out.";

        protected override void OnInitialized()
        {
            if(SessionTimeout)
            {
                Message = "Sorry, your session has expired. You have been logged out.";
            }
            //base.OnInitialized();
            //Navigation.NavigateToLogin("authentication/login");
        }
    }
}
