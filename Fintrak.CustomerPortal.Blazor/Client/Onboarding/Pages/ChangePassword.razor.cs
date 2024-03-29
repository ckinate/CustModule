using Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models;
using Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Pages
{
	public partial class ChangePassword
	{
		[Inject]
		public IOnboardingService? OnboardingService { get; set; }

		public ChangePasswordModel? PageModel { get; set; } = new ChangePasswordModel();
		public UserDto CurrentUser { get; set; }

		public ChangePasswordValidator? ChangePasswordValidator { get; set; }
		public List<string> Errors { get; set; }  = new List<string>();

		//private EditContext EditContext;
		//private ValidationMessageStore ValidationMessages;

		protected override async void OnInitialized()
		{
			SpinnerService.Show();

			PageModel = new();

			ChangePasswordValidator = new();

			//EditContext = new EditContext(PageModel);
			//EditContext.OnFieldChanged += HandleFieldChanged;
			//ValidationMessages = new ValidationMessageStore(EditContext);

			await LoadData();

			SpinnerService.Hide();
			StateHasChanged();
		}

		private async Task LoadData()
		{
			var userResponse = await OnboardingService.GetCurrentUser();
			if (userResponse != null && userResponse.Success)
			{
				CurrentUser = userResponse.Result;
			}
		}

		//private async void HandleFieldChanged(object sender, FieldChangedEventArgs e)
		//{
		//	await Task.Yield();

		//	ValidationMessages.Clear(e.FieldIdentifier);

		//	var validationResult = ChangePasswordValidator?.Validate(PageModel);
		//	if (!validationResult.IsValid)
		//	{
		//		//var validationDictionary = validationResult.ToDictionary();

		//		//if (validationDictionary.ContainsKey(e.FieldIdentifier.FieldName))
		//		//{
		//		//	ValidationMessages.Clear(e.FieldIdentifier);
		//		//	ValidationMessages.Add(e.FieldIdentifier, validationDictionary[e.FieldIdentifier.FieldName]);
		//		//}
		//	}
		//	else
		//	{
		//		ValidationMessages.Clear(e.FieldIdentifier);
		//	}

		//	EditContext.NotifyValidationStateChanged();
		//}

		private async void HandleValidSubmit()
		{
			SpinnerService.Show();

			var response = await OnboardingService.ChangePassword(new Blazor.Shared.Models.Users.ChangePasswordDto
			{
				OldPassword = PageModel.OldPassword,
				NewPassword = PageModel.NewPassword,
				ConfirmPassword = PageModel.ConfirmPassword 
			});

			SpinnerService.Hide();

			Errors = new();

			if (response != null && response.Success && response.Result != null)
			{
				var icon = "success";
				StateHasChanged();
				await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "Change password successful.", icon, "Ok");
			}
			else
			{
				if (response.ValidationErrors != null && response.ValidationErrors.Any())
				{
					Errors = response.ValidationErrors;
					StateHasChanged();
				}

				await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "Unable to change password at this time.\r\n Test", "error", "Ok");
			}
		}
	}
}
