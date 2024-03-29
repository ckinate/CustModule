using Fintrak.CustomerPortal.Blazor.Shared.Models.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Pages
{
    public partial class ReviewDialog
	{
		[Inject]
		public IOnboardingService? OnboardingService { get; set; }

		private DotNetObjectReference<ReviewDialog> dotNetRef;

		public bool ShowDialog { get; set; }

		public OnboardingModel PageModel { get; set; } = new();

		protected override async Task OnInitializedAsync()
		{
			dotNetRef = DotNetObjectReference.Create(this);

			PageModel = new();
		}

		public void Show(OnboardingModel model)
		{
			ShowDialog = true;
			BlockPage();

			PageModel = model;

			UnBlockPage();
			StateHasChanged();
		}

		public void Close()
		{
			ShowDialog = false;
			StateHasChanged();
		}
	}
}
