
using Blazored.FluentValidation;
using BootstrapBlazor.Components;
using Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models;
using Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators;
using Fintrak.CustomerPortal.Blazor.Client.Services;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using Syncfusion.Blazor.DropDowns;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Pages
{
	public partial class Registration
    {
		[Inject]
		public IBankService? BankService { get; set; }

		[Inject]
		public IInstitutionTypeService? InstitutionTypeService { get; set; }

		[Inject]
		public ICountryService? CountryService { get; set; }

		[Inject]
		public ISectorService? SectorService { get; set; }

		[Inject]
		public IIndustryService? IndustryService { get; set; }

		[Inject]
		public IDocumentTypeService? DocumentTypeService { get; set; }

		[Inject]
		public IDocumentService? DocumentService { get; set; }

		[Inject]
		public IOnboardingService? OnboardingService { get; set; }

		[Inject]
		public OnboardingStepService OnboardingStepService { get; set; }

        private DotNetObjectReference<Registration>? dotNetRef;

		public OnboardingModel? PageModel { get; set; } = new ();

		protected ReviewDialog ReviewDialog { get; set; }

		public OfficialInformationValidator? OfficialInformationValidator { get; set; }
		public ContactPersonsValidator? ContactPersonsValidator { get; set; }
		public ContactChannelsValidator? ContactChannelsValidator { get; set; }
		//public BankAccountsValidator? BankAccountsValidator { get; set; }
		public FeeAccountsValidator? FeeAccountsValidator { get; set; }
		public CommissionAccountsValidator? CommissionAccountsValidator { get; set; }
		public SignatoriesValidator? SignatoriesValidator { get; set; }
		public DocumentsValidator? DocumentsValidator { get; set; }
        public OnboardingValidator PageModelValidator { get; set; }

		private int CurrentStep = 1;
		private bool FormNotValid = true;

		//Lookup
		public List<LookupModel<string, string>>? BankLookup { get; set; }
		public List<LookupModel<string,int>>? SectorLookup { get; set; }
		public List<LookupModel<string, int>>? CountryLookup { get; set; }
		public List<LookupModel<string, int>>? CountryCallCodeLookup { get; set; }
		public List<LookupModel<string, int>>? StateLookup { get; set; }
		public List<LookupModel<string, int>> IndustryLookup { get; set; }
		public List<LookupModel<string, int>> ParentInstitutionTypeLookup { get; set; }
		public List<LookupModel<string, int>> ChildrenInstitutionTypeLookup { get; set; }
		public List<LookupModel<string, int>> DocumentTypeLookup { get; set; }
        public List<CustomFieldModel> CustomFields { get; set; }
		public List<LookupModel>? ParentLookup { get; set; }

		public string RequireDocuments { get; set; }
		public string FileTypes { get; set; }

		public UserDto CurrentUser { get; set; }

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
			await LoadBankLookups();
			await LoadRootInstitutionTypeLookups();
			await LoadSetorLookups();
		
			await LoadPageData();

			await LoadStateLookups(PageModel.OfficialInformation.Country);
			await LoadIndustryLookups(PageModel.OfficialInformation.SectorId);
			await LoadChildrenInstitutionTypeLookups(PageModel.OfficialInformation.InstitutionTypeId);

			if (!PageModel.OfficialInformation.HasChildInstitutionType)
			{
				await LoadRequireDocumentTypes(PageModel.OfficialInformation.InstitutionTypeId);
			}
			else
			{
				await LoadRequireDocumentTypes(PageModel.OfficialInformation.ChildInstitutionTypeId);
			}

			await LoadParentLookups();

			await LoadCustomFields();

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

			var response = await OnboardingService.GetCustomer();
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

						if (PageModel.OfficialInformation.InstitutionTypeId.HasValue)
						{
							var type = ParentInstitutionTypeLookup.FirstOrDefault(c => c.Value == PageModel.OfficialInformation.InstitutionTypeId.Value);
							if (type != null)
							{
								if (type.AdditionalData.ContainsKey("HasSettlementBank"))
								{
									PageModel.HasSettlementBank = bool.Parse(type.AdditionalData["HasSettlementBank"].ToString());
									OnboardingStepService.HasSettlementBank = PageModel.HasSettlementBank;
								}

								if (type.AdditionalData.ContainsKey("HasFeeAccount"))
								{
									PageModel.HasFeeAccounts = bool.Parse(type.AdditionalData["HasFeeAccount"].ToString());
									OnboardingStepService.HasFeeAccounts = PageModel.HasFeeAccounts;
								}
							}
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

					foreach (var signatory in PageModel.Signatories.Signatories)
					{
						if (!string.IsNullOrEmpty(signatory.MobileNumberCallCode))
						{
							var countryIso = CountryLookup.FirstOrDefault(c => c.AlternateText2 == signatory.MobileNumberCallCode).AlternateText;

							signatory.MobileNumberIsoCode = countryIso;
						}
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
			CountryLookup = new List<LookupModel<string, int>>();
			CountryCallCodeLookup = new List<LookupModel<string, int>>();

			var countryLookupResponse = await CountryService.GetCountryLookup();
			if (countryLookupResponse != null && countryLookupResponse.Success)
			{
				CountryLookup = countryLookupResponse.Result;
				CountryCallCodeLookup = CountryLookup.OrderBy(c => c.AlternateText).ToList();
			}
		}

		private async Task LoadStateLookups(string? countryName)
		{
			StateLookup = new List<LookupModel<string, int>>();

			if (!string.IsNullOrEmpty(countryName))
			{
				var country = CountryLookup.FirstOrDefault(c=> c.Text == countryName);

				var lookupResponse = await CountryService.GetStateLookup(country.Value);
				if (lookupResponse != null && lookupResponse.Success)
				{
					StateLookup = lookupResponse.Result;
				}
			}
		}

		private async Task LoadRootInstitutionTypeLookups()
		{
			ParentInstitutionTypeLookup = new List<LookupModel<string, int>>();

			var lookupResponse = await InstitutionTypeService.GetParentInstitutionTypeLookup();
			if (lookupResponse != null && lookupResponse.Success)
			{
				ParentInstitutionTypeLookup = lookupResponse.Result;

				if (PageModel.OfficialInformation.InstitutionTypeId.HasValue)
				{
					var type = ParentInstitutionTypeLookup.FirstOrDefault(c => c.Value == PageModel.OfficialInformation.InstitutionTypeId.Value);
					if(type != null)
					{
						if (type.AdditionalData.ContainsKey("HasSettlementBank"))
						{
							PageModel.HasSettlementBank = bool.Parse(type.AdditionalData["HasSettlementBank"].ToString());
							OnboardingStepService.HasSettlementBank = PageModel.HasSettlementBank;
						}

						if (type.AdditionalData.ContainsKey("HasFeeAccount"))
						{
							PageModel.HasFeeAccounts = bool.Parse(type.AdditionalData["HasFeeAccount"].ToString());
							OnboardingStepService.HasFeeAccounts = PageModel.HasFeeAccounts;
						}
					}
				}
			}
		}

		private async Task LoadChildrenInstitutionTypeLookups(int? parentId)
		{
			ChildrenInstitutionTypeLookup = new List<LookupModel<string, int>>();

			if (parentId.HasValue)
			{
				var lookupResponse = await InstitutionTypeService.GetChildrenInstitutionTypeLookup(parentId.Value);
				if (lookupResponse != null && lookupResponse.Success)
				{
					ChildrenInstitutionTypeLookup = lookupResponse.Result;

					if (PageModel.OfficialInformation.ChildInstitutionTypeId.HasValue)
					{
						var type = ChildrenInstitutionTypeLookup.FirstOrDefault(c => c.Value == PageModel.OfficialInformation.ChildInstitutionTypeId.Value);
						if (type != null)
						{
							if (type.AdditionalData.ContainsKey("HasSettlementBank"))
							{
								PageModel.HasSettlementBank = bool.Parse(type.AdditionalData["HasSettlementBank"].ToString());
								OnboardingStepService.HasSettlementBank = PageModel.HasSettlementBank;
							}

							if (type.AdditionalData.ContainsKey("HasFeeAccount"))
							{
								PageModel.HasFeeAccounts = bool.Parse(type.AdditionalData["HasFeeAccount"].ToString());
								OnboardingStepService.HasFeeAccounts = PageModel.HasFeeAccounts;
							}
						}
					}
				}
			}		
		}

		private async Task LoadSetorLookups()
		{
			SectorLookup = new List<LookupModel<string, int>>();

			var lookupResponse = await SectorService.GetSectorLookup();
			if (lookupResponse != null && lookupResponse.Success)
			{
				SectorLookup = lookupResponse.Result;
			}
		}

		private async Task LoadIndustryLookups(int? sectorId)
		{
			IndustryLookup = new List<LookupModel<string, int>>();

			if (sectorId != null)
			{
				var lookupResponse = await IndustryService.GetIndustryLookup(sectorId);
				if (lookupResponse != null && lookupResponse.Success)
				{
					IndustryLookup = lookupResponse.Result;
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

		private async Task LoadRequireDocumentTypes(int? institutionTypeId)
		{
			RequireDocuments = "";
			DocumentTypeLookup = new List<LookupModel<string, int>>();

			if (institutionTypeId.HasValue)
			{
				var response = await DocumentTypeService.GetDocumentTypeLookupByInstitutionType(institutionTypeId.Value);
				if (response != null && response.Success)
				{
					DocumentTypeLookup = response.Result;
					RequireDocuments = string.Join(",", response.Result.Select(c=> c.Text));
				}
			}		
		}

		private async Task LoadParentLookups()
		{
			IndustryLookup = new List<LookupModel<string, int>>();

			var lookupResponse = await OnboardingService.GetParentLookup();
			if (lookupResponse != null && lookupResponse.Success)
			{
				ParentLookup = lookupResponse.Result;
			}
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

		[JSInvokable]
		public async Task<bool> SetCurrentStep(int step, int newStep)
		{
            SpinnerService.Show();

			var doSaving = true;
			if (step < CurrentStep)
				doSaving = false;

            CurrentStep = step;
		
			OnboardingStepService.CurrentStep = CurrentStep;

			//Save stage data
			var previousStep = CurrentStep == 1 ? CurrentStep : CurrentStep - 1;

			if (doSaving)
			{
                var response = await OnboardingService.SavedCustomer(PageModel, previousStep);
				//if (response != null && response.Success && response.Result != null)
				//{
				//	var icon = response.Success ? "success" : "error";
				//	await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "Customer information saved successfully.", icon, "Ok");

				//	NavManager.NavigateTo("/onboarding/profile");
				//}
				//else
				//{
				//	await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", $"Unable to onboard customer at this time. {response.Message}", "error", "Ok");
				//}
			}

			//if(CurrentStep == 9)
			//{
			//	await LoadPageData();
			//	StateHasChanged();
			//}

            SpinnerService.Hide();

            return true;
		}

		[JSInvokable]
        public async Task<ValidateStepResultModel> ValidateStep(int step)
        {
			var result = new ValidateStepResultModel();

			if (step == 1)
			{
				var step1ValidationResult = await OnboardingService.ValidateCustomer(PageModel, step);
				if (step1ValidationResult != null && !step1ValidationResult.Result)
				{
					result.Errors.Add(step1ValidationResult.Message);
					result.Valid = false;

					return result;
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
				if (!PageModel.FeeAccounts.BankAccounts.Any(c => c.AccountType == AccountType.Fee) && OnboardingStepService.HasFeeAccounts)
				{
					result.Errors.Add("Atleast one fee account is require.");
					result.Valid = false;

					return result;
				}

				if (PageModel.FeeAccounts.BankAccounts.Where(c => c.AccountType == AccountType.Fee).Count() > 0)
				{
					var accountNumbers = PageModel.FeeAccounts.BankAccounts.Where(c => c.AccountType == AccountType.Fee).Select(c => c.AccountNumber).Distinct().ToList();
					if (accountNumbers.Count != PageModel.FeeAccounts.BankAccounts.Where(c => c.AccountType == AccountType.Fee).Count())
					{
						result.Errors.Add("All fee account numbers must be unique.");
						result.Valid = false;

						return result;
					}
				}

				if (PageModel.FeeAccounts.BankAccounts.Any(c => c.AccountType == AccountType.Fee && !c.Validated))
				{
					result.Errors.Add("All fee account numbers must be validated.");
					result.Valid = false;

					return result;
				}
			}
			else if (step == 5)
			{
				if (!PageModel.CommissionAccounts.BankAccounts.Any(c=> c.AccountType == AccountType.Commission))
				{
					result.Errors.Add("Atleast one commision account is require.");
					result.Valid = false;

					return result;
				}

				if (PageModel.CommissionAccounts.BankAccounts.Where(c => c.AccountType == AccountType.Commission).Count() > 0)
				{
					var accountNumbers = PageModel.CommissionAccounts.BankAccounts.Where(c => c.AccountType == AccountType.Commission).Select(c => c.AccountNumber).Distinct().ToList();
					if (accountNumbers.Count != PageModel.CommissionAccounts.BankAccounts.Where(c => c.AccountType == AccountType.Commission).Count())
					{
						result.Errors.Add("All commission account numbers must be unique.");
						result.Valid = false;

						return result;
					}
				}

				if (PageModel.FeeAccounts.BankAccounts.Any(c => c.AccountType == AccountType.Fee && !c.Validated))
				{
					result.Errors.Add("All commission account numbers must be validated.");
					result.Valid = false;

					return result;
				}
			}
			else if (step == 6)
			{
				if (PageModel.Signatories.Signatories.Count > 0)
				{
					var contactNames = PageModel.Signatories.Signatories.Select(c => c.Name).Distinct().ToList();
					if (contactNames.Count != PageModel.Signatories.Signatories.Count)
					{
						result.Errors.Add($"All signatory names must be unique.");
						result.Valid = false;

						return result;
					}

					var contactEmails = PageModel.Signatories.Signatories.Select(c => c.Email).Distinct().ToList();
					if (contactEmails.Count != PageModel.Signatories.Signatories.Count)
					{
						result.Errors.Add($"All signatory emails must be unique.");
						result.Valid = false;

						return result;
					}

					var contactPhones = PageModel.Signatories.Signatories.Select(c => $"{c.MobileNumberCallCode} - {c.MobileNumber}").Distinct().ToList();
					if (contactPhones.Count != PageModel.Signatories.Signatories.Count)
					{
						result.Errors.Add($"All signatory phones must be unique.");
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

					foreach(var document in PageModel.Documents.Documents)
					{
						if (document.IssueDate.HasValue)
						{
							if (document.IssueDate.Value.Date > DateTime.Now.Date)
							{
								result.Errors.Add("One or more of the document(s) issue date is set to a future date.");
								result.Valid = false;

								return result;
							}
						}

						if (document.ExpiryDate.HasValue)
						{
							if (document.ExpiryDate.Value.Date < DateTime.Now.Date)
							{
								result.Errors.Add("One or more of the document(s) expiry date is set to a past date.");
								result.Valid = false;

								return result;
							}
						}
					}
				}
					
			}
			else if (step == 8)
			{
				if (PageModel.CustomFields.CustomFields.Count > 0)
				{
					if (PageModel.CustomFields.CustomFields.Any(c=> c.Compulsory && string.IsNullOrEmpty(c.Response)))
					{
						result.Errors.Add("All compulsory field must have value.");
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

			var response = await OnboardingService.OnboardCustomer(PageModel);

			SpinnerService.Hide();

			if (response != null && response.Success && response.Result != null)
			{
				var icon = response.Success ? "success" : "error";
				await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "Customer information saved successfully.", icon, "Ok");

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

		private async Task OnCountryValueChanged(string value)
		{
			SpinnerService.Show();

			PageModel.OfficialInformation.Country = "";
			PageModel.OfficialInformation.CountryId = null;

			var country = CountryLookup.FirstOrDefault(c => c.Text == value);
			if(country != null) 
			{
				PageModel.OfficialInformation.Country = value;
				PageModel.OfficialInformation.CountryId = country.Value;

				PageModel.OfficialInformation.OfficePhoneCallCode = country.AlternateText2;
				PageModel.OfficialInformation.OfficePhoneIsoCode = country.AlternateText;
				PageModel.OfficialInformation.MobilePhoneCallCode = country.AlternateText2;
				PageModel.OfficialInformation.MobilePhoneIsoCode = country.AlternateText;
			}

			await LoadStateLookups(PageModel.OfficialInformation.Country);

			SpinnerService.Hide();
		}

		private async Task OnStateValueChanged(string value)
		{
			PageModel.OfficialInformation.State = "";
			PageModel.OfficialInformation.StateId = null;

			var state = StateLookup.FirstOrDefault(c => c.Text == value);
			if (state != null)
			{
				PageModel.OfficialInformation.State = value;
				PageModel.OfficialInformation.StateId = state.Value;
			}
		}

		private async Task OnInstitutionTypesValueChanged(int? value)
		{
			SpinnerService.Show();

			PageModel.OfficialInformation.InstitutionTypeId = null;
			PageModel.OfficialInformation.InstitutionTypeName = string.Empty;

			PageModel.OfficialInformation.ChildInstitutionTypeId = null;
			PageModel.OfficialInformation.ChildInstitutionTypeName = string.Empty;

			PageModel.HasFeeAccounts = false;
			OnboardingStepService.HasSettlementBank = false;

			var type = ParentInstitutionTypeLookup.FirstOrDefault(c => c.Value == value);

			if (type != null)
			{
				PageModel.OfficialInformation.InstitutionTypeId = type.Value;
				PageModel.OfficialInformation.InstitutionTypeName = type.Text;

				if (type.AdditionalData.ContainsKey("HasSettlementBank"))
				{
					PageModel.HasSettlementBank = bool.Parse(type.AdditionalData["HasSettlementBank"].ToString());
					OnboardingStepService.HasSettlementBank = PageModel.HasSettlementBank;
				}

				if (type.AdditionalData.ContainsKey("HasFeeAccount"))
				{
					PageModel.HasFeeAccounts = bool.Parse(type.AdditionalData["HasFeeAccount"].ToString());
					OnboardingStepService.HasFeeAccounts = PageModel.HasFeeAccounts;
				}
			}

			await LoadRequireDocumentTypes(PageModel.OfficialInformation.InstitutionTypeId);

			//if (!PageModel.OfficialInformation.HasChildInstitutionType)
			//{
			//	await LoadRequireDocumentTypes(PageModel.OfficialInformation.InstitutionTypeId);
			//}

			await LoadChildrenInstitutionTypeLookups(PageModel.OfficialInformation.InstitutionTypeId);

			SpinnerService.Hide();
		}

		private async Task OnSubInstitutionTypesValueChanged(int? value)
		{
			SpinnerService.Show();

			PageModel.OfficialInformation.ChildInstitutionTypeId = null;
			PageModel.OfficialInformation.ChildInstitutionTypeName = string.Empty;

			PageModel.HasFeeAccounts = false;
			OnboardingStepService.HasSettlementBank = false;

			var type = ChildrenInstitutionTypeLookup.FirstOrDefault(c => c.Value == value);

			if (type != null)
			{
				PageModel.OfficialInformation.ChildInstitutionTypeId = type.Value;
				PageModel.OfficialInformation.ChildInstitutionTypeName = type.Text;

				if (type.AdditionalData.ContainsKey("HasSettlementBank"))
				{
					PageModel.HasSettlementBank = bool.Parse(type.AdditionalData["HasSettlementBank"].ToString());
					OnboardingStepService.HasSettlementBank = PageModel.HasSettlementBank;
				}

				if (type.AdditionalData.ContainsKey("HasFeeAccount"))
				{
					PageModel.HasFeeAccounts = bool.Parse(type.AdditionalData["HasFeeAccount"].ToString());
					OnboardingStepService.HasFeeAccounts = PageModel.HasFeeAccounts;
				}
			}

			await LoadRequireDocumentTypes(PageModel.OfficialInformation.ChildInstitutionTypeId);

			//EditContext.NotifyValidationStateChanged();

			SpinnerService.Hide();
		}

		private async Task OnSectorValueChanged(int? value)
		{
			SpinnerService.Show();

			PageModel.OfficialInformation.SectorId = null;
			PageModel.OfficialInformation.SectorName = string.Empty;

			PageModel.OfficialInformation.IndustryId = null ;
			PageModel.OfficialInformation.IndustryName = string.Empty;

			var sector = SectorLookup.FirstOrDefault(c => c.Value == value);

			if (sector != null) 
			{
				PageModel.OfficialInformation.SectorId = sector.Value;
				PageModel.OfficialInformation.SectorName = sector.Text;

				//ValidationMessages.Clear(new FieldIdentifier(PageModel.OfficialInformation, "CategoryId"));
			}

			await LoadIndustryLookups(PageModel.OfficialInformation.SectorId);

			//EditContext.NotifyValidationStateChanged();

			SpinnerService.Hide();
		}

		private async Task OnIndustriesValueChanged(int? value)
		{
			PageModel.OfficialInformation.IndustryId = null;
			PageModel.OfficialInformation.IndustryName = string.Empty;

			var industry = IndustryLookup.FirstOrDefault(c => c.Value == value);

			if (industry != null)
			{
				PageModel.OfficialInformation.IndustryId = industry.Value;
				PageModel.OfficialInformation.IndustryName = industry.Text;

				//ValidationMessages.Clear(new FieldIdentifier(PageModel.OfficialInformation, "CategoryId"));
			}

			//EditContext.NotifyValidationStateChanged();
		}

        private async Task OnParentValueChanged(string value)
        {
            PageModel.OfficialInformation.ParentId = null;
            PageModel.OfficialInformation.ParentCode = "";
			PageModel.OfficialInformation.ParentName = "";

			var parent = ParentLookup.FirstOrDefault(c => c.Value == value);

            if (parent != null)
            {
                PageModel.OfficialInformation.ParentCode = parent.Value;
                PageModel.OfficialInformation.ParentName = parent.Text;

                //ValidationMessages.Clear(new FieldIdentifier(PageModel.OfficialInformation, "CategoryId"));
            }

            //EditContext.NotifyValidationStateChanged();
        }

        private async Task OnStaffSizeValueChanged(StaffSize value)
		{
			PageModel.OfficialInformation.StaffSize = value;
		}

		private async Task OnBankValueChanged(string value, AccountType accountType, Guid formId)
		{
			BankAccountModel account = null;
			if (accountType == AccountType.Fee)
			{
				account = PageModel.FeeAccounts.BankAccounts.FirstOrDefault(c => c.FormId == formId && c.AccountType == accountType);
			}
			else
			{
				account = PageModel.CommissionAccounts.BankAccounts.FirstOrDefault(c => c.FormId == formId && c.AccountType == accountType);
			}

			account.BankName = string.Empty;
			account.BankCode = string.Empty;

			var item = BankLookup.FirstOrDefault(c => c.Text == value);
			if (item != null)
			{
				account.BankName = item.Text;
				account.BankCode = item.AlternateText;
			}
		}

		private async Task OnSettlementBankValueChanged(string value)
		{
			BankAccountModel account = null;
			
			PageModel.OfficialInformation.SettlementBankName = string.Empty;
			PageModel.OfficialInformation.SettlementBankCode = string.Empty;

			var item = BankLookup.FirstOrDefault(c => c.Text == value);
			if (item != null)
			{
				PageModel.OfficialInformation.SettlementBankName = item.Text;
				PageModel.OfficialInformation.SettlementBankCode = item.AlternateText;
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

		protected void ContactPersonFirstNameHandler(string value, Guid formId)
		{
			var contactPerson = PageModel.ContactPersons.ContactPersons.FirstOrDefault(c => c.FormId == formId);
			contactPerson.FirstName = value;

			StateHasChanged();
		}

		protected void ContactPersonMiddleNameHandler(string value, Guid formId)
		{
			var contactPerson = PageModel.ContactPersons.ContactPersons.FirstOrDefault(c => c.FormId == formId);
			contactPerson.MiddleName = value;

			StateHasChanged();
		}

		protected void ContactPersonLastNameHandler(string value, Guid formId)
		{
			var contactPerson = PageModel.ContactPersons.ContactPersons.FirstOrDefault(c => c.FormId == formId);
			contactPerson.LastName = value;

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

		void AddSignatory()
		{
			PageModel.Signatories.Signatories.Add(new SignatoryModel
			{
				FormId = Guid.NewGuid()
			});

			StateHasChanged();
		}

		protected void RemoveSignatory(Guid formId)
		{
			PageModel.Signatories.Signatories.RemoveAll(c => c.FormId == formId);

			StateHasChanged();
		}

		protected void SignatoryNameHandler(string value, Guid formId)
		{
			var signatory = PageModel.Signatories.Signatories.FirstOrDefault(c => c.FormId == formId);
			signatory.Name = value;

			StateHasChanged();
		}

		protected void SignatoryFirstNameHandler(string value, Guid formId)
		{
			var signatory = PageModel.Signatories.Signatories.FirstOrDefault(c => c.FormId == formId);
			signatory.FirstName = value;

			StateHasChanged();
		}

		protected void SignatoryMiddleNameHandler(string value, Guid formId)
		{
			var signatory = PageModel.Signatories.Signatories.FirstOrDefault(c => c.FormId == formId);
			signatory.MiddleName = value;

			StateHasChanged();
		}

		protected void SignatoryLastNameHandler(string value, Guid formId)
		{
			var signatory = PageModel.Signatories.Signatories.FirstOrDefault(c => c.FormId == formId);
			signatory.LastName = value;

			StateHasChanged();
		}

		protected void SignatoryCallCodeHandler(string value, Guid formId)
		{
			var signatory = PageModel.Signatories.Signatories.FirstOrDefault(c => c.FormId == formId);
			signatory.MobileNumberCallCode = value;
			signatory.MobileNumberIsoCode = CountryLookup.FirstOrDefault(c => c.AlternateText2 == value).AlternateText;

			StateHasChanged();
		}

		protected async Task ValidateAccount(Guid formId, AccountType accountType)
		{
			BankAccountModel? account = null;
			if(accountType == AccountType.Fee)
			{
				account = PageModel.FeeAccounts.BankAccounts.FirstOrDefault(c => c.FormId == formId);
			}
			else
			{
				account = PageModel.CommissionAccounts.BankAccounts.FirstOrDefault(c => c.FormId == formId);
			}

		    account.Validated = false;

			var response = await BankService.ValidateAccount(account.BankCode, account.AccountName, account.AccountNumber);
			if (response != null && response.Success && response.Result != null)
			{
				var icon = response.Result.Valid ? "success" : "error";

				account.Validated = response.Result.Valid;

				if (response.Result.Valid)
				{
					//
				}

				await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", response.Result.Message, icon, "Ok");
			}
			else
			{
				await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "Unable to validate account at this time.", "error", "Ok");
			}

			account.Validated = true;
		}

		void AddBankAccount(AccountType accountType)
		{
			if(accountType == AccountType.Fee){
				PageModel.FeeAccounts.BankAccounts.Add(new BankAccountModel
				{
					FormId = Guid.NewGuid(),
					AccountType = accountType
				});
			}
			else
			{
				PageModel.CommissionAccounts.BankAccounts.Add(new BankAccountModel
				{
					FormId = Guid.NewGuid(),
					AccountType = accountType
				});
			}

			StateHasChanged();
		}

		protected void RemoveBankAccount(Guid formId, AccountType accountType)
		{
			if (accountType == AccountType.Fee)
			{
				PageModel.FeeAccounts.BankAccounts.RemoveAll(c => c.FormId == formId);
			}
			else
			{
				PageModel.CommissionAccounts.BankAccounts.RemoveAll(c => c.FormId == formId);
			}
			
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

		private async Task OnDocumentTypeValueChanged(int? value, Guid formId)
		{
			SpinnerService.Show();

			var document = PageModel.Documents.Documents.FirstOrDefault(c => c.FormId == formId);

			document.DocumentTypeId = null;
			document.DocumentTypeName = string.Empty;
			document.Title = string.Empty;

			var documentType = DocumentTypeLookup.FirstOrDefault(c => c.Value == value);
			if (documentType != null)
			{
				document.DocumentTypeId = documentType.Value;
				document.DocumentTypeName = documentType.Text;
				document.Title = documentType.Text;

				if (documentType.HasAdditionalData)
				{
					if (documentType.AdditionalData.ContainsKey("RequiresExpiryDate"))
					{
						document.HasExpiryDate = bool.Parse(documentType.AdditionalData["RequiresExpiryDate"].ToString());
					}

					if (documentType.AdditionalData.ContainsKey("RequiresIssueDate"))
					{
						document.HasIssueDate = bool.Parse(documentType.AdditionalData["RequiresIssueDate"].ToString());
					}
				}
				
				//ValidationMessages.Clear(new FieldIdentifier(PageModel.OfficialInformation, "CategoryId"));
			}

			SpinnerService.Hide();
		}

		private async Task OnIssueDateValueChanged(DateTime? value, Guid formId)
		{
			if (value.HasValue)
			{
				if(value.Value.Date > DateTime.Now.Date)
				{
					await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "Issue date cannot be a future date", "error", "Ok");
				}
				else
				{
					var document = PageModel.Documents.Documents.FirstOrDefault(c => c.FormId == formId);
					document.IssueDate = value;
				}
			}
			
			//return Task.CompletedTask;
		}

		private async Task OnExpiryDateValueChanged(DateTime? value, Guid formId)
		{
			if (value.HasValue)
			{
				if (value.Value.Date < DateTime.Now.Date)
				{
					await JSRuntime.InvokeVoidAsync("coreInterop.showMessage", "Expiry date cannot be a past date", "error", "Ok");
				}
				else
				{
					var document = PageModel.Documents.Documents.FirstOrDefault(c => c.FormId == formId);
					document.ExpiryDate = value;
				}
			}		
		}

		async Task HandleLogoFileSelection(InputFileChangeEventArgs e)
		{
			SpinnerService.Show();

			var maxAllowedFiles = 1;
			var maxSize = 100 * 1024 * 1024;

			try
			{
				var maxFileSizeResult = await DocumentService.GetMaximumFileSize(null);
				if(maxFileSizeResult != null)
				{
					maxSize = int.Parse(maxFileSizeResult.Result.ToString());
				}

				foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
				{
					var buffer = new byte[file.Size];
					await file.OpenReadStream(maxSize).ReadAsync(buffer);

					PageModel.OfficialInformation.LogoFileData = buffer;
					PageModel.OfficialInformation.LogoContentType = file.ContentType;
					PageModel.OfficialInformation.LogoSize = file.Size;

					PageModel.OfficialInformation.LogoSelectedFileName = file.Name;
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

		async Task HandleFileSelection(InputFileChangeEventArgs e, Guid formId)
		{
			SpinnerService.Show();

			var maxAllowedFiles = 1;
			var maxSize = 100 * 1024 * 1024;

			try
			{
				var document = PageModel.Documents.Documents.FirstOrDefault(c => c.FormId == formId);

				var maxFileSizeResult = await DocumentService.GetMaximumFileSize(document.DocumentTypeId);
				if (maxFileSizeResult != null)
				{
					maxSize = int.Parse(maxFileSizeResult.Result.ToString());
				}

				foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
				{
					var buffer = new byte[file.Size];
					await file.OpenReadStream(maxSize).ReadAsync(buffer);

				
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

		private async Task<LookupModel<string,string>> getPhoneByIdAsync(List<LookupModel<string, string>> allItems, string id, CancellationToken token)
		{
			return allItems.FirstOrDefault(c=> c.Value == id);
		}

		public async void ShowReview()
		{
			SpinnerService.Show();

			await LoadPageData();

			SpinnerService.Hide();

			ReviewDialog.Show(PageModel);
		}

	}
}
