using Fintrak.CustomerPortal.Blazor.Client.Services;
using Fintrak.CustomerPortal.Blazor.Shared.Extensions;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Net.Mime;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Pages
{
	public partial class Signing
	{
		[Inject]
		public IOnboardingService OnboardingService { get; set; }

		[Inject]
		public IDocumentService? DocumentService { get; set; }

		public List<CustomerOnboardingDocumentDto> PageModel { get; set; } = default!;
		public UserDto CurrentUser { get; set; }

		protected override async void OnInitialized()
		{
			SpinnerService.Show();

			PageModel = new();

			await LoadCurrentUser();
			await LoadData();

			SpinnerService.Hide();
			StateHasChanged();
		}

		private async Task LoadCurrentUser()
		{
			var userResponse = await OnboardingService.GetCurrentUser();
			if (userResponse != null && userResponse.Success)
			{
				CurrentUser = userResponse.Result;
			}
		}

		private async Task LoadData()
		{
			var response = await OnboardingService.GetLegalDocuments();
			if (response != null && response.Success)
			{
				PageModel = response.Result;
			}
		}

		async Task HandleFileSelection(InputFileChangeEventArgs e, int? documentId)
		{
			SpinnerService.Show();

			var maxAllowedFiles = 1;
			var maxSize = 100 * 1024 * 1024;

			try
			{
				var document = PageModel.FirstOrDefault(c => c.Id == documentId);

				var maxFileSizeResult = await DocumentService.GetMaximumFileSize(document.PartnerDocumentTypeId);
				if (maxFileSizeResult != null)
				{
					maxSize = int.Parse(maxFileSizeResult.Result.ToString());
				}

				foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
				{
					var buffer = new byte[file.Size];
					await file.OpenReadStream(maxSize).ReadAsync(buffer);

					document.FileUploadData = new FileUploadDto();

					document.FileUploadData.FileData = buffer;
					document.FileUploadData.FileExtensionType = FileHelper.GetFileExtension(file.ContentType);
					document.FileUploadData.FileSize = file.Size;

					document.SelectedFileName = file.Name;
					document.FileUploaded = true;
				}

				SpinnerService.Hide();
			}
			catch (Exception ex)
			{
				//Supplied file with size 4066391 bytes exceeds the maximum of 2097152 bytes.
				if (ex.Message.Contains("bytes exceeds the maximum"))
				{
					await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "File must be less than 100 MB.", "error", "Ok");
				}
				else
				{
					await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "Error while uploading file.", "error", "Ok");
				}

				SpinnerService.Hide();
				StateHasChanged();
			}
		}

		async Task SubmitDocuments()
		{
			SpinnerService.Show();

			if (PageModel.Any(c=> !c.FileUploaded))
			{
				await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "One or more documents has not been uploaded", "error", "Ok");
			}
			else
			{
				var response = await OnboardingService.SubmitLegalDocuments(PageModel);
				if (response != null && response.Success && response.Result)
				{
					var icon = response.Success ? "success" : "error";
					await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "Signed documents uploaded successfully.", icon, "Ok");
					await LoadData();
				}
				else
				{
					await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "Unable to submit signed documents", "error", "Ok");
				}
			}

			SpinnerService.Hide();
			StateHasChanged();
		}
	}
}
