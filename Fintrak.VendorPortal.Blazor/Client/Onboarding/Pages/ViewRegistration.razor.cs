using Fintrak.VendorPortal.Blazor.Client.Onboarding.Models;
using Fintrak.VendorPortal.Blazor.Client.Onboarding.Models.Validators;
using Fintrak.VendorPortal.Blazor.Client.Services;
using Fintrak.VendorPortal.Blazor.Shared.Models;
using Fintrak.VendorPortal.Blazor.Shared.Models.Onboarding;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Linq;
using System.Xml.Linq;

namespace Fintrak.VendorPortal.Blazor.Client.Onboarding.Pages
{
	public partial class ViewRegistration
	{
		[Inject]
		public IOnboardingService? OnboardingService { get; set; }

		private DotNetObjectReference<ViewRegistration>? dotNetRef;

		public OnboardingModel? PageModel { get; set; }

		private int CurrentStep = 1;

		protected override async void OnInitialized()
		{
			SpinnerService.Show();

			PageModel = new();
			
			await LoadPageData();

			SpinnerService.Hide();

			StateHasChanged();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			dotNetRef = DotNetObjectReference.Create(this);

			//await JSRuntime.InvokeVoidAsync("registrationInterop.initSelectPicker");
			await JSRuntime.InvokeVoidAsync("registrationInterop.init", dotNetRef);

		}

		private async Task LoadPageData()
		{
			var response = await OnboardingService.GetVendor();
			if (response != null && response.Success)
			{
				if (response.Result != null)
				{
					PageModel = response.Result;
				}
			}
		}

		[JSInvokable]
		public bool SetCurrentStep(int step, int newStep)
		{
			CurrentStep = step;
			Console.WriteLine($"Step : {step} - New Step : {newStep}");

			//if (CurrentStep == 4)
			//{
			//	if (!PageModel.OfficialInformation.UseForeignAccount)
			//		PageModel.OfficialInformation.IncludeLocalAccount = true;

			//	StateHasChanged();
			//}

			return true;
		}

		[JSInvokable]
		public ValidateStepResultModel ValidateStep(int step)
		{
			var result = new ValidateStepResultModel();
			result.Valid = true;

			return result;
		}

		[JSInvokable]
		public async Task SubmitData()
		{
			
		}
	}
}
