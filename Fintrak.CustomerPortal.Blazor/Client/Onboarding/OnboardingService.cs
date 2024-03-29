using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Queries;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models;
using Fintrak.CustomerPortal.Blazor.Client.Onboarding.Pages;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using UpsertCustomFieldDto = Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct.UpsertCustomFieldDto;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;
using System.Xml.Linq;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding
{
    public class OnboardingService : IOnboardingService
	{
		private readonly HttpClient _http;

		public OnboardingService(HttpClient http)
		{
			_http = http;
		}

		public async Task<BaseResponse<List<QueryDto>>> GetQueries()
		{
			var response = new BaseResponse<List<QueryDto>> ();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<List<QueryDto>>>($"api/Queries");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<bool>> ResponseToQuery(ResponseToQueryDto responseToQuery)
		{
			var response = new BaseResponse<bool>();

			try
			{
				var requestJson = JsonSerializer.Serialize(responseToQuery);
				var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

				var result = await _http.PostAsync($"api/Queries/ResponseToQuery", requestContent);
				result.EnsureSuccessStatusCode();

				response = await result.Content.ReadFromJsonAsync<BaseResponse<bool>>();
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}


		public async Task<BaseResponse<UserDto>> GetCurrentUser()
		{
			var response = new BaseResponse<UserDto>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<UserDto>>($"api/User");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<bool>> ChangePassword(ChangePasswordDto changePassword)
		{
			var response = new BaseResponse<bool>();

			try
			{ 
				var requestJson = JsonSerializer.Serialize(changePassword);
				var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

				var result = await _http.PostAsync($"api/User/changepassword", requestContent);
				result.EnsureSuccessStatusCode();

				response = await result.Content.ReadFromJsonAsync<BaseResponse<bool>>();

				//var result = await _http.PostAsJsonAsync<ChangePasswordDto>($"api/Users/changepassword", changePassword);
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<OnboardingStatusDto>> GetOnboardingStatus()
		{
			var response = new BaseResponse<OnboardingStatusDto>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<OnboardingStatusDto>>($"api/Onboardings/GetStatus");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<bool>> ValidateCustomer(OnboardingModel onboarding, int currentStep)
		{
			var response = new BaseResponse<bool>();

			try
			{
				var request = new OnboardCustomerDto();

				//OfficialInformation
				request.Name = onboarding.OfficialInformation.CompanyName;
				request.RegistrationCertificateNumber = onboarding.OfficialInformation.RegistrationCertificateNumber;
				request.IncorporationDate = onboarding.OfficialInformation.IncorporationDate.Value;
				request.RegisterAddress1 = onboarding.OfficialInformation.RegisterAddress1;
				request.RegisterAddress2 = onboarding.OfficialInformation.RegisterAddress2;
				request.BusinessNature = onboarding.OfficialInformation.BusinessNature;
				request.TaxIdentificationNumber = onboarding.OfficialInformation.TaxIdentificationNumber;
				request.CountryId = onboarding.OfficialInformation.CountryId;
				request.Country = onboarding.OfficialInformation.Country;
				request.StateId = onboarding.OfficialInformation.StateId;
				request.State = onboarding.OfficialInformation.State;
				request.City = onboarding.OfficialInformation.City;
				request.OfficePhoneCallCode = onboarding.OfficialInformation.OfficePhoneCallCode;
				request.OfficePhoneNumber = onboarding.OfficialInformation.OfficePhoneNumber;
				request.MobilePhoneCallCode = onboarding.OfficialInformation.MobilePhoneCallCode;
				request.MobilePhoneNumber = onboarding.OfficialInformation.MobilePhoneNumber;
				request.Email = onboarding.OfficialInformation.Email;
				request.Fax = onboarding.OfficialInformation.Fax;
				request.Website = onboarding.OfficialInformation.Website;
				request.SectorId = onboarding.OfficialInformation.SectorId;
				request.SectorName = onboarding.OfficialInformation.SectorName;
				request.IndustryId = onboarding.OfficialInformation.IndustryId;
				request.IndustryName = onboarding.OfficialInformation.IndustryName;
				request.InstitutionTypeId = onboarding.OfficialInformation.InstitutionTypeId;
				request.InstitutionTypeName = onboarding.OfficialInformation.InstitutionTypeName;
				request.HasChildInstitutionType = onboarding.OfficialInformation.HasChildInstitutionType;
				request.ChildInstitutionTypeId = onboarding.OfficialInformation.ChildInstitutionTypeId;
				request.ChildInstitutionTypeName = onboarding.OfficialInformation.ChildInstitutionTypeName;
				request.InstitutionCode = onboarding.OfficialInformation.InstitutionCode;
				request.StaffSize = onboarding.OfficialInformation.StaffSize;
				request.IsPublic = onboarding.OfficialInformation.IsPublic;

				request.LogoLocation = onboarding.OfficialInformation.LogoLocation;
				request.LogoFileData = onboarding.OfficialInformation.LogoFileData;
				request.LogoContentType = onboarding.OfficialInformation.LogoContentType;
				request.LogoSize = onboarding.OfficialInformation.LogoSize;

				request.SettlementBankCode = onboarding.OfficialInformation.SettlementBankCode;
				request.SettlementBankName = onboarding.OfficialInformation.SettlementBankName;

				//ContactPersons
				foreach (var contactPerson in onboarding.ContactPersons.ContactPersons)
				{
					request.ContactPersons.Add(new UpsertContactPersonDto
					{
						Name = contactPerson.Name,
						Email = contactPerson.Email,
						MobilePhoneCallCode = contactPerson.MobilePhoneCallCode,
						MobilePhoneNumber = contactPerson.MobilePhoneNumber,
						Designation = contactPerson.Designation,
						Default = contactPerson.Default
					});
				}

				//ContactChannels
				foreach (var contactChannel in onboarding.ContactChannels.ContactChannels)
				{
					request.ContactChannels.Add(new UpsertContactChannelDto
					{
						Type = contactChannel.Type,
						Email = contactChannel.Email,
						MobilePhoneCallCode = contactChannel.MobilePhoneCallCode,
						MobilePhoneNumber = contactChannel.MobilePhoneNumber
					});
				}

				//BankAccounts

				foreach (var bankAccount in onboarding.FeeAccounts.BankAccounts)
				{
					request.BankAccounts.Add(new UpsertBankAccountDto
					{
						AccountType = bankAccount.AccountType,
						BankName = bankAccount.BankName,
						BankCode = bankAccount.BankCode,
						Country = bankAccount.Country,
						BankAddress = bankAccount.BankAddress,
						AccountName = bankAccount.AccountName,
						AccountNumber = bankAccount.AccountNumber,
						Validated = bankAccount.Validated,
					});
				}

				foreach (var bankAccount in onboarding.CommissionAccounts.BankAccounts)
				{
					request.BankAccounts.Add(new UpsertBankAccountDto
					{
						AccountType = bankAccount.AccountType,
						BankName = bankAccount.BankName,
						BankCode = bankAccount.BankCode,
						Country = bankAccount.Country,
						BankAddress = bankAccount.BankAddress,
						AccountName = bankAccount.AccountName,
						AccountNumber = bankAccount.AccountNumber,
						Validated = bankAccount.Validated,
					});
				}

				//Signatories
				foreach (var signatory in onboarding.Signatories.Signatories)
				{
					request.Signatories.Add(new UpsertSignatoryDto
					{
						Name = signatory.Name,
						Email = signatory.Email,
						MobilePhoneCallCode = signatory.MobileNumberCallCode,
						MobilePhoneNumber = signatory.MobileNumber,
						Designation = signatory.Designation
					});
				}

				//Documents
				foreach (var document in onboarding.Documents.Documents)
				{
					request.Documents.Add(new UpsertDocumentDto
					{
						DocumentTypeId = document.DocumentTypeId.Value,
						DocumentTypeName = document.DocumentTypeName,
						Title = document.Title,
						FileData = document.FileData,
						ContentType = document.ContentType,
						Size = document.Size,
						LocationUrl = document.LocationUrl,
						DocumentId = document.DocumentId,
						HasIssueDate = document.HasIssueDate,
						IssueDate = document.IssueDate,
						HasExpiryDate = document.HasExpiryDate,
						ExpiryDate = document.ExpiryDate,
					});
				}

				//Fields
				foreach (var field in onboarding.CustomFields.CustomFields)
				{
					request.CustomFields.Add(new UpsertCustomFieldDto
					{
						CustomFieldId = field.FieldId,
						CustomField = field.Field,
						Response = field.Response,
						IsCompulsory = field.Compulsory,
					});
				}

				var requestJson = JsonSerializer.Serialize(request);
				var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

				var result = await _http.PostAsync($"api/Onboardings/validate?currentStep={currentStep}", requestContent);
				result.EnsureSuccessStatusCode();

				response = await result.Content.ReadFromJsonAsync<BaseResponse<bool>>();
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<int>> OnboardCustomer(OnboardingModel onboarding)
		{
			var response = new BaseResponse<int>();

			try
			{
				var request = new OnboardCustomerDto();

				//OfficialInformation
				request.Name = onboarding.OfficialInformation.CompanyName;
				request.RegistrationCertificateNumber = onboarding.OfficialInformation.RegistrationCertificateNumber;
				request.IncorporationDate = onboarding.OfficialInformation.IncorporationDate.Value;
				request.RegisterAddress1 = onboarding.OfficialInformation.RegisterAddress1;
                request.RegisterAddress2 = onboarding.OfficialInformation.RegisterAddress2;
                request.TaxIdentificationNumber = onboarding.OfficialInformation.TaxIdentificationNumber;
				request.BusinessNature = onboarding.OfficialInformation.BusinessNature;
				request.CountryId = onboarding.OfficialInformation.CountryId;
				request.Country = onboarding.OfficialInformation.Country;
				request.StateId = onboarding.OfficialInformation.StateId;
				request.State = onboarding.OfficialInformation.State;
                request.City = onboarding.OfficialInformation.City;
                request.OfficePhoneCallCode = onboarding.OfficialInformation.OfficePhoneCallCode;
				request.OfficePhoneNumber = onboarding.OfficialInformation.OfficePhoneNumber;
				request.MobilePhoneCallCode = onboarding.OfficialInformation.MobilePhoneCallCode;
				request.MobilePhoneNumber = onboarding.OfficialInformation.MobilePhoneNumber;
				request.Email = onboarding.OfficialInformation.Email;
				request.Fax = onboarding.OfficialInformation.Fax;
				request.Website = onboarding.OfficialInformation.Website;
				request.SectorId = onboarding.OfficialInformation.SectorId;
				request.SectorName = onboarding.OfficialInformation.SectorName;
				request.IndustryId = onboarding.OfficialInformation.IndustryId;
				request.IndustryName = onboarding.OfficialInformation.IndustryName;
                request.InstitutionTypeId = onboarding.OfficialInformation.InstitutionTypeId;
                request.InstitutionTypeName = onboarding.OfficialInformation.InstitutionTypeName;
				request.HasChildInstitutionType = onboarding.OfficialInformation.HasChildInstitutionType;
				request.ChildInstitutionTypeId = onboarding.OfficialInformation.ChildInstitutionTypeId;
                request.ChildInstitutionTypeName = onboarding.OfficialInformation.ChildInstitutionTypeName;
                request.InstitutionCode = onboarding.OfficialInformation.InstitutionCode;
                request.StaffSize = onboarding.OfficialInformation.StaffSize;
                request.IsPublic = onboarding.OfficialInformation.IsPublic;

				request.LogoLocation = onboarding.OfficialInformation.LogoLocation;
				request.LogoFileData = onboarding.OfficialInformation.LogoFileData;
				request.LogoContentType = onboarding.OfficialInformation.LogoContentType;
				request.LogoSize = onboarding.OfficialInformation.LogoSize;

				request.SettlementBankCode = onboarding.OfficialInformation.SettlementBankCode;
				request.SettlementBankName = onboarding.OfficialInformation.SettlementBankName;

				request.IsCorrespondentBank = onboarding.OfficialInformation.IsCorrespondentBank;
				request.HasSubsidiary = onboarding.OfficialInformation.HasSubsidiary;
				request.IsSubsidiary = onboarding.OfficialInformation.IsSubsidiary;
				request.ParentId = onboarding.OfficialInformation.ParentId;
				request.ParentCode = onboarding.OfficialInformation.ParentCode;
				request.ParentName = onboarding.OfficialInformation.ParentName;


				//ContactPersons
				foreach (var contactPerson in onboarding.ContactPersons.ContactPersons)
				{
					request.ContactPersons.Add(new UpsertContactPersonDto
					{
						FirstName = contactPerson.FirstName,
						MiddleName = contactPerson.MiddleName,
						LastName = contactPerson.LastName,
						Name = UpsertContactPersonDto.GetFullName(contactPerson.FirstName, contactPerson.MiddleName, contactPerson.LastName),
						Email = contactPerson.Email,
						MobilePhoneCallCode = contactPerson.MobilePhoneCallCode,
						MobilePhoneNumber = contactPerson.MobilePhoneNumber,
						Designation = contactPerson.Designation,
						Default = contactPerson.Default 
					});
				}

				//ContactChannels
				foreach (var contactChannel in onboarding.ContactChannels.ContactChannels)
				{
					request.ContactChannels.Add(new UpsertContactChannelDto
					{
						Type = contactChannel.Type,
						Email = contactChannel.Email,
						MobilePhoneCallCode = contactChannel.MobilePhoneCallCode,
						MobilePhoneNumber = contactChannel.MobilePhoneNumber
					});
				}

				//BankAccounts

				foreach (var bankAccount in onboarding.FeeAccounts.BankAccounts)
				{
					request.BankAccounts.Add(new UpsertBankAccountDto
					{
						AccountType = bankAccount.AccountType,
						BankName = bankAccount.BankName,
						BankCode = bankAccount.BankCode,
						Country = bankAccount.Country,
						BankAddress = bankAccount.BankAddress,
						AccountName = bankAccount.AccountName,
						AccountNumber = bankAccount.AccountNumber,
						Validated = bankAccount.Validated,
					});
				}

				foreach (var bankAccount in onboarding.CommissionAccounts.BankAccounts)
				{
					request.BankAccounts.Add(new UpsertBankAccountDto
					{
						AccountType = bankAccount.AccountType,
						BankName = bankAccount.BankName,
						BankCode = bankAccount.BankCode,
						Country = bankAccount.Country,
						BankAddress = bankAccount.BankAddress,
						AccountName = bankAccount.AccountName,
						AccountNumber = bankAccount.AccountNumber,
						Validated = bankAccount.Validated,
					});
				}

				//Signatories
				foreach (var signatory in onboarding.Signatories.Signatories)
				{
					request.Signatories.Add(new UpsertSignatoryDto
					{
						FirstName = signatory.FirstName,
						MiddleName = signatory.MiddleName,
						LastName = signatory.LastName,
						Name = UpsertSignatoryDto.GetFullName(signatory.FirstName, signatory.MiddleName, signatory.LastName),
						Email = signatory.Email,
						MobilePhoneCallCode = signatory.MobileNumberCallCode,
						MobilePhoneNumber = signatory.MobileNumber,
						Designation = signatory.Designation 
                    });
				}

				//Documents
				foreach (var document in onboarding.Documents.Documents)
				{
					request.Documents.Add(new UpsertDocumentDto
					{
						DocumentTypeId = document.DocumentTypeId.Value,
						DocumentTypeName = document.DocumentTypeName,
						Title = document.Title,
						FileData = document.FileData,
						ContentType = document.ContentType,
						Size = document.Size,
						LocationUrl = document.LocationUrl,
						DocumentId = document.DocumentId,
						HasExpiryDate = document.HasExpiryDate,
						ExpiryDate = document.ExpiryDate,
						HasIssueDate = document.HasIssueDate,
						IssueDate = document.IssueDate,
						
					});
				}

				//Fields
				foreach (var field in onboarding.CustomFields.CustomFields)
				{
					request.CustomFields.Add(new UpsertCustomFieldDto
					{
						CustomFieldId = field.FieldId,
						CustomField = field.Field,
						Response = field.Response,
						IsCompulsory = field.Compulsory,
					});
				}

				var requestJson = JsonSerializer.Serialize(request);
				var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

				var result = await _http.PostAsync($"api/Onboardings", requestContent);
				result.EnsureSuccessStatusCode();

				response = await result.Content.ReadFromJsonAsync<BaseResponse<int>>();
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<bool>> UpdateCustomer(UpdateCustomerDto customer)
		{
			var response = new BaseResponse<bool>();

			try
			{
				var requestJson = JsonSerializer.Serialize(customer);
				var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

				var result = await _http.PostAsync($"api/Onboardings/UpdateCustomer", requestContent);
				result.EnsureSuccessStatusCode();

				response = await result.Content.ReadFromJsonAsync<BaseResponse<bool>>();

				//var result = await _http.PostAsJsonAsync<ChangePasswordDto>($"api/Users/changepassword", changePassword);
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

        public async Task<BaseResponse<int>> SavedCustomer(OnboardingModel onboarding, int currentStep)
        {
            var response = new BaseResponse<int>();

            try
            {
                var request = new OnboardCustomerDto();

                //OfficialInformation
                request.Name = onboarding.OfficialInformation.CompanyName;
                request.RegistrationCertificateNumber = onboarding.OfficialInformation.RegistrationCertificateNumber;
                request.IncorporationDate = onboarding.OfficialInformation.IncorporationDate.Value;
                request.RegisterAddress1 = onboarding.OfficialInformation.RegisterAddress1;
                request.RegisterAddress2 = onboarding.OfficialInformation.RegisterAddress2;
				request.BusinessNature = onboarding.OfficialInformation.BusinessNature;
				request.TaxIdentificationNumber = onboarding.OfficialInformation.TaxIdentificationNumber;
                request.CountryId = onboarding.OfficialInformation.CountryId;
                request.Country = onboarding.OfficialInformation.Country;
                request.StateId = onboarding.OfficialInformation.StateId;
                request.State = onboarding.OfficialInformation.State;
                request.City = onboarding.OfficialInformation.City;
                request.OfficePhoneCallCode = onboarding.OfficialInformation.OfficePhoneCallCode;
                request.OfficePhoneNumber = onboarding.OfficialInformation.OfficePhoneNumber;
                request.MobilePhoneCallCode = onboarding.OfficialInformation.MobilePhoneCallCode;
                request.MobilePhoneNumber = onboarding.OfficialInformation.MobilePhoneNumber;
                request.Email = onboarding.OfficialInformation.Email;
                request.Fax = onboarding.OfficialInformation.Fax;
                request.Website = onboarding.OfficialInformation.Website;
                request.SectorId = onboarding.OfficialInformation.SectorId;
                request.SectorName = onboarding.OfficialInformation.SectorName;
                request.IndustryId = onboarding.OfficialInformation.IndustryId;
                request.IndustryName = onboarding.OfficialInformation.IndustryName;
                request.InstitutionTypeId = onboarding.OfficialInformation.InstitutionTypeId;
                request.InstitutionTypeName = onboarding.OfficialInformation.InstitutionTypeName;
				request.HasChildInstitutionType = onboarding.OfficialInformation.HasChildInstitutionType;
				request.ChildInstitutionTypeId = onboarding.OfficialInformation.ChildInstitutionTypeId;
                request.ChildInstitutionTypeName = onboarding.OfficialInformation.ChildInstitutionTypeName;
                request.InstitutionCode = onboarding.OfficialInformation.InstitutionCode;
                request.StaffSize = onboarding.OfficialInformation.StaffSize;
                request.IsPublic = onboarding.OfficialInformation.IsPublic;

				request.LogoLocation = onboarding.OfficialInformation.LogoLocation;
				request.LogoFileData = onboarding.OfficialInformation.LogoFileData;
				request.LogoContentType = onboarding.OfficialInformation.LogoContentType;
				request.LogoSize = onboarding.OfficialInformation.LogoSize;

				request.SettlementBankCode = onboarding.OfficialInformation.SettlementBankCode;
				request.SettlementBankName = onboarding.OfficialInformation.SettlementBankName;

				request.IsCorrespondentBank = onboarding.OfficialInformation.IsCorrespondentBank;
				request.HasSubsidiary = onboarding.OfficialInformation.HasSubsidiary;
				request.IsSubsidiary = onboarding.OfficialInformation.IsSubsidiary;
				request.ParentId = onboarding.OfficialInformation.ParentId;
				request.ParentCode = onboarding.OfficialInformation.ParentCode;
				request.ParentName = onboarding.OfficialInformation.ParentName;

				//ContactPersons
				foreach (var contactPerson in onboarding.ContactPersons.ContactPersons)
                {
                    request.ContactPersons.Add(new UpsertContactPersonDto
                    {
						FirstName = contactPerson.FirstName,
						MiddleName = contactPerson.MiddleName,
						LastName = contactPerson.LastName,
                        Name = UpsertContactPersonDto.GetFullName(contactPerson.FirstName, contactPerson.MiddleName, contactPerson.LastName),
                        Email = contactPerson.Email,
                        MobilePhoneCallCode = contactPerson.MobilePhoneCallCode,
                        MobilePhoneNumber = contactPerson.MobilePhoneNumber,
                        Designation = contactPerson.Designation,
                        Default = contactPerson.Default
                    });
                }

                //ContactChannels
                foreach (var contactChannel in onboarding.ContactChannels.ContactChannels)
                {
                    request.ContactChannels.Add(new UpsertContactChannelDto
                    {
                        Type = contactChannel.Type,
                        Email = contactChannel.Email,
                        MobilePhoneCallCode = contactChannel.MobilePhoneCallCode,
                        MobilePhoneNumber = contactChannel.MobilePhoneNumber
                    });
                }

                //BankAccounts

                foreach (var bankAccount in onboarding.FeeAccounts.BankAccounts)
                {
                    request.BankAccounts.Add(new UpsertBankAccountDto
                    {
                        AccountType = bankAccount.AccountType,
                        BankName = bankAccount.BankName,
                        BankCode = bankAccount.BankCode,
                        Country = bankAccount.Country,
                        BankAddress = bankAccount.BankAddress,
                        AccountName = bankAccount.AccountName,
                        AccountNumber = bankAccount.AccountNumber,
						Validated = bankAccount.Validated,
                    });
                }

                foreach (var bankAccount in onboarding.CommissionAccounts.BankAccounts)
                {
                    request.BankAccounts.Add(new UpsertBankAccountDto
                    {
                        AccountType = bankAccount.AccountType,
                        BankName = bankAccount.BankName,
                        BankCode = bankAccount.BankCode,
                        Country = bankAccount.Country,
                        BankAddress = bankAccount.BankAddress,
                        AccountName = bankAccount.AccountName,
                        AccountNumber = bankAccount.AccountNumber,
						Validated = bankAccount.Validated,
                    });
                }

                //Signatories
                foreach (var signatory in onboarding.Signatories.Signatories)
                {
                    request.Signatories.Add(new UpsertSignatoryDto
                    {
						FirstName = signatory.FirstName,
						MiddleName = signatory.MiddleName,
						LastName = signatory.LastName,
						Name = UpsertSignatoryDto.GetFullName(signatory.FirstName, signatory.MiddleName, signatory.LastName),
                        Email = signatory.Email,
                        MobilePhoneCallCode = signatory.MobileNumberCallCode,
                        MobilePhoneNumber = signatory.MobileNumber,
                        Designation = signatory.Designation
                    });
                }

                //Documents
                foreach (var document in onboarding.Documents.Documents)
                {
                    request.Documents.Add(new UpsertDocumentDto
                    {
                        DocumentTypeId = document.DocumentTypeId.Value,
                        DocumentTypeName = document.DocumentTypeName,
                        Title = document.Title,
                        FileData = document.FileData,
                        ContentType = document.ContentType,
                        Size = document.Size,
                        LocationUrl = document.LocationUrl,
                        DocumentId = document.DocumentId,
						HasIssueDate = document.HasIssueDate,
						IssueDate = document.IssueDate,
						HasExpiryDate = document.HasExpiryDate,
						ExpiryDate = document.ExpiryDate,
                    });
                }

				//Fields
				foreach (var field in onboarding.CustomFields.CustomFields)
				{
					request.CustomFields.Add(new UpsertCustomFieldDto
					{
						CustomFieldId = field.FieldId,
						CustomField = field.Field,
						Response = field.Response,
						IsCompulsory = field.Compulsory,
					});
				}

				var requestJson = JsonSerializer.Serialize(request);
                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var result = await _http.PostAsync($"api/Onboardings/save?currentStep={currentStep}", requestContent);
                result.EnsureSuccessStatusCode();

                response = await result.Content.ReadFromJsonAsync<BaseResponse<int>>();
            }
            catch (Exception exception)
            {
                response.Success = false;
                response.Message = exception.Message;
                //exception.Redirect();
            }

            return response;
        }

        public async Task<BaseResponse<OnboardingModel>> GetCustomer()
		{
			var result = new BaseResponse<OnboardingModel>();

			try
			{
				var response = await _http.GetFromJsonAsync<BaseResponse<OnboardCustomerDto>>($"api/Onboardings/GetCustomerDetails");

				if (response != null && response.Success)
				{
					if (response.Result != null)
					{
						var model = new OnboardingModel();

                        model.OfficialInformation.Code = response.Result.Code;
                        model.OfficialInformation.CustomCode = response.Result.CustomCode;
                        model.OfficialInformation.CompanyName = response.Result.Name;
						model.OfficialInformation.RegistrationCertificateNumber = response.Result.RegistrationCertificateNumber;
						model.OfficialInformation.IncorporationDate = response.Result.IncorporationDate;
						model.OfficialInformation.RegisterAddress1 = response.Result.RegisterAddress1;
                        model.OfficialInformation.RegisterAddress2 = response.Result.RegisterAddress2;
                        model.OfficialInformation.TaxIdentificationNumber = response.Result.TaxIdentificationNumber;
						model.OfficialInformation.BusinessNature = response.Result.BusinessNature;
						model.OfficialInformation.CountryId = response.Result.CountryId;
						model.OfficialInformation.Country = response.Result.Country;
						model.OfficialInformation.StateId = response.Result.StateId;
						model.OfficialInformation.State = response.Result.State;
                        model.OfficialInformation.City = response.Result.City;
                        model.OfficialInformation.OfficePhoneCallCode = response.Result.OfficePhoneCallCode;
						model.OfficialInformation.OfficePhoneNumber = response.Result.OfficePhoneNumber;
						model.OfficialInformation.MobilePhoneCallCode = response.Result.MobilePhoneCallCode;
						model.OfficialInformation.MobilePhoneNumber = response.Result.MobilePhoneNumber;
						model.OfficialInformation.Email = response.Result.Email;
						model.OfficialInformation.Fax = response.Result.Fax;
						model.OfficialInformation.Website = response.Result.Website;
						model.OfficialInformation.SectorId = response.Result.SectorId;
						model.OfficialInformation.SectorName = response.Result.SectorName;
                        model.OfficialInformation.IndustryId = response.Result.IndustryId;
                        model.OfficialInformation.IndustryName = response.Result.IndustryName;
                        model.OfficialInformation.InstitutionTypeId = response.Result.InstitutionTypeId;
                        model.OfficialInformation.InstitutionTypeName = response.Result.InstitutionTypeName;
						model.OfficialInformation.HasChildInstitutionType = response.Result.HasChildInstitutionType;
						model.OfficialInformation.ChildInstitutionTypeId = response.Result.ChildInstitutionTypeId;
                        model.OfficialInformation.ChildInstitutionTypeName = response.Result.ChildInstitutionTypeName;
                        model.OfficialInformation.InstitutionCode = response.Result.InstitutionCode;
                        model.OfficialInformation.StaffSize = response.Result.StaffSize;
                        model.OfficialInformation.DueDiligenceCompleted = response.Result.DueDiligenceCompleted;
						model.OfficialInformation.IsPublic = response.Result.IsPublic;
                        model.OfficialInformation.Status = response.Result.Status;
						model.OfficialInformation.LogoLocation = response.Result.LogoLocation;

						model.OfficialInformation.SettlementBankCode = response.Result.SettlementBankCode;
						model.OfficialInformation.SettlementBankName = response.Result.SettlementBankName;

						model.OfficialInformation.IsCorrespondentBank = response.Result.IsCorrespondentBank;
						model.OfficialInformation.HasSubsidiary = response.Result.HasSubsidiary;
						model.OfficialInformation.IsSubsidiary = response.Result.IsSubsidiary;
						model.OfficialInformation.ParentId = response.Result.ParentId;
						model.OfficialInformation.ParentName = response.Result.ParentName;
						model.OfficialInformation.ParentCode = response.Result.ParentCode;
						model.OfficialInformation.ParentInstitutionCode = response.Result.ParentInstitutionCode;

						model.CanUpdate = response.Result.CanUpdate;

						//ContactPersons
						foreach (var item in response.Result.ContactPersons)
						{
							model.ContactPersons.ContactPersons.Add(new ContactPersonModel
							{
								FormId = Guid.NewGuid(),
								FirstName = item.FirstName,
								MiddleName = item.MiddleName,
								LastName = item.LastName,
								Name = item.Name,
								Email = item.Email,
								MobilePhoneCallCode = item.MobilePhoneCallCode,
								MobilePhoneNumber = item.MobilePhoneNumber,
								Designation = item.Designation,
								Default = item.Default 
							});
						}

						//ContactChannels
						foreach (var item in response.Result.ContactChannels)
						{
							model.ContactChannels.ContactChannels.Add(new ContactChannelModel
							{
								FormId = Guid.NewGuid(),
								Type = item.Type,
								Email = item.Email,
								MobilePhoneCallCode = item.MobilePhoneCallCode,
								MobilePhoneNumber = item.MobilePhoneNumber
							});
						}

						//LocalBank & ForeignBanks
						foreach (var item in response.Result.BankAccounts)
						{
							if(item.AccountType == AccountType.Fee)
							{
								model.FeeAccounts.BankAccounts.Add(new BankAccountModel
								{
									FormId = Guid.NewGuid(),
									AccountType = item.AccountType,
									BankName = item.BankName,
									BankCode = item.BankCode,
									Country = item.Country,
									BankAddress = item.BankAddress,
									AccountName = item.AccountName,
									AccountNumber = item.AccountNumber
								});
							}
							else
							{
								model.CommissionAccounts.BankAccounts.Add(new BankAccountModel
								{
									FormId = Guid.NewGuid(),
									AccountType = item.AccountType,
									BankName = item.BankName,
									BankCode = item.BankCode,
									Country = item.Country,
									BankAddress = item.BankAddress,
									AccountName = item.AccountName,
									AccountNumber = item.AccountNumber
								});
							}
                        }

						//Questionnaires
						foreach (var item in response.Result.Signatories)
						{
							model.Signatories.Signatories.Add(new SignatoryModel
							{
								FirstName = item.FirstName,
								MiddleName = item.MiddleName,
								LastName = item.LastName,
								Name = item.Name,
								Email = item.Email,
								MobileNumber = item.MobilePhoneNumber,
								MobileNumberCallCode = item.MobilePhoneCallCode,
								Designation = item.Designation,
							});
						}

						//Documents
						foreach (var item in response.Result.Documents)
						{
							model.Documents.Documents.Add(new DocumentModel
							{
								FormId = Guid.NewGuid(),
								DocumentTypeId = item.DocumentTypeId,
								DocumentTypeName = item.DocumentTypeName,
								Title = item.Title,
								HasIssueDate = item.HasIssueDate,
								IssueDate = item.IssueDate,
								HasExpiryDate = item.HasExpiryDate,
								ExpiryDate = item.ExpiryDate,
								FileData = item.FileData,
								ContentType = item.ContentType,
								Size = item.Size,
								LocationUrl = item.LocationUrl ,
								DocumentId = item.DocumentId,
							});
						}

						//Custom Fields
						foreach (var item in response.Result.CustomFields)
						{
							model.CustomFields.CustomFields.Add(new Models.CustomFieldModel
                            {
								FieldId = item.CustomFieldId,
								Field = item.CustomField,
								Response = item.Response,
								Compulsory = item.IsCompulsory
							});
						}

						result.Result = model;
					}
				}
			}
			catch (Exception exception)
			{
				result.Success = false;
				result.Message = exception.Message;
				//exception.Redirect();
			}

			return result;
		}

		public async Task<BaseResponse<CustomerModel>> GetCurrentCustomer()
		{
			var result = new BaseResponse<CustomerModel>();

			try
			{
				var response = await _http.GetFromJsonAsync<BaseResponse<BasicCustomerDto>>($"api/Onboardings/GetCurrentCustomer");

				if (response != null && response.Success)
				{
					if (response.Result != null)
					{
						var model = new CustomerModel();

						model.Code = response.Result.Code;
						model.CustomCode = response.Result.CustomCode;
						model.CompanyName = response.Result.Name;
						model.RegistrationCertificateNumber = response.Result.RegistrationCertificateNumber;
						model.IncorporationDate = response.Result.IncorporationDate;
						model.RegisterAddress1 = response.Result.RegisterAddress1;
						model.RegisterAddress2 = response.Result.RegisterAddress2;
						model.TaxIdentificationNumber = response.Result.TaxIdentificationNumber;
						model.BusinessNature = response.Result.BusinessNature;
						model.CountryId = response.Result.CountryId;
						model.Country = response.Result.Country;
						model.StateId = response.Result.StateId;
						model.State = response.Result.State;
						model.City = response.Result.City;
						model.OfficePhoneCallCode = response.Result.OfficePhoneCallCode;
						model.OfficePhoneNumber = response.Result.OfficePhoneNumber;
						model.MobilePhoneCallCode = response.Result.MobilePhoneCallCode;
						model.MobilePhoneNumber = response.Result.MobilePhoneNumber;
						model.Email = response.Result.Email;
						model.Fax = response.Result.Fax;
						model.Website = response.Result.Website;
						model.SectorId = response.Result.SectorId;
						model.SectorName = response.Result.SectorName;
						model.IndustryId = response.Result.IndustryId;
						model.IndustryName = response.Result.IndustryName;
						model.InstitutionTypeId = response.Result.InstitutionTypeId;
						model.InstitutionTypeName = response.Result.InstitutionTypeName;
						model.HasChildInstitutionType = response.Result.HasChildInstitutionType;
						model.ChildInstitutionTypeId = response.Result.ChildInstitutionTypeId;
						model.ChildInstitutionTypeName = response.Result.ChildInstitutionTypeName;
						model.InstitutionCode = response.Result.InstitutionCode;
						model.StaffSize = response.Result.StaffSize;
						model.DueDiligenceCompleted = response.Result.DueDiligenceCompleted;
						model.IsPublic = response.Result.IsPublic;
						model.Status = response.Result.Status;
						model.LogoLocation = response.Result.LogoLocation;

						model.SettlementBankCode = response.Result.SettlementBankCode;
						model.SettlementBankName = response.Result.SettlementBankName;

						model.IsCorrespondentBank = response.Result.IsCorrespondentBank;
						model.HasSubsidiary = response.Result.HasSubsidiary;
						model.IsSubsidiary = response.Result.IsSubsidiary;
						model.ParentId = response.Result.Parent != null ? response.Result.Parent.Id : null;

						result.Result = model;
					}
				}
			}
			catch (Exception exception)
			{
				result.Success = false;
				result.Message = exception.Message;
				//exception.Redirect();
			}

			return result;
		}

		public async Task<BaseResponse<List<string>>> GetTinValidationPatterns()
		{
			var response = new BaseResponse<List<string>>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<List<string>>>($"api/Onboardings/GetTinValidationPatterns");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}


		public async Task<BaseResponse<int>> OnboardProduct(OnboardingProductModel onboarding)
		{
			var response = new BaseResponse<int>();

			try
			{
				var model = new OnboardProductDto();

				model.Id = onboarding.Id;
				model.CustomerName = onboarding.CustomerName;
				model.CustomerCode = onboarding.CustomerCode;
				model.ProductCode = onboarding.ProductCode;
				model.ProductName = onboarding.ProductName;
				model.ProductId = onboarding.ProductId;
				model.ContactPersonId = onboarding.ContactPersonId;
				model.OperationMode = onboarding.OperationMode;
				model.AccountId = onboarding.AccountId;
				model.Reason = onboarding.Reason;
				model.Website = onboarding.Website;

				foreach (var field in onboarding.AdditionalInformations)
				{
					model.CustomerProductCustomFields.Add(new Blazor.Shared.Models.OnboardingProduct.UpsertCustomFieldDto
					{
						CustomFieldId = field.CustomFieldId,
						CustomField = field.CustomField,
						IsCompulsory = field.IsCompulsory,
						Response = field.Response,
					});
				}

				foreach (var document in onboarding.Documents)
				{
					model.CustomerProductDocuments.Add(new UpsertDocumentDto
					{
						DocumentId = document.DocumentId,
						DocumentTypeId = document.DocumentTypeId.Value,
						DocumentTypeName = document.DocumentTypeName,
						Title = document.Title,
						FileData = document.FileData,
						ContentType = document.ContentType,
						Size = document.Size,
						IssueDate = document.IssueDate,
						HasExpiryDate = document.HasExpiryDate,
						ExpiryDate = document.ExpiryDate,
						LocationUrl = document.LocationUrl,
					});
				}


				var requestJson = JsonSerializer.Serialize(model);
				var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

				var result = await _http.PostAsync($"api/OnboardingProducts/CreateProduct", requestContent);
				result.EnsureSuccessStatusCode();

				response = await result.Content.ReadFromJsonAsync<BaseResponse<int>>();
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<bool>> UpdateProduct(UpdateProductDto product)
		{
			var response = new BaseResponse<bool>();

			try
			{
				var requestJson = JsonSerializer.Serialize(product);
				var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

				var result = await _http.PostAsync($"api/OnboardingProducts/UpdateProduct", requestContent);
				result.EnsureSuccessStatusCode();

				response = await result.Content.ReadFromJsonAsync<BaseResponse<bool>>();

				//var result = await _http.PostAsJsonAsync<ChangePasswordDto>($"api/Users/changepassword", changePassword);
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<OnboardingProductStatus>> GetOnboardingProductStatus()
		{
			var response = new BaseResponse<OnboardingProductStatus>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<OnboardingProductStatus>>($"api/OnboardingProducts/GetProductStatus");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<List<OnboardingProductModel>>> GetProducts()
		{
			var result = new BaseResponse<List<OnboardingProductModel>>();

			try
			{
				var response = await _http.GetFromJsonAsync<BaseResponse<List<OnboardingProductModel>>>($"api/OnboardingProducts/GetMyProducts");

				if (response != null && response.Success)
				{
					if (response.Result != null)
					{
						var models = new List<OnboardingProductModel>();

						foreach(var item in response.Result)
						{
							var model = new OnboardingProductModel();

							model.Id = item.Id;
							model.CustomerCode = item.CustomerCode;
							model.CustomerName = item.CustomerName;
							model.ProductId = item.ProductId;
							model.ProductCode = item.ProductCode;
							model.ProductName = item.ProductName;
							model.ContactPersonId = item.ContactPersonId;
							model.OperationMode = item.OperationMode;
							model.AccountId = item.AccountId;
							model.Reason = item.Reason;
							model.Website = item.Website;

							foreach (var field in item.AdditionalInformations)
							{
								model.AdditionalInformations.Add(new Models.UpsertCustomFieldDto
								{
									CustomFieldId = field.CustomFieldId,
									CustomField = field.CustomField,
									IsCompulsory = field.IsCompulsory,
									Response = field.Response
								});
							}

							foreach (var document in item.Documents)
							{
								model.Documents.Add(new DocumentModel
								{
									DocumentId = document.DocumentId,
									DocumentTypeId = document.DocumentTypeId,
									DocumentTypeName = document.DocumentTypeName,
									Title = document.Title,
									LocationUrl = document.LocationUrl,
									IssueDate = document.IssueDate,
									HasExpiryDate = document.HasExpiryDate,
									ExpiryDate = document.ExpiryDate,
								});
							}

							models.Add(model);
						}

						result.Result = models;
					}
				}
			}
			catch (Exception exception)
			{
				result.Success = false;
				result.Message = exception.Message;
				//exception.Redirect();
			}

			return result;
		}

		public async Task<BaseResponse<OnboardingProductModel>> GetProductDetail(int customerProductId)
		{
			var result = new BaseResponse<OnboardingProductModel>();

			try
			{
				var response = await _http.GetFromJsonAsync<BaseResponse<OnboardProductDto>>($"api/OnboardingProducts/GetCustomerProductDetails/{customerProductId}");

				if (response != null && response.Success)
				{
					if (response.Result != null)
					{
						var model = new OnboardingProductModel();

						model.Id = response.Result.Id;
						model.CustomerCode = response.Result.CustomerCode;
						model.CustomerName = response.Result.CustomerName;
						model.ProductId = response.Result.ProductId;
						model.ProductCode = response.Result.ProductCode;
						model.ProductName = response.Result.ProductName;
						model.ContactPersonId = response.Result.ContactPersonId;
						model.OperationMode = response.Result.OperationMode;
						model.AccountId = response.Result.AccountId;
						model.Reason = response.Result.Reason;
						model.Website = response.Result.Website;
						
						foreach(var field in response.Result.CustomerProductCustomFields)
						{
							model.AdditionalInformations.Add(new Models.UpsertCustomFieldDto
							{
								CustomFieldId = field.CustomFieldId,
								CustomField = field.CustomField,
								IsCompulsory = field.IsCompulsory,
								Response = field.Response 
							});
						}

						foreach (var document in response.Result.CustomerProductDocuments)
						{
							model.Documents.Add(new DocumentModel
							{
								DocumentId = document.DocumentId,
								DocumentTypeId = document.DocumentTypeId,	
								DocumentTypeName = document.DocumentTypeName,	
								Title = document.Title,	
								LocationUrl = document.LocationUrl,	
								IssueDate = document.IssueDate,
								HasExpiryDate = document.HasExpiryDate,
								ExpiryDate = document.ExpiryDate,
							});
						}

						result.Result = model;
					}
				}
			}
			catch (Exception exception)
			{
				result.Success = false;
				result.Message = exception.Message;
				//exception.Redirect();
			}

			return result;
		}

		public async Task<BaseResponse<List<LookupModel<string, int>>>> GetContactPersonLookup()
		{
			var response = new BaseResponse<List<LookupModel<string, int>>>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<List<LookupModel<string, int>>>>($"api/Onboardings/CustomerContactLookup");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<List<LookupModel>>> GetParentLookup()
		{
			var response = new BaseResponse<List<LookupModel>>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<List<LookupModel>>>($"api/Onboardings/ParentLookup");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<List<LookupModel<string, int>>>> GetAccountLookup(AccountType? accountType)
		{
			var response = new BaseResponse<List<LookupModel<string, int>>>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<List<LookupModel<string, int>>>>($"api/Onboardings/CustomerAccountLookup?accountType={accountType}");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<DashboardDto>> GetDashboard()
		{
			var result = new BaseResponse<DashboardDto>();

			try
			{
				var response = await _http.GetFromJsonAsync<BaseResponse<DashboardDto>>($"api/Onboardings/MyDashboard");

				//if (response != null && response.Success)
				//{
				//	if (response.Result != null)
				//	{
				//		var model = new DashboardModel();

				//		foreach (var item in response.Result.)
				//		{

				//		}

				//		result.Result = model;
				//	}
				//}

				return response;
			}
			catch (Exception exception)
			{
				result.Success = false;
				result.Message = exception.Message;
				//exception.Redirect();
			}

			return result;
		}

        public async Task<BaseResponse<bool>> AcceptTerms(AcceptTermsDto acceptTerms)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var requestJson = JsonSerializer.Serialize(acceptTerms);
                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var result = await _http.PostAsync($"api/User/AcceptTerms", requestContent);
                result.EnsureSuccessStatusCode();

                response = await result.Content.ReadFromJsonAsync<BaseResponse<bool>>();
            }
            catch (Exception exception)
            {
                response.Success = false;
                response.Message = exception.Message;
                //exception.Redirect();
            }

            return response;
        }

        public async Task<BaseResponse<CustomFieldsModel>> GetCustomFields()
        {
            var result = new BaseResponse<CustomFieldsModel> { Result = new CustomFieldsModel() };

            try
            {
                var response = await _http.GetFromJsonAsync<BaseResponse<List<CustomFieldResponseDto>>>($"api/Onboardings/GetCustomerCustomFields");

                if (response != null && response.Success)
                {
                    if (response.Result != null)
                    {
                        foreach (var item in response.Result)
                        {
                            var model = new Onboarding.Models.CustomFieldModel();

							model.FieldId = item.Id;
                            model.Field = item.Name;
                            model.Response = item.Response;
                            model.Compulsory = item.IsCompulsory;

                            result.Result.CustomFields.Add(model);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                result.Success = false;
                result.Message = exception.Message;
                //exception.Redirect();
            }

            return result;
        }

		public async Task<BaseResponse<List<CustomerOnboardingDocumentDto>>> GetLegalDocuments()
		{
			var result = new BaseResponse<List<CustomerOnboardingDocumentDto>>();

			try
			{
				var response = await _http.GetFromJsonAsync<BaseResponse<List<CustomerOnboardingDocumentDto>>>($"api/Onboardings/LegalDocuments");

				//if (response != null && response.Success)
				//{
				//	if (response.Result != null)
				//	{
				//		var model = new DashboardModel();

				//		foreach (var item in response.Result.)
				//		{

				//		}

				//		result.Result = model;
				//	}
				//}

				return response;
			}
			catch (Exception exception)
			{
				result.Success = false;
				result.Message = exception.Message;
				//exception.Redirect();
			}

			return result;
		}

		public async Task<BaseResponse<bool>> SubmitLegalDocuments(List<CustomerOnboardingDocumentDto> documents)
		{
			var response = new BaseResponse<bool>();

			try
			{
				var requestJson = JsonSerializer.Serialize(new SignedDocuments { Documents = documents});
				var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

				var result = await _http.PostAsync($"api/Onboardings/SubmitLegalDocuments", requestContent);
				result.EnsureSuccessStatusCode();

				response = await result.Content.ReadFromJsonAsync<BaseResponse<bool>>();
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<bool>> SubmitPaymentReceipt(PortalInvoicePaymentReceiptDto document)
		{
			var response = new BaseResponse<bool>();

			try
			{
				var requestJson = JsonSerializer.Serialize(document);
				var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

				var result = await _http.PostAsync($"api/Onboardings/SubmitPaymentReceipt", requestContent);
				result.EnsureSuccessStatusCode();

				response = await result.Content.ReadFromJsonAsync<BaseResponse<bool>>();
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}
	}

	public interface IOnboardingService
	{
		Task<BaseResponse<OnboardingStatusDto>> GetOnboardingStatus();

		Task<BaseResponse<UserDto>> GetCurrentUser();

		Task<BaseResponse<List<QueryDto>>> GetQueries();

		Task<BaseResponse<bool>> ResponseToQuery(ResponseToQueryDto responseToQuery);

		Task<BaseResponse<bool>> ChangePassword(ChangePasswordDto changePassword);

		Task<BaseResponse<bool>> ValidateCustomer(OnboardingModel onboarding, int currentStep);

		Task<BaseResponse<int>> OnboardCustomer(OnboardingModel onboarding);

		Task<BaseResponse<bool>> UpdateCustomer(UpdateCustomerDto customer);

		Task<BaseResponse<int>> SavedCustomer(OnboardingModel onboarding, int currentStep);

        Task<BaseResponse<OnboardingModel>> GetCustomer();

		Task<BaseResponse<CustomerModel>> GetCurrentCustomer();

		Task<BaseResponse<List<string>>> GetTinValidationPatterns();


		Task<BaseResponse<int>> OnboardProduct(OnboardingProductModel onboarding);

		Task<BaseResponse<bool>> UpdateProduct(UpdateProductDto product);

		Task<BaseResponse<OnboardingProductStatus>> GetOnboardingProductStatus();

		//Task<BaseResponse<OnboardingProductModel>> GetProduct(int productId);

		Task<BaseResponse<OnboardingProductModel>> GetProductDetail(int customerProductId);

		Task<BaseResponse<List<LookupModel<string, int>>>> GetContactPersonLookup();

		Task<BaseResponse<List<LookupModel<string, int>>>> GetAccountLookup(AccountType? accountType);

		Task<BaseResponse<DashboardDto>> GetDashboard();

		Task<BaseResponse<bool>> AcceptTerms(AcceptTermsDto acceptTerms);

        Task<BaseResponse<CustomFieldsModel>> GetCustomFields();

		Task<BaseResponse<List<LookupModel>>> GetParentLookup();

		Task<BaseResponse<List<CustomerOnboardingDocumentDto>>> GetLegalDocuments();

		Task<BaseResponse<bool>> SubmitLegalDocuments(List<CustomerOnboardingDocumentDto> documents);

		Task<BaseResponse<bool>> SubmitPaymentReceipt(PortalInvoicePaymentReceiptDto document);
	}


	public class OnboardingStepService
	{
		public int CurrentStep { get; set; } = 1;

		public bool HasSettlementBank { get; set; }
		public bool HasFeeAccounts { get; set; }

		public List<string> TinValidationPatterns { get; set; } = new List<string>();
	}
}
