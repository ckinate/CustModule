using Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models;
using Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators;
using Fintrak.CustomerPortal.Blazor.Client.Services;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Linq;
using System.Xml.Linq;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Pages
{
	public partial class ViewRegistration
	{
		[Inject]
		public IOnboardingService? OnboardingService { get; set; }

		private DotNetObjectReference<ViewRegistration>? dotNetRef;

		public OnboardingModel? PageModel { get; set; }

		private int CurrentStep = 1;

		public List<CustomFieldModel> CustomFields { get; set; }

		protected override async void OnInitialized()
		{
			SpinnerService.Show();

			PageModel = new();
			
			await LoadPageData();
			await LoadCustomFields();

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
			var response = await OnboardingService.GetCustomer();
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

		private async Task LoadCustomFields()
		{
			CustomFields = new List<CustomFieldModel>();

			var response = await OnboardingService.GetCustomFields();
			if (response != null && response.Success && response.Result != null)
			{
				CustomFields = response.Result.CustomFields;
			}

			if (PageModel.CustomFields.CustomFields.Count < 1)
			{
				foreach (var customField in CustomFields)
				{
					PageModel.CustomFields.CustomFields.Add(customField);
				}
			}
		}
	}
}
