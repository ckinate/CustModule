using Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models;
using Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators;
using Fintrak.CustomerPortal.Blazor.Client.Services;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Pages
{
	public partial class OnboardProduct
	{
        [Parameter]
        public int Id { get; set; }

        [Inject]
		public IDocumentService? DocumentService { get; set; }

		[Inject]
		public IDocumentTypeService? DocumentTypeService { get; set; }

		[Inject]
		public IOnboardingService? OnboardingService { get; set; }

		[Inject]
		public IProductService? ProductService { get; set; }

		private DotNetObjectReference<OnboardProduct>? dotNetRef;

		public OnboardingProductModel? PageModel { get; set; } = new();

		public OnboardingProductValidator PageModelValidator { get; set; }

		public List<LookupModel<string, int>> ProductLookup { get; set; }
		public List<LookupModel<string, int>> ContactPersonLookup { get; set; }
		public List<LookupModel<string, int>> AccountLookup { get; set; }
		public List<LookupModel<string, int>> DocumentTypeLookup { get; set; }
		public List<CustomFieldDto> CustomFields { get; set; }

		public string RequireDocuments { get; set; }
		public string FileTypes { get; set; }

		public CustomerModel Customer { get; set; }

		public List<Models.UpsertCustomFieldDto> AdditionalInformation { get; set; } = new();

		protected override async void OnInitialized()
		{
			SpinnerService.Show();

			dotNetRef = DotNetObjectReference.Create(this);

			var customerResponse = await OnboardingService.GetCurrentCustomer();
			if (customerResponse != null && customerResponse.Success)
			{
				Customer = customerResponse.Result;
			}

			await LoadProductLookup();
			await LoadContactPersonLookup();
			await LoadAccountLookup();

			await LoadPageData();

			await LoadRequireDocumentTypes(PageModel.ProductId);
			await LoadCustomFields(PageModel.ProductId);

			PageModelValidator = new();

			StateHasChanged();

			SpinnerService.Hide();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{

			}
		}

		private async Task LoadPageData()
		{
			var initializeCompanyName = true;

			if (Id > 0)
			{
				var response = await OnboardingService.GetProductDetail(Id);
				if (response != null && response.Success)
				{
					if (response.Result != null)
					{
						initializeCompanyName = false;

						PageModel = response.Result;
					}
				}
			}
			
		}

		private async Task LoadProductLookup()
		{
			ProductLookup = new List<LookupModel<string, int>>();

			var lookupResponse = await ProductService.GetProductLookup(Customer.Code);
			if (lookupResponse != null && lookupResponse.Success)
			{
				ProductLookup = lookupResponse.Result;
				//StateHasChanged();
			}
		}

		private async Task LoadContactPersonLookup()
		{
			ContactPersonLookup = new List<LookupModel<string, int>>();

			var lookupResponse = await OnboardingService.GetContactPersonLookup();
			if (lookupResponse != null && lookupResponse.Success)
			{
				ContactPersonLookup = lookupResponse.Result;
			}
		}

		private async Task LoadAccountLookup()
		{
			AccountLookup = new List<LookupModel<string, int>>();

			var lookupResponse = await OnboardingService.GetAccountLookup(AccountType.Fee);
			if (lookupResponse != null && lookupResponse.Success)
			{
				AccountLookup = lookupResponse.Result;
			}
		}

		private async Task OnProductValueChanged(int value)
		{
			SpinnerService.Show();

			PageModel.ProductId = 0;
			PageModel.ProductCode = string.Empty;
			PageModel.ProductName = string.Empty;

			var product = ProductLookup.FirstOrDefault(c => c.Value == value);

			if (product != null)
			{
				PageModel.ProductId = value;
				PageModel.ProductName = product.Text;
				PageModel.ProductCode = product.AlternateText;
			}

			await LoadRequireDocumentTypes(PageModel.ProductId);
			await LoadCustomFields(PageModel.ProductId);

			SpinnerService.Hide();
		}

		private async Task OnContactPersonValueChanged(int value)
		{
			PageModel.ContactPersonId = value;
		}

		private async Task OnAccountValueChanged(int value)
		{
			PageModel.AccountId = value;
		}

		private async Task LoadRequireDocumentTypes(int? productId)
		{
			RequireDocuments = "";
			DocumentTypeLookup = new List<LookupModel<string, int>>();

			if (productId.HasValue)
			{
				var response = await DocumentTypeService.GetDocumentTypeLookupByProduct(productId.Value, Customer.Code);
				if (response != null && response.Success)
				{
					DocumentTypeLookup = response.Result;
					RequireDocuments = string.Join(",", response.Result);
				}
			}
		}

		private async Task LoadCustomFields(int? productId)
		{
			CustomFields = new List<CustomFieldDto>();

			if (productId.HasValue)
			{
				var response = await ProductService.GetProductCustomFields(productId.Value);
				if (response != null && response.Success)
				{
					CustomFields = response.Result;
				}
			}

			LoadAdditionalInformation();

			StateHasChanged();
		}

		private void LoadAdditionalInformation()
		{
			AdditionalInformation = new();

			foreach (var field in CustomFields)
			{
				AdditionalInformation.Add(new Models.UpsertCustomFieldDto
				{
					CustomFieldId = field.CustomFieldId,
					CustomField = field.CustomField,
					IsCompulsory = field.IsCompulsory,
				});
			}

			if (PageModel != null && PageModel.AdditionalInformations != null && PageModel.AdditionalInformations.Count > 0)
			{
				foreach(var item in AdditionalInformation)
				{
					var info = PageModel.AdditionalInformations.FirstOrDefault(c=> c.CustomFieldId == item.CustomFieldId);
					if(info != null)
					{
						item.Response = info.Response;
					}
				}
			}		
		}

		private async Task OnOperationModeValueChanged(OperationMode value)
		{
			PageModel.OperationMode = value;
		}

		void AddDocument()
		{
			PageModel.Documents.Add(new DocumentModel
			{
				FormId = Guid.NewGuid()
			});

			StateHasChanged();
		}

		protected void RemoveDocument(Guid formId)
		{
			PageModel.Documents.RemoveAll(c => c.FormId == formId);
			StateHasChanged();
		}

		private async Task OnDocumentTypeValueChanged(int? value, Guid formId)
		{
			SpinnerService.Show();

			var document = PageModel.Documents.FirstOrDefault(c => c.FormId == formId);

			document.DocumentTypeId = null;
			document.DocumentTypeName = string.Empty;
			document.Title = string.Empty;

			var documentType = DocumentTypeLookup.FirstOrDefault(c => c.Value == value);
			if (documentType != null)
			{
				document.DocumentTypeId = documentType.Value;
				document.DocumentTypeName = documentType.Text;
				document.Title = documentType.Text;

				//ValidationMessages.Clear(new FieldIdentifier(PageModel.OfficialInformation, "CategoryId"));
			}

			SpinnerService.Hide();
		}

		private Task OnIssueDateValueChanged(DateTime? value, Guid formId)
		{
			var document = PageModel.Documents.FirstOrDefault(c => c.FormId == formId);
			document.IssueDate = value;

			return Task.CompletedTask;
		}

		private Task OnExpiryDateValueChanged(DateTime? value, Guid formId)
		{
			var document = PageModel.Documents.FirstOrDefault(c => c.FormId == formId);
			document.ExpiryDate = value;

			return Task.CompletedTask;
		}

		async Task HandleFileSelection(InputFileChangeEventArgs e, Guid formId)
		{
			SpinnerService.Show();

			var maxAllowedFiles = 1;
			var maxSize = 100 * 1024 * 1024;

			try
			{
				foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
				{
					var buffer = new byte[file.Size];
					await file.OpenReadStream(maxSize).ReadAsync(buffer);

					var document = PageModel.Documents.FirstOrDefault(c => c.FormId == formId);
					document.FileData = buffer;
					document.ContentType = file.ContentType;
					document.Size = file.Size;

					document.SelectedFileName = file.Name;
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

		public async Task SubmitData()
		{
			SpinnerService.Show();

			PageModel.AdditionalInformations = AdditionalInformation;

			var response = await OnboardingService.OnboardProduct(PageModel);

			SpinnerService.Hide();

			if (response != null && response.Success && response.Result != null)
			{
				var icon = response.Success ? "success" : "error";
				await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "Customer product information saved successfully.", icon, "Ok");

				NavManager.NavigateTo("/onboarding/dashboard");
			}
			else
			{
				await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", $"Unable to onboard customer product at this time. {response.Message}", "error", "Ok");
			}
		}
	}
}
