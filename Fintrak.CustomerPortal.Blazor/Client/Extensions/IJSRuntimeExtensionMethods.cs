using Microsoft.JSInterop;

namespace Fintrak.CustomerPortal.Blazor.Client.Extensions
{
    public static class IJSRuntimeExtensionMethods
    {
        public static async ValueTask InitializeInactivityTimer<T>(this IJSRuntime js, DotNetObjectReference<T> dotNetObjectReference, int duration = 3000)
            where T : class
        {
            await js.InvokeVoidAsync("initializeInactivityTimer", dotNetObjectReference, duration);
        }
    }
}
