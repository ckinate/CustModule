using Fintrak.CustomerPortal.Blazor.Client.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;

namespace Fintrak.CustomerPortal.Blazor.Client.Shared
{
    public partial class MainLayout
    {
        [Inject]
        public IJSRuntime? JSRuntime { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        IConfiguration Configuration { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var sessionTimeout = Configuration["SessionTimeout"];
            var timeout = !string.IsNullOrEmpty(sessionTimeout) ? int.Parse(sessionTimeout) * 1000 : 300 * 1000;
            await JSRuntime.InitializeInactivityTimer(DotNetObjectReference.Create(this), timeout);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("coreInterop.initHeader");

          
        }

        [JSInvokable]
        public async Task Logout()
        {
            var authState = await AuthenticationState;
            if (authState.User.Identity.IsAuthenticated)
            {
                NavigationManager.NavigateToLogout("authentication/logout?sessionTimeout=true");
            }
        }
    }
}
