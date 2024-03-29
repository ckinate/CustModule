using Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators;
using Fintrak.CustomerPortal.Blazor.Client.Services;
using Fintrak.CustomerPortal.Blazor.Shared.Extensions;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Reflection.Metadata;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Pages
{
	public partial class PaymentReceiptyDialog
	{
        [Inject]
        public IOnboardingService? OnboardingService { get; set; }

        [Inject]
        public IDocumentService DocumentService { get; set; }

        private DotNetObjectReference<PaymentReceiptyDialog> dotNetRef;

        public bool ShowDialog { get; set; }

        private EditContext EditContext;
		public PortalInvoicePaymentReceiptDto PageModel { get; set; } = new();
        public PortalInvoicePaymentReceiptValidator PageModelValidator { get; set; }

        [Parameter]
		public EventCallback OnReceiptSubmittedResponse { get; set; }

        protected override async Task OnInitializedAsync()
        {
			dotNetRef = DotNetObjectReference.Create(this);

            PageModel = new();

			PageModelValidator = new();
		}

        public void Show(string invoiceCode)
		{
			ShowDialog = true;
			BlockPage();

            PageModel = new PortalInvoicePaymentReceiptDto
			{
				InvoiceCode = invoiceCode,
			};

			PageModelValidator = new();

            //EditContext = new EditContext(PageModel);

            //EditContext.OnFieldChanged += HandleFieldChanged;

            UnBlockPage();
			StateHasChanged();
		}

        private void HandleFieldChanged(object sender, FieldChangedEventArgs e)
        {
            StateHasChanged();
        }

        async Task OnSaveClick()
		{
			SpinnerService.Show();

			var response = await OnboardingService.SubmitPaymentReceipt(PageModel);

			SpinnerService.Hide();

			if (response.Success)
			{
				var icon = response.Success ? "success" : "error";
				await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "Payment receipt uploaded successfully.", icon, "Ok");

				ShowDialog = false;
                StateHasChanged();

                await OnReceiptSubmittedResponse.InvokeAsync();
            }
			else
			{
				await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "Unable to submit payment receipt.", "error", "Ok");
			}		
		}

		public void Close()
		{
			ShowDialog = false;
			StateHasChanged();
		}

        async Task HandleFileSelection(InputFileChangeEventArgs e)
        {
            SpinnerService.Show();

            var maxAllowedFiles = 1;
            var maxSize = 100 * 1024 * 1024;

            try
            {
                var maxFileSizeResult = await DocumentService.GetMaximumFileSize(null);
                if (maxFileSizeResult != null)
                {
                    maxSize = int.Parse(maxFileSizeResult.Result.ToString());
                }

                foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
                {
                    var buffer = new byte[file.Size];
                    await file.OpenReadStream(maxSize).ReadAsync(buffer);

                    PageModel.FileData = buffer;
                    PageModel.FileExtensionType = FileHelper.GetFileExtension(file.ContentType);
                    PageModel.FileSize = file.Size;

                    PageModel.SelectedFileName = file.Name;
                }

                SpinnerService.Hide();
            }
            catch (Exception ex)
            {
                //Supplied file with size 4066391 bytes exceeds the maximum of 2097152 bytes.
                if (ex.Message.Contains("bytes exceeds the maximum"))
                {
                    await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", $"File must be less than {maxSize}B.", "error", "Ok");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "Error while uploading file.", "error", "Ok");
                }

                SpinnerService.Hide();
                StateHasChanged();
            }
        }
    }
}
