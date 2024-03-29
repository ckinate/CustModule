using Fintrak.VendorPortal.Blazor.Shared.Models;
using Fintrak.VendorPortal.Blazor.Shared.Models.Enums;
using Fintrak.VendorPortal.Blazor.Shared.Models.Queries;
using Fintrak.VendorPortal.Blazor.Shared.Models.Users;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using Fintrak.VendorPortal.Blazor.Client.Onboarding.Models;
using Fintrak.VendorPortal.Blazor.Client.Onboarding.Pages;
using Fintrak.VendorPortal.Blazor.Shared.Models.Onboarding;

namespace Fintrak.VendorPortal.Blazor.Client.Onboarding
{
	public class OnboardingService : IOnboardingService
	{
		private readonly HttpClient _http;

		public OnboardingService(HttpClient http)
		{
			_http = http;
		}

		public async Task<BaseResponse<OnboardingStatus>> GetOnboardingStatus()
		{
			var response = new BaseResponse<OnboardingStatus>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<OnboardingStatus>>($"api/Onboardings/GetStatus");
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

		public async Task<BaseResponse<int>> OnboardVendor(OnboardingModel onboarding)
		{
			var response = new BaseResponse<int>();

			try
			{
				var request = new OnboardVendorDto();

				//OfficialInformation
				request.CompanyName = onboarding.OfficialInformation.CompanyName;
				request.RegistrationCertificateNumber = onboarding.OfficialInformation.RegistrationCertificateNumber;
				request.IncorporationDate = onboarding.OfficialInformation.IncorporationDate.Value;
				request.RegisterAddress = onboarding.OfficialInformation.RegisterAddress;
				request.TaxIdentificationNumber = onboarding.OfficialInformation.TaxIdentificationNumber;
				request.Country = onboarding.OfficialInformation.Country;
				request.OfficePhoneCallCode = onboarding.OfficialInformation.OfficePhoneCallCode;
				request.OfficePhoneNumber = onboarding.OfficialInformation.OfficePhoneNumber;
				request.MobilePhoneCallCode = onboarding.OfficialInformation.MobilePhoneCallCode;
				request.MobilePhoneNumber = onboarding.OfficialInformation.MobilePhoneNumber;
				request.Email = onboarding.OfficialInformation.Email;
				request.Fax = onboarding.OfficialInformation.Fax;
				request.Website = onboarding.OfficialInformation.Website;
				request.CategoryId = onboarding.OfficialInformation.CategoryId;
				request.CategoryName = onboarding.OfficialInformation.CategoryName;
				request.SubCategoryIds = string.Join(",", onboarding.OfficialInformation.SubCategoryIds);
				request.SubCategoryNames = onboarding.OfficialInformation.SubCategoryNames;
				request.UseForeignAccount = onboarding.OfficialInformation.UseForeignAccount;
				request.IsPublic = onboarding.OfficialInformation.IsPublic;
				request.IncludeLocalAccount = onboarding.OfficialInformation.IncludeLocalAccount;

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
				if(onboarding.LocalBank != null && onboarding.OfficialInformation.IncludeLocalAccount)
				{
					request.BankAccounts.Add(new UpsertBankAccountDto
					{
						BankName = onboarding.LocalBank.BankName,
						BankCode = onboarding.LocalBank.BankCode,
						BankAddress = onboarding.LocalBank.BankAddress,
						AccountName = onboarding.LocalBank.AccountName,
						AccountNumber = onboarding.LocalBank.AccountNumber,
						IsLocalAccount = true,
						Validated = onboarding.LocalBank.Validated 
					});
				}

				foreach (var foreignBank in onboarding.ForeignBanks.ForeignBanks)
				{
					request.BankAccounts.Add(new UpsertBankAccountDto
					{
						BankName = foreignBank.BankName,
						BankCode = foreignBank.BankCode,
						Country = foreignBank.Country,
						BankAddress = foreignBank.BankAddress,
						AccountName = foreignBank.AccountName,
						AccountNumber = foreignBank.AccountNumber
					});
				}

				//Questionnaires
				foreach (var questionnaire in onboarding.Questionnaires.Questionnaires)
				{
					request.Questionnaires.Add(new UpsertQuestionnaireDto
					{
						QuestionId = questionnaire.QuestionId,
						Question = questionnaire.Question,
						Response = questionnaire.Response,
						Compulsory = questionnaire.Compulsory 
					});
				}

				//Documents
				foreach (var document in onboarding.Documents.Documents)
				{
					request.Documents.Add(new UpsertDocumentDto
					{
						Title = document.Title,
						FileData = document.FileData,
						ContentType = document.ContentType,
						Size = document.Size,
						LocationUrl = document.LocationUrl,
						DocumentId = document.DocumentId,
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

		public async Task<BaseResponse<bool>> UpdateVendor(UpdateVendorDto customer)
		{
			var response = new BaseResponse<bool>();

			try
			{
				var requestJson = JsonSerializer.Serialize(customer);
				var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

				var result = await _http.PostAsync($"api/Onboardings/UpdateVendor", requestContent);
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

		public async Task<BaseResponse<OnboardingModel>> GetVendor()
		{
			var result = new BaseResponse<OnboardingModel>();

			try
			{
				var response = await _http.GetFromJsonAsync<BaseResponse<OnboardVendorDto>>($"api/Onboardings/GetVendorDetails");

				if (response != null && response.Success)
				{
					if (response.Result != null)
					{
						var model = new OnboardingModel();

						model.OfficialInformation.CompanyName = response.Result.CompanyName;
						model.OfficialInformation.RegistrationCertificateNumber = response.Result.RegistrationCertificateNumber;
						model.OfficialInformation.IncorporationDate = response.Result.IncorporationDate;
						model.OfficialInformation.RegisterAddress = response.Result.RegisterAddress;
						model.OfficialInformation.TaxIdentificationNumber = response.Result.TaxIdentificationNumber;
						model.OfficialInformation.Country = response.Result.Country;
						model.OfficialInformation.OfficePhoneCallCode = response.Result.OfficePhoneCallCode;
						model.OfficialInformation.OfficePhoneNumber = response.Result.OfficePhoneNumber;
						model.OfficialInformation.MobilePhoneCallCode = response.Result.MobilePhoneCallCode;
						model.OfficialInformation.MobilePhoneNumber = response.Result.MobilePhoneNumber;
						model.OfficialInformation.Email = response.Result.Email;
						model.OfficialInformation.Fax = response.Result.Fax;
						model.OfficialInformation.Website = response.Result.Website;
						model.OfficialInformation.CategoryId = response.Result.CategoryId;
						model.OfficialInformation.CategoryName = response.Result.CategoryName;
                        model.OfficialInformation.DueDiligenceCompleted = response.Result.DueDiligenceCompleted;

                        var subCategoryIds = Array.ConvertAll(response.Result.SubCategoryIds.Split(","), s => int.Parse(s));
						model.OfficialInformation.SubCategoryIds = subCategoryIds
							.Where(x => !String.IsNullOrWhiteSpace(x.ToString()))
							.Select(x => (int?)Convert.ToInt32(x)).ToArray();

						model.OfficialInformation.SubCategoryNames = response.Result.SubCategoryNames;
						model.OfficialInformation.UseForeignAccount = response.Result.UseForeignAccount;
						model.OfficialInformation.IsPublic = response.Result.IsPublic;
						model.OfficialInformation.IncludeLocalAccount = response.Result.IncludeLocalAccount;
                        model.OfficialInformation.Status = response.Result.Status;

						model.CanUpdate = response.Result.CanUpdate;

						//ContactPersons
						foreach (var item in response.Result.ContactPersons)
						{
							model.ContactPersons.ContactPersons.Add(new ContactPersonModel
							{
								FormId = Guid.NewGuid(),
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
							if (item.IsLocalAccount)
							{
								model.LocalBank = new LocalBankModel
								{
									BankName = item.BankName,
									BankCode = item.BankCode,
									BankAddress = item.BankAddress,
									AccountName = item.AccountName,
									AccountNumber = item.AccountNumber,
									Validated = item.Validated,	
								};
							}
							else
							{
								model.ForeignBanks.ForeignBanks.Add(new ForeignBankModel
								{
									FormId = Guid.NewGuid(),
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
						foreach (var item in response.Result.Questionnaires)
						{
							model.Questionnaires.Questionnaires.Add(new QuestionnaireModel
							{
								QuestionId = item.QuestionId,
								Question = item.Question,
								Response = item.Response,
								Compulsory = item.Compulsory
							});
						}

						//Documents
						foreach (var item in response.Result.Documents)
						{
							model.Documents.Documents.Add(new DocumentModel
							{
								FormId = Guid.NewGuid(),
								Title = item.Title,
								FileData = item.FileData,
								ContentType = item.ContentType,
								Size = item.Size,
								LocationUrl = item.LocationUrl ,
								DocumentId = item.DocumentId,
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
	}

	public interface IOnboardingService
	{
		Task<BaseResponse<OnboardingStatus>> GetOnboardingStatus();

		Task<BaseResponse<UserDto>> GetCurrentUser();

		Task<BaseResponse<List<QueryDto>>> GetQueries();

		Task<BaseResponse<bool>> ResponseToQuery(ResponseToQueryDto responseToQuery);

		Task<BaseResponse<bool>> ChangePassword(ChangePasswordDto changePassword);

		Task<BaseResponse<int>> OnboardVendor(OnboardingModel onboarding);

		Task<BaseResponse<bool>> UpdateVendor(UpdateVendorDto customer);

		Task<BaseResponse<OnboardingModel>> GetVendor();

		Task<BaseResponse<List<string>>> GetTinValidationPatterns();

	}


	public class OnboardingStepService
	{
		public int CurrentStep { get; set; } = 1;

		public List<string> TinValidationPatterns { get; set; } = new List<string>();
	}
}
