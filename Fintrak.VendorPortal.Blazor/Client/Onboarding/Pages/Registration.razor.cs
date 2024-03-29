
using Blazored.FluentValidation;
using BootstrapBlazor.Components;
using Fintrak.VendorPortal.Blazor.Client.Onboarding.Models;
using Fintrak.VendorPortal.Blazor.Client.Onboarding.Models.Validators;
using Fintrak.VendorPortal.Blazor.Client.Services;
using Fintrak.VendorPortal.Blazor.Shared.Models;
using Fintrak.VendorPortal.Blazor.Shared.Models.Enums;
using Fintrak.VendorPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.VendorPortal.Blazor.Shared.Models.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using Syncfusion.Blazor.DropDowns;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Fintrak.VendorPortal.Blazor.Client.Onboarding.Pages
{
	public partial class Registration
    {
		[Inject]
		public IBankService? BankService { get; set; }

		[Inject]
		public ICategoryService? CategoryService { get; set; }

		[Inject]
		public ICountryService? CountryService { get; set; }

		[Inject]
		public ISubCategoryService? SubCategoryService { get; set; }

		[Inject]
		public IDocumentService? DocumentService { get; set; }

		[Inject]
		public IQuestionService? QuestionService { get; set; }

		[Inject]
		public IOnboardingService? OnboardingService { get; set; }

		[Inject]
		public OnboardingStepService OnboardingStepService { get; set; }

        private DotNetObjectReference<Registration>? dotNetRef;

		public OnboardingModel? PageModel { get; set; } = new ();

		public OfficialInformationValidator? OfficialInformationValidator { get; set; }
		public ContactPersonsValidator? ContactPersonsValidator { get; set; }
		public ContactChannelsValidator? ContactChannelsValidator { get; set; }
		public LocalBankValidator? LocalBanksValidator { get; set; }
		public ForeignBanksValidator? ForeignBanksValidator { get; set; }
		public QuestionnairesValidator? QuestionnairesValidator { get; set; }
		public DocumentsValidator? DocumentsValidator { get; set; }
        public OnboardingValidator PageModelValidator { get; set; }

		private int CurrentStep = 1;
		private bool FormNotValid = true;

		//Lookup
		public List<LookupModel<string, string>>? BankLookup { get; set; }
		public List<LookupModel<string,int>>? CategoryLookup { get; set; }
		public List<LookupModel<string, string>>? CountryLookup { get; set; }
		public List<LookupModel<string, string>>? CountryCallCodeLookup { get; set; }
		public List<LookupModel<string, int>> SubCategoryLookup { get; set; }
		public List<QuestionDto> Questions { get; set; }
		public string RequireDocuments { get; set; }
		public string FileTypes { get; set; }

		public UserDto CurrentUser { get; set; }

		private string InitialBankCode = "";
		private string InitialAccountName = "";
		private string InitialAccountNumber = "";
		private bool InitialAccountValidation = false;

		public List<LookupModel<string, string>> SelectedSubCategories { get; set; } = new();
		public LookupModel<string, int> SelectedSubCategory { get; set; } = new();

		protected override async void OnInitialized()
        {
			SpinnerService.Show();

			dotNetRef = DotNetObjectReference.Create(this);

			var userResponse = await OnboardingService.GetCurrentUser();
			if (userResponse != null && userResponse.Success)
			{
				CurrentUser = userResponse.Result;
			}

		    await LoadCountryLookups();
			await LoadCategoryLookups();
			await LoadBankLookups();
			await LoadQuestions();
			await LoadRequireDocuments();
			await LoadFileTypes();

			await LoadPageData();

			await LoadSubCategoryLookups(PageModel.OfficialInformation.CategoryId);
			
			await LoadTinValidationPatterns();

			PageModelValidator = new(OnboardingStepService);
			
			StateHasChanged();

			SpinnerService.Hide();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
        {
			//await JSRuntime.InvokeVoidAsync("registrationInterop.initSelectPicker");
			await JSRuntime.InvokeVoidAsync("registrationInterop.init", dotNetRef);
            
        }

		private async Task LoadPageData()
		{
			var initializeCompanyName = true;

			var response = await OnboardingService.GetVendor();
			if (response != null && response.Success)
			{
				if(response.Result != null)
				{
					initializeCompanyName = false;

					PageModel = response.Result;

					if (PageModel.OfficialInformation != null)
					{
						if (!string.IsNullOrEmpty(PageModel.OfficialInformation.Country))
						{
							var countryIso = CountryLookup.FirstOrDefault(c=> c.Text == PageModel.OfficialInformation.Country).AlternateText;

							PageModel.OfficialInformation.OfficePhoneIsoCode = countryIso;
							PageModel.OfficialInformation.MobilePhoneIsoCode = countryIso;
						}
					}

					foreach(var contactPerson in PageModel.ContactPersons.ContactPersons)
					{
						if (!string.IsNullOrEmpty(contactPerson.MobilePhoneCallCode))
						{
							var countryIso = CountryLookup.FirstOrDefault(c => c.AlternateText2 == contactPerson.MobilePhoneCallCode).AlternateText;

							contactPerson.MobilePhoneIsoCode = countryIso;
						}
					}

					foreach (var contactChannel in PageModel.ContactChannels.ContactChannels)
					{
						if (contactChannel.Type == Blazor.Shared.Models.Enums.ChannelType.Phone && !string.IsNullOrEmpty(contactChannel.MobilePhoneCallCode))
						{
							var countryIso = CountryLookup.FirstOrDefault(c => c.AlternateText2 == contactChannel.MobilePhoneCallCode).AlternateText;

							contactChannel.MobilePhoneIsoCode = countryIso;
						}
					}

					if (PageModel.LocalBank != null)
					{
						InitialBankCode = PageModel.LocalBank.BankCode;
						InitialAccountName = PageModel.LocalBank.AccountName;
						InitialAccountNumber = PageModel.LocalBank.AccountNumber;
						InitialAccountValidation = PageModel.LocalBank.Validated;
					}				
				}
			}

			if (initializeCompanyName)
			{
				if(CurrentUser != null)
				{
					//PageModel.OfficialInformation = new OfficialInformationModel();
					PageModel.OfficialInformation.CompanyName = CurrentUser.CompanyName;
				}
			}
		}

		private async Task LoadCountryLookups()
		{
			CountryLookup = new List<LookupModel<string, string>>();
			CountryCallCodeLookup = new List<LookupModel<string, string>>();

			var countryLookupResponse = await CountryService.GetCountryLookup();
			if (countryLookupResponse != null && countryLookupResponse.Success)
			{
				CountryLookup = countryLookupResponse.Result;
				CountryCallCodeLookup = CountryLookup.OrderBy(c => c.AlternateText2).ToList();
			}
		}

		private async Task LoadCategoryLookups()
		{
			CategoryLookup = new List<LookupModel<string, int>>();

			var categoryLookupResponse = await CategoryService.GetCategoryLookup();
			if (categoryLookupResponse != null && categoryLookupResponse.Success)
			{
				CategoryLookup = categoryLookupResponse.Result;
			}
		}

		private async Task LoadSubCategoryLookups(int? categoryId)
		{
			SubCategoryLookup = new List<LookupModel<string, int>>();

			if (categoryId != null)
			{
				var subCategoryLookupResponse = await SubCategoryService.GetSubCategoryLookup(categoryId);
				if (subCategoryLookupResponse != null && subCategoryLookupResponse.Success)
				{
					SubCategoryLookup = subCategoryLookupResponse.Result;
				}
			}
		}

		private async Task LoadBankLookups()
		{
			BankLookup = new List<LookupModel<string, string>>();

			var bankLookupResponse = await BankService.GetBankLookup();
			if (bankLookupResponse != null && bankLookupResponse.Success)
			{
				BankLookup = bankLookupResponse.Result;
			}
		}

		private async Task LoadQuestions()
		{
			Questions = new List<QuestionDto>();

			var response = await QuestionService.GetQuestions();
			if (response != null && response.Success)
			{
				Questions = response.Result;
			}

			if(PageModel.Questionnaires.Questionnaires.Count < 1)
			{
				foreach(var question in Questions)
				{
					PageModel.Questionnaires.Questionnaires.Add(new QuestionnaireModel
					{
						QuestionId = question.Id,
						Question = question.Title,
						Compulsory = question.Compulsory,
						Response = ""
					});
				}
			}
		}

		private async Task LoadRequireDocuments()
		{
			RequireDocuments = "";

			var response = await DocumentService.GetDocumentRequirements();
			if (response != null && response.Success)
			{
				RequireDocuments = string.Join(",", response.Result);
			}
		}

		private async Task LoadFileTypes()
		{
			FileTypes = "";

			var response = await DocumentService.GetFileTypes();
			if (response != null && response.Success)
			{
				FileTypes = string.Join(",", response.Result);
			}
		}

		private async Task LoadTinValidationPatterns()
		{
			var response = await OnboardingService.GetTinValidationPatterns();
			if (response != null && response.Success)
			{
				OnboardingStepService.TinValidationPatterns = response.Result;
			}
		}

		//private async void HandleFieldChanged(object sender, FieldChangedEventArgs e)
		//{
		//	//await Task.Yield();
		//	//FormNotValid = false;

		//	//FieldIdentifier tempFieldIdentifier = e.FieldIdentifier;

			
		//	//EditContext.NotifyValidationStateChanged();
		//	StateHasChanged();

		//}

		[JSInvokable]
		public bool SetCurrentStep(int step, int newStep)
		{
			CurrentStep = step;
		
			if (CurrentStep == 4)
			{
				if (!PageModel.OfficialInformation.UseForeignAccount)
				{
					PageModel.OfficialInformation.IncludeLocalAccount = true;
					PageModel.LocalBank.IsCompulsory = true;
				}
				else if (PageModel.LocalBank == null || string.IsNullOrEmpty(PageModel.LocalBank.AccountNumber))
				{
					PageModel.OfficialInformation.IncludeLocalAccount = false;
                    PageModel.LocalBank.IsCompulsory = false;
                }

				StateHasChanged();
			}

			OnboardingStepService.CurrentStep = CurrentStep;

			return true;
		}

		[JSInvokable]
        public async Task<ValidateStepResultModel> ValidateStep(int step)
        {
			var result = new ValidateStepResultModel();

			if (step == 1)
			{
				if (!PageModel.OfficialInformation.UseForeignAccount)
				{
					PageModel.OfficialInformation.IncludeLocalAccount = true;
					PageModel.LocalBank.IsCompulsory = true;
				}
                else if (PageModel.LocalBank == null || string.IsNullOrEmpty(PageModel.LocalBank.AccountNumber))
                {
                    PageModel.OfficialInformation.IncludeLocalAccount = false;
                    PageModel.LocalBank.IsCompulsory = false;
                }
            }
			else if(step == 2)
			{
				if(PageModel.ContactPersons.ContactPersons.Count > 0)
				{
					var contactNames = PageModel.ContactPersons.ContactPersons.Select(c => c.Name).Distinct().ToList();
					if (contactNames.Count != PageModel.ContactPersons.ContactPersons.Count)
					{
						result.Errors.Add($"All contact names must be unique.");
						result.Valid = false;

						return result;
					}

					var contactEmails = PageModel.ContactPersons.ContactPersons.Select(c => c.Email).Distinct().ToList();
					if (contactEmails.Count != PageModel.ContactPersons.ContactPersons.Count)
					{
						result.Errors.Add($"All contact emails must be unique.");
						result.Valid = false;

						return result;
					}

					var contactPhones = PageModel.ContactPersons.ContactPersons.Select(c => $"{c.MobilePhoneCallCode} - {c.MobilePhoneNumber}").Distinct().ToList();
					if (contactPhones.Count != PageModel.ContactPersons.ContactPersons.Count)
					{
						result.Errors.Add($"All contact phones must be unique.");
						result.Valid = false;

						return result;
					}
				}
			}
			else if (step == 3)
			{
				if (PageModel.ContactChannels.ContactChannels.Count > 0)
				{
					var channelEmails = PageModel.ContactChannels.ContactChannels.Where(c => c.Type == ChannelType.Email).Select(c => c.Email).Distinct().ToList();
					if (channelEmails.Count != PageModel.ContactChannels.ContactChannels.Where(c => c.Type == ChannelType.Email).ToList().Count)
					{
						result.Errors.Add($"All channel emails must be unique.");
						result.Valid = false;

						return result;
					}

					var channelPhones = PageModel.ContactChannels.ContactChannels.Where(c => c.Type == ChannelType.Phone).Select(c => $"{c.MobilePhoneCallCode} - {c.MobilePhoneNumber}").Distinct().ToList();
					if (channelPhones.Count != PageModel.ContactChannels.ContactChannels.Where(c => c.Type == ChannelType.Phone).ToList().Count)
					{
						result.Errors.Add($"All channel phones must be unique.");
						result.Valid = false;

						return result;
					}
				}
			}
			else if (step == 4)
			{
				PageModel.LocalBank.IsCompulsory = PageModel.OfficialInformation.IncludeLocalAccount;

				if (PageModel.LocalBank.BankCode != InitialBankCode || PageModel.LocalBank.AccountName != InitialAccountName || PageModel.LocalBank.AccountNumber != InitialAccountNumber)
				{
					PageModel.LocalBank.Validated = false;
				}
				else if (InitialAccountValidation)
				{
					PageModel.LocalBank.Validated = true;
				}
			}
			else if (step == 5)
			{
				if (PageModel.OfficialInformation.UseForeignAccount && PageModel.ForeignBanks.ForeignBanks.Count == 0)
				{
					result.Errors.Add("Atleast one foreign account is require.");
					result.Valid = false;

					return result;
				}

				if (PageModel.ForeignBanks.ForeignBanks.Count > 0)
				{
					var accountNumbers = PageModel.ForeignBanks.ForeignBanks.Select(c => c.AccountNumber).Distinct().ToList();
					if (accountNumbers.Count != PageModel.ForeignBanks.ForeignBanks.Count)
					{
						result.Errors.Add("All foreign account numbers must be unique.");
						result.Valid = false;

						return result;
					}
				}
			}
			else if(step == 7)
			{
				if (PageModel.Documents.Documents.Count > 0)
				{
					var documentTitles = PageModel.Documents.Documents.Select(c => c.Title).Distinct().ToList();
					if (documentTitles.Count != PageModel.Documents.Documents.Count)
					{
						result.Errors.Add("All document title must be unique.");
						result.Valid = false;

						return result;
					}
				}
					
			}
			else
				result.Valid = true;

			//var a = FluentValidationValidator.Validate();

			var validationResult = PageModelValidator.Validate(PageModel);

			result.Valid = validationResult.IsValid;
			return result;

		}

		[JSInvokable]
		public async Task SubmitData()
		{
			SpinnerService.Show();

			var response = await OnboardingService.OnboardVendor(PageModel);

			SpinnerService.Hide();

			if (response != null && response.Success && response.Result != null)
			{
				var icon = response.Success ? "success" : "error";
				await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "Vendor information saved successfully.", icon, "Ok");

				NavManager.NavigateTo("/onboarding/profile");
			}
			else
			{
				await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", $"Unable to onboard customer at this time. {response.Message}", "error", "Ok");
			}
		}

		private Task OnIncorporationDateValueChanged(DateTime? value)
		{
			PageModel.OfficialInformation.IncorporationDate = value;

			return Task.CompletedTask;
		}

		private Task OnCountryValueChanged(string value)
		{
			PageModel.OfficialInformation.Country = value;

			var country = CountryLookup.FirstOrDefault(c => c.Text == value);
			if(country != null) 
			{
				PageModel.OfficialInformation.OfficePhoneCallCode = country.AlternateText2;
				PageModel.OfficialInformation.OfficePhoneIsoCode = country.AlternateText;
				PageModel.OfficialInformation.MobilePhoneCallCode = country.AlternateText2;
				PageModel.OfficialInformation.MobilePhoneIsoCode = country.AlternateText;
			}

			return Task.CompletedTask;
		}

		private async Task OnCategoryValueChanged(int? value)
		{
			SpinnerService.Show();

			PageModel.OfficialInformation.CategoryId = null;
			PageModel.OfficialInformation.CategoryName = string.Empty;

			PageModel.OfficialInformation.SubCategoryIds = new int?[] { }; ;
			PageModel.OfficialInformation.SubCategoryNames = "";

			var category = CategoryLookup.FirstOrDefault(c => c.Value == value);

			if (category != null) 
			{
				PageModel.OfficialInformation.CategoryId = category.Value;
				PageModel.OfficialInformation.CategoryName = category.Text;

				//ValidationMessages.Clear(new FieldIdentifier(PageModel.OfficialInformation, "CategoryId"));
			}

			await LoadSubCategoryLookups(PageModel.OfficialInformation.CategoryId);

			//EditContext.NotifyValidationStateChanged();

			SpinnerService.Hide();
		}

		private async Task OnSubCategoriesValueChanged(MultiSelectChangeEventArgs<int?[]> args)
		{
			PageModel.OfficialInformation.SubCategoryIds = new int?[] { };
			PageModel.OfficialInformation.SubCategoryNames = "";

			var values = args.Value as int?[];
			if (values != null)
			{
				var subCategories = SubCategoryLookup.Where(c => values.Contains(c.Value)).ToList();
				if (subCategories != null)
				{
					PageModel.OfficialInformation.SubCategoryIds = values;
					PageModel.OfficialInformation.SubCategoryNames = string.Join(",", subCategories.Select(c => c.Text).ToList());

					//ValidationMessages.Clear(new FieldIdentifier(PageModel.OfficialInformation, "SubCategoryIds"));
				}
			}

			//EditContext.NotifyValidationStateChanged();
		}

		//private async Task OnSubCategoriesValueChanged(int[]? values)
		//{
		//	PageModel.OfficialInformation.SubCategoryIds = new int[] { };
		//	PageModel.OfficialInformation.SubCategoryNames = "";

		//	if (values != null)
		//	{
		//		var subCategories = SubCategoryLookup.Where(c => values.Contains(c.Value)).ToList();
		//		if (subCategories != null)
		//		{
		//			PageModel.OfficialInformation.SubCategoryIds = values;
		//			PageModel.OfficialInformation.SubCategoryNames = string.Join(",", subCategories.Select(c => c.Text).ToList());

		//			//ValidationMessages.Clear(new FieldIdentifier(PageModel.OfficialInformation, "SubCategoryIds"));
		//		}
		//	}

		//	//EditContext.NotifyValidationStateChanged();
		//}

		private async Task OnBankValueChanged(string value)
		{
			PageModel.LocalBank.BankName = string.Empty;
			PageModel.LocalBank.BankCode = string.Empty;

			var item = BankLookup.FirstOrDefault(c => c.Text == value);
			if (item != null)
			{
				PageModel.LocalBank.BankName = item.Text;
				PageModel.LocalBank.BankCode = item.AlternateText;
			}
		}

		void AddContactPerson()
		{
			PageModel.ContactPersons.ContactPersons.Add(new ContactPersonModel
			{
				FormId = Guid.NewGuid()
			});

			StateHasChanged();
		}

		protected void RemoveContactPerson(Guid formId)
		{
			PageModel.ContactPersons.ContactPersons.RemoveAll(c=> c.FormId == formId);
			StateHasChanged();
		}

		protected void MarkContactPersonAsDefaultHandler(bool value, Guid formId)
		{
			var previousDefault = PageModel.ContactPersons.ContactPersons.FirstOrDefault(c => c.Default);
			if (previousDefault != null)
			{
				previousDefault.Default = false;
				//previousDefault.RecordChanged = true;
			}

			var contactPerson = PageModel.ContactPersons.ContactPersons.FirstOrDefault(c => c.FormId == formId);
			contactPerson.Default = value;

			//if (document.NewRecord)
			//contactPerson.RecordChanged = true;

			StateHasChanged();
		}

		protected void ContactPersonNameHandler(string value, Guid formId)
		{
			var contactPerson = PageModel.ContactPersons.ContactPersons.FirstOrDefault(c => c.FormId == formId);
			contactPerson.Name = value;

			//if (document.NewRecord)
			//document.RecordChanged = true;
			
			StateHasChanged();
		}

		protected void ContactPersonCallCodeHandler(string value, Guid formId)
		{
			var contactPerson = PageModel.ContactPersons.ContactPersons.FirstOrDefault(c => c.FormId == formId);
			contactPerson.MobilePhoneCallCode = value;
			contactPerson.MobilePhoneIsoCode = CountryLookup.FirstOrDefault(c=> c.AlternateText2 == value).AlternateText;

			StateHasChanged();
		}

		void AddContactChannel()
		{
			PageModel.ContactChannels.ContactChannels.Add(new ContactChannelModel
			{
				FormId = Guid.NewGuid()
			});

			StateHasChanged();
		}

		protected void ContactChannelTypeHandler(ChannelType value, Guid formId)
		{
			var contactChannel = PageModel.ContactChannels.ContactChannels.FirstOrDefault(c => c.FormId == formId);
			contactChannel.Type = value;

			if(value == ChannelType.Email)
			{
				contactChannel.Email = "";
			}
			else
			{
				contactChannel.MobilePhoneCallCode = "";
				contactChannel.MobilePhoneIsoCode = "";
				contactChannel.MobilePhoneNumber = "";
			}

			StateHasChanged();
		}

		protected void ContactChannelCallCodeHandler(string value, Guid formId)
		{
			var contactChannel = PageModel.ContactChannels.ContactChannels.FirstOrDefault(c => c.FormId == formId);
			contactChannel.MobilePhoneCallCode = value;
			contactChannel.MobilePhoneIsoCode = CountryLookup.FirstOrDefault(c => c.AlternateText2 == value).AlternateText;

			StateHasChanged();
		}

		protected void RemoveContactChannel(Guid formId)
		{
			PageModel.ContactChannels.ContactChannels.RemoveAll(c => c.FormId == formId);
			StateHasChanged();
		}

		protected async Task ValidateAccount()
		{
			PageModel.LocalBank.Validated = false;

			var response = await BankService.ValidateAccount(PageModel.LocalBank.BankCode, PageModel.LocalBank.AccountName, PageModel.LocalBank.AccountNumber);
			if (response != null && response.Success && response.Result != null)
			{
				var icon = response.Result.Valid ? "success" : "error";

				PageModel.LocalBank.Validated = response.Result.Valid;

				if (response.Result.Valid)
				{
					InitialBankCode = PageModel.LocalBank.BankCode;
					InitialAccountName = PageModel.LocalBank.AccountName;
					InitialAccountNumber = PageModel.LocalBank.AccountNumber;
				}
				
				await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", response.Result.Message, icon,"Ok");
			}
			else
			{
				await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "Unable to validate account at this time.", "error", "Ok");
			}
		}

		void AddForeignBank()
		{
			PageModel.ForeignBanks.ForeignBanks.Add(new ForeignBankModel
			{
				FormId = Guid.NewGuid()
			});

			StateHasChanged();
		}

		protected void RemoveForeignBank(Guid formId)
		{
			PageModel.ForeignBanks.ForeignBanks.RemoveAll(c => c.FormId == formId);
			StateHasChanged();
		}

		void AddDocument()
		{
			PageModel.Documents.Documents.Add(new DocumentModel
			{
				FormId = Guid.NewGuid()
			});

			StateHasChanged();
		}

		protected void RemoveDocument(Guid formId)
		{
			PageModel.Documents.Documents.RemoveAll(c => c.FormId == formId);
			StateHasChanged();
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

					var document = PageModel.Documents.Documents.FirstOrDefault(c => c.FormId == formId);
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

		private async Task<LookupModel<string,string>> getPhoneByIdAsync(List<LookupModel<string, string>> allItems, string id, CancellationToken token)
		{
			return allItems.FirstOrDefault(c=> c.Value == id);
		}

	}
}
