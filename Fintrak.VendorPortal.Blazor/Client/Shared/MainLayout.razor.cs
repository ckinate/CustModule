using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Fintrak.VendorPortal.Blazor.Client.Shared
{
    public partial class MainLayout
    {
        [Inject]
        public IJSRuntime? JSRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("coreInterop.initHeader");
        }
    }
}
