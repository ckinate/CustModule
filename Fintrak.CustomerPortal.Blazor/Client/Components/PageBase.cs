using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Fintrak.CustomerPortal.Blazor.Client;

public abstract class PageBase : ComponentBase
{
	[Inject]
	public NavigationManager? NavManager { get; set; }

	[Inject]
	public IJSRuntime? JSRuntime { get; set; }

	public string? PageState { get; set; }
	public bool PageStateFlag { get; set; }

	public void BlockPage()
	{
		PageState = "overlay overlay-block";
		PageStateFlag = true;
		StateHasChanged();
	}

	public void UnBlockPage()
	{
		PageState = "";
		PageStateFlag = false;
		StateHasChanged();
	}
}
