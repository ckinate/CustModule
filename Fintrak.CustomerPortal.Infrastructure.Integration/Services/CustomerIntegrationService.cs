using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using Fintrak.CustomerPortal.Infrastructure.Integration.Caching;
using Fintrak.CustomerPortal.Infrastructure.Integration.Extensions;
using Fintrak.CustomerPortal.Infrastructure.Integration.Models;
using Fintrak.CustomerPortal.Infrastructure.Integration.Models.Docusign;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace Fintrak.CustomerPortal.Infrastructure.Integration.Services
{
    public class CustomerIntegrationService : ICustomerIntegrationService
    {
        private const string COUNTRY_LOOKUP_END_POINT = "/api/services/app/Portal/GetCountryLookUp";
        private const string STATE_LOOKUP_END_POINT = "/api/services/app/Portal/GetStateLookUp";
		private const string SECTOR_LOOKUP_END_POINT = "/api/services/app/Portal/GetSectorLookUp";
		private const string INDUSTRY_LOOKUP_END_POINT = "/api/services/app/Portal/GetIndustryLookUp";
		private const string SUBSIDIARY_HEADS_LOOKUP_END_POINT = "/api/services/app/Portal/GetSubsidiaryHeadLookUp";
		private const string INSTITUTION_TYPE_LOOKUP_END_POINT = "/api/services/app/Portal/GetInstitutionTypeLookUp";
		private const string INSTITUTION_TYPE_DOCUMENT_LOOKUP_END_POINT = "/api/services/app/Portal/GetDocumentTypeForInstitutionLookUp";
        private const string PRODUCT_LOOKUP_END_POINT = "/api/services/app/Portal/GetProductLookUp";
        private const string PRODUCT_DOCUMENT_LOOKUP_END_POINT = "/api/services/app/Portal/GetDocumentTypeForProductLookUp";
		private const string PRODUCT_CUSTOM_FIELDS_END_POINT = "/api/services/app/Portal/GetProductCustomFields";
		private const string CUSTOMER_RECENT_TRACKERS_END_POINT = "/api/services/app/Portal/GetRecentTrackersByCode";
		private const string CUSTOMER_INVOICES_END_POINT = "/api/services/app/Portal/GetBillInvoices";
        private const string CUSTOMER_INVOICE_END_POINT = "/api/services/app/Portal/GetBillInvoice";
        private const string CUSTOMER_CUSTOM_FIELDS_END_POINT = "/api/services/app/Portal/GetCustomerCustomFields";

        private const string MAKE_PAYMENT_REQUEST_END_POINT = "/api/services/app/Portal/MakePaymentRequest";
        private const string VERIFY_PAYMENT_REQUEST_END_POINT = "/api/services/app/Portal/VerifyPaymentRequest";

        private const string CUSTOMER_SETTING_END_POINT = "/api/services/app/Portal/GetSettings";

		private const string PROCESS_DOCUSIGN_REQUEST_END_POINT = "/api/services/app/Portal/ProcessDocusignRequest";

		private const string TEST_CUSTOMER_ACCEPTANCE_END_POINT = "/api/services/app/Portal/AcceptOnboadingData";

		private const string VALIDATE_ACCOUNT_END_POINT = "/api/services/app/Integration/ValidateAccount";

		private const string FILE_SIZE_END_POINT = "/api/services/app/Portal/GetSize";
        private const string SUBMIT_PAYMENT_RECEIPT_END_POINT = "/api/services/app/Portal/SubmitPaymentReceipt";
		private const string GET_LEGAL_DOCUMENTS_END_POINT = "/api/services/app/Portal/GetCustomerLegalDocuments";
		private const string UPLOAD_SIGNED_DOCUMENTS_END_POINT = "/api/services/app/Portal/UploadSignedDocuments";
		private const string SUBMIT_SIGNED_DOCUMENTS_END_POINT = "/api/services/app/Portal/SubmitSignedDocuments";
		private const string UPLOAD_AND_SUBMIT_SIGNED_DOCUMENTS_END_POINT = "/api/services/app/Portal/UploadAndSubmitSignedDocuments";

		private readonly string _apiServiceBaseUrl;
        private readonly ILogger<CustomerIntegrationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICacheService _cacheService;
        private readonly CacheOption _cacheOption;

        public CustomerIntegrationService(ILogger<CustomerIntegrationService> logger,
            IConfiguration configuration,
            ICacheService cacheService)
        {
            _logger = logger;
            _configuration = configuration;
            _cacheService = cacheService;

            _cacheOption = new CacheOption
            {
                AbsoluteExpiration = 1,
                AbsoluteExpirationPeriod = CachePeriod.Days,
                SlidingExpiration = 1,
                SlidingExpirationPeriod = CachePeriod.Days
            };

            _apiServiceBaseUrl = _configuration["HttpClient:Services:CustomerService:BaseUrl"];

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

		public async Task<BaseResponse<List<LookupModel>>> GetCountryLookUp(bool includeAll = false)
		{
			var result = new BaseResponse<List<LookupModel>>();

            var request = new RestRequest($"{COUNTRY_LOOKUP_END_POINT}?includeAll={includeAll}&forceCacheRefresh=true", Method.GET)
			{
				RequestFormat = DataFormat.Json,
			};

			IRestResponse<ResultModel<List<LookupModel>>> response = null;

			var client = new RestClient(_apiServiceBaseUrl);
			response = client.Execute<ResultModel<List<LookupModel>>>(request);

			var data = JsonConvert.DeserializeObject<ResultModel<List<LookupModel>>>(response.Content);

			if(data != null)
			{
				if(data.Result != null)
				{
                    result.Result = data.Result.Response;
                    result.Success = data.Success;
                }
			}

			return result;
		}

		public async Task<BaseResponse<List<LookupModel>>> GetStateLookUp(int? countryId, bool includeAll = false)
		{
            var result = new BaseResponse<List<LookupModel>>();

            var request = new RestRequest($"{STATE_LOOKUP_END_POINT}?countryId={countryId}&includeAll ={includeAll}&forceCacheRefresh=true", Method.GET)
			{
				RequestFormat = DataFormat.Json,
			};

			IRestResponse<ResultModel<List<LookupModel>>> response = null;

			var client = new RestClient(_apiServiceBaseUrl);
			response = client.Execute<ResultModel<List<LookupModel>>>(request);

			var data = JsonConvert.DeserializeObject<ResultModel<List<LookupModel>>>(response.Content);

            if (data != null)
            {
                if (data.Result != null)
                {
                    result.Result = data.Result.Response;
                    result.Success = data.Success;
                }
            }

            return result;
        }

		public async Task<BaseResponse<List<LookupModel>>> GetSectorLookUp(bool includeAll = false)
		{
            var result = new BaseResponse<List<LookupModel>>();

            var request = new RestRequest($"{SECTOR_LOOKUP_END_POINT}?includeAll={includeAll}&forceCacheRefresh=true", Method.GET)
			{
				RequestFormat = DataFormat.Json,
			};

            IRestResponse<ResultModel<List<LookupModel>>> response = null;

            var client = new RestClient(_apiServiceBaseUrl);
            response = client.Execute<ResultModel<List<LookupModel>>>(request);

            var data = JsonConvert.DeserializeObject<ResultModel<List<LookupModel>>>(response.Content);

            if (data != null)
            {
                if (data.Result != null)
                {
                    result.Result = data.Result.Response;
                    result.Success = data.Success;
                }
            }

            return result;
        }

		public async Task<BaseResponse<List<LookupModel>>> GetIndustryLookUp(int? sectorId, bool includeAll = false)
		{
            var result = new BaseResponse<List<LookupModel>>();

            var request = new RestRequest($"{INDUSTRY_LOOKUP_END_POINT}?sectorId={sectorId}&includeAll ={includeAll}&forceCacheRefresh=true", Method.GET)
			{
				RequestFormat = DataFormat.Json,
			};

            IRestResponse<ResultModel<List<LookupModel>>> response = null;

            var client = new RestClient(_apiServiceBaseUrl);
            response = client.Execute<ResultModel<List<LookupModel>>>(request);

            var data = JsonConvert.DeserializeObject<ResultModel<List<LookupModel>>>(response.Content);

            if (data != null)
            {
                if (data.Result != null)
                {
                    result.Result = data.Result.Response;
                    result.Success = data.Success;
                }
            }

            return result;
        }

		public async Task<BaseResponse<List<LookupModel>>> GetSubsidiaryHeadsLookUp(string customerCode, bool includeAll = false)
		{
			var result = new BaseResponse<List<LookupModel>>();

			var request = new RestRequest($"{SUBSIDIARY_HEADS_LOOKUP_END_POINT}?customerCode={customerCode}&includeAll ={includeAll}&forceCacheRefresh=true", Method.GET)
			{
				RequestFormat = DataFormat.Json,
			};

			IRestResponse<ResultModel<List<LookupModel>>> response = null;

			var client = new RestClient(_apiServiceBaseUrl);
			response = client.Execute<ResultModel<List<LookupModel>>>(request);

			var data = JsonConvert.DeserializeObject<ResultModel<List<LookupModel>>>(response.Content);

			if (data != null)
			{
				if (data.Result != null)
				{
					result.Result = data.Result.Response;
					result.Success = data.Success;
				}
			}

			return result;
		}

		public async Task<BaseResponse<List<LookupModel>>> GetInstitutionTypeLookUp(int? parentId, bool? parentOnly = false, bool includeAll = false)
		{
            var result = new BaseResponse<List<LookupModel>>();

            var request = new RestRequest($"{INSTITUTION_TYPE_LOOKUP_END_POINT}?parentId={parentId}&parentOnly={parentOnly}&includeAll={includeAll}&forceCacheRefresh=true", Method.GET)
			{
				RequestFormat = DataFormat.Json,
			};

            IRestResponse<ResultModel<List<LookupModel>>> response = null;

            var client = new RestClient(_apiServiceBaseUrl);
            response = client.Execute<ResultModel<List<LookupModel>>>(request);

            var data = JsonConvert.DeserializeObject<ResultModel<List<LookupModel>>>(response.Content);

            if (data != null)
            {
                if (data.Result != null)
                {
                    result.Result = data.Result.Response;
                    result.Success = data.Success;
                }
            }

            return result;
        }

		public async Task<BaseResponse<List<LookupModel>>> GetInstitutionTypeDocumentLookUp(int? institutionTypeId, bool includeAll = false)
		{
            var result = new BaseResponse<List<LookupModel>>();

            var request = new RestRequest($"{INSTITUTION_TYPE_DOCUMENT_LOOKUP_END_POINT}?institutionTypeId={institutionTypeId}&includeAll={includeAll}&forceCacheRefresh=true", Method.GET)
			{
				RequestFormat = DataFormat.Json,
			};

            IRestResponse<ResultModel<List<LookupModel>>> response = null;

            var client = new RestClient(_apiServiceBaseUrl);
            response = client.Execute<ResultModel<List<LookupModel>>>(request);

            var data = JsonConvert.DeserializeObject<ResultModel<List<LookupModel>>>(response.Content);

            if (data != null)
            {
                if (data.Result != null)
                {
                    result.Result = data.Result.Response;
                    result.Success = data.Success;
                }
            }

            return result;
        }

        public async Task<BaseResponse<List<LookupModel>>> GetProductLookUp(string customerCode, bool includeAll = false)
        {
            var result = new BaseResponse<List<LookupModel>>();

            var request = new RestRequest($"{PRODUCT_LOOKUP_END_POINT}?customerCode={customerCode}&includeAll ={includeAll}&forceCacheRefresh=true", Method.GET)
            {
                RequestFormat = DataFormat.Json,
            };

            IRestResponse<ResultModel<List<LookupModel>>> response = null;

            var client = new RestClient(_apiServiceBaseUrl);
            response = client.Execute<ResultModel<List<LookupModel>>>(request);

            var data = JsonConvert.DeserializeObject<ResultModel<List<LookupModel>>>(response.Content);

            if (data != null)
            {
                if (data.Result != null)
                {
                    result.Result = data.Result.Response;
                    result.Success = data.Success;
                }
            }

            return result;
        }

		public async Task<BaseResponse<List<CustomFieldDto>>> GetProductCustomFields(int productId)
		{
			var result = new BaseResponse<List<CustomFieldDto>>();

			var request = new RestRequest($"{PRODUCT_CUSTOM_FIELDS_END_POINT}?productId={productId}", Method.GET)
			{
				RequestFormat = DataFormat.Json,
			};

			IRestResponse<ResultModel<List<CustomFieldModel>>> response = null;

			var client = new RestClient(_apiServiceBaseUrl);
			response = client.Execute<ResultModel<List<CustomFieldModel>>>(request);

			var data = JsonConvert.DeserializeObject<ResultModel<List<CustomFieldModel>>>(response.Content);

			if (data != null)
			{
				if (data.Result != null)
				{
					result.Result = data.Result.Response.Select(c=> new CustomFieldDto
                    {
						CustomFieldId = c.Id,
                        CustomField = c.Title,
                        IsCompulsory = c.IsCompulsory,
                    }).ToList();
					result.Success = data.Success;
				}
			}

			return result;
		}

		public async Task<BaseResponse<List<LookupModel>>> GetProductDocumentLookUp(int productId, string customerCode, bool includeAll = false)
		{
            var result = new BaseResponse<List<LookupModel>>();

            var request = new RestRequest($"{PRODUCT_DOCUMENT_LOOKUP_END_POINT}?productId={productId}&customerCode={customerCode}&includeAll ={includeAll}&forceCacheRefresh=true", Method.GET)
			{
				RequestFormat = DataFormat.Json,
			};

            IRestResponse<ResultModel<List<LookupModel>>> response = null;

            var client = new RestClient(_apiServiceBaseUrl);
            response = client.Execute<ResultModel<List<LookupModel>>>(request);

            var data = JsonConvert.DeserializeObject<ResultModel<List<LookupModel>>>(response.Content);

            if (data != null)
            {
                if (data.Result != null)
                {
                    result.Result = data.Result.Response;
                    result.Success = data.Success;
                }
            }

            return result;
        }

        public async Task<BaseResponse<string>> GetCustomerNotificationEmail()
        {
            var result = new BaseResponse<string>();

            try
            {
                var request = new RestRequest(CUSTOMER_SETTING_END_POINT, Method.GET)
                {
                    RequestFormat = DataFormat.Json,
                };

                IRestResponse<ResultModel<CustomerSettingModel>> response = null;

                var client = new RestClient(_apiServiceBaseUrl);
                response = client.Execute<ResultModel<CustomerSettingModel>>(request);

                var setting = JsonConvert.DeserializeObject<ResultModel<CustomerSettingModel>>(response.Content);
                if (response != null)
                {
                    if (setting.Success && setting.Result != null)
                        result.Result = setting.Result.Response.NotificationEmail;
                    else
                    {
                        result.Success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[CustomerIntegrationService][GetCustomerNotificationEmail] - Unable to get customer settings at this time.");
                result.Message = $"[CustomerIntegrationService][GetCustomerNotificationEmail] - Unable to get customer settings at this time.";
                result.ValidationErrors.Add(ex.GetDetails());
                result.Success = false;
            }

            return result;
        }

        public async Task<BaseResponse<List<CustomFieldResponseDto>>> GetCustomerCustomFields(string customerCode = "")
        {
            var result = new BaseResponse<List<CustomFieldResponseDto>>();

            var request = new RestRequest($"{CUSTOMER_CUSTOM_FIELDS_END_POINT}?customerCode={customerCode}", Method.GET)
            {
                RequestFormat = DataFormat.Json,
            };

            IRestResponse<ResultModel<List<CustomFieldResponseDto>>> response = null;

            var client = new RestClient(_apiServiceBaseUrl);
            response = client.Execute<ResultModel<List<CustomFieldResponseDto>>>(request);

            var data = JsonConvert.DeserializeObject<ResultModel<List<CustomFieldResponseDto>>>(response.Content);

            if (data != null)
            {
                if (data.Result != null)
                {
                    result.Result = data.Result.Response;
                    result.Success = data.Success;
                }
            }

            return result;
        }

        public async Task<BaseResponse<RecentCustomerOnboardingTrackerDto>> GetRecentTrackers(string customerCode)
		{
			var result = new BaseResponse<RecentCustomerOnboardingTrackerDto>();

			var request = new RestRequest($"{CUSTOMER_RECENT_TRACKERS_END_POINT}?customerCode={customerCode}", Method.GET)
			{
				RequestFormat = DataFormat.Json,
			};

			IRestResponse<ResultModel<RecentCustomerOnboardingTrackerDto>> response = null;

			var client = new RestClient(_apiServiceBaseUrl);
			response = client.Execute<ResultModel<RecentCustomerOnboardingTrackerDto>>(request);

			var data = JsonConvert.DeserializeObject<ResultModel<RecentCustomerOnboardingTrackerDto>>(response.Content);

			if (data != null && data.Result != null)
			{
                result.Result = data.Result.Response;
			}

			return result;
		}

        public async Task<BaseResponse<List<BillInvoiceDto>>> GetInvoices(string customerCode, int pageIndex, int pageSize)
        {
            var result = new BaseResponse<List<BillInvoiceDto>>();

            var request = new RestRequest($"{CUSTOMER_INVOICES_END_POINT}?customerCode={customerCode}&pageIndex={pageIndex}&pageSize={pageSize}", Method.GET)
            {
                RequestFormat = DataFormat.Json,
            };

            IRestResponse<ResultModel<List<BillInvoiceDto>>> response = null;

            var client = new RestClient(_apiServiceBaseUrl);
            response = client.Execute<ResultModel<List<BillInvoiceDto>>>(request);

            var data = JsonConvert.DeserializeObject<ResultModel<List<BillInvoiceDto>>>(response.Content);

            if (data != null && data.Result != null && data.Result.Response != null)
            {
                result.Result = data.Result.Response;
            }

            return result;
        }

        public async Task<BaseResponse<BillInvoiceDto>> GetInvoice(int invoiceId)
		{
			var result = new BaseResponse<BillInvoiceDto>();

			var request = new RestRequest($"{CUSTOMER_INVOICE_END_POINT}?invoiceId={invoiceId}", Method.GET)
			{
				RequestFormat = DataFormat.Json,
			};

			IRestResponse<ResultModel<BillInvoiceDto>> response = null;

			var client = new RestClient(_apiServiceBaseUrl);
			response = client.Execute<ResultModel<BillInvoiceDto>>(request);

			var data = JsonConvert.DeserializeObject<ResultModel<BillInvoiceDto>>(response.Content);

			if (data != null && data.Result != null)
			{
				result.Result = data.Result.Response;
			}

			return result;
		}

        public async Task<BaseResponse<CentralPayLogDto>> CreatePaymentRequest(int invoiceId)
        {
            var result = new BaseResponse<CentralPayLogDto>();

            var request = new RestRequest($"{MAKE_PAYMENT_REQUEST_END_POINT}?invoiceId={invoiceId}", Method.POST)
            {
                RequestFormat = DataFormat.Json,
            };

            IRestResponse<ResultModel<CentralPayLogDto>> response = null;

            var client = new RestClient(_apiServiceBaseUrl);
            response = client.Execute<ResultModel<CentralPayLogDto>>(request);

            var data = JsonConvert.DeserializeObject<ResultModel<CentralPayLogDto>>(response.Content);

            if (data != null && data.Result != null)
            {
                result.Result = data.Result.Response;
            }

            return result;
        }

        public async Task<BaseResponse<CentralPayLogDto>> VerifyPaymentRequest(string requestId)
        {
            var result = new BaseResponse<CentralPayLogDto>();

            var request = new RestRequest($"{VERIFY_PAYMENT_REQUEST_END_POINT}?requestId={requestId}", Method.POST)
            {
                RequestFormat = DataFormat.Json,
            };

            IRestResponse<ResultModel<CentralPayLogDto>> response = null;

            var client = new RestClient(_apiServiceBaseUrl);
            response = client.Execute<ResultModel<CentralPayLogDto>>(request);

            var data = JsonConvert.DeserializeObject<ResultModel<CentralPayLogDto>>(response.Content);

            if (data != null && data.Result != null)
            {
                result.Result = data.Result.Response;
            }

            return result;
        }

		public async Task<BaseResponse<bool>> ProcessDocusignRequest(DocusignWebhookCompletedModel model)
		{
			var result = new BaseResponse<bool>();

			var request = new RestRequest($"{PROCESS_DOCUSIGN_REQUEST_END_POINT}", Method.POST)
			{
				RequestFormat = DataFormat.Json,
			};

            request.AddJsonBody(model);

			IRestResponse<ResultModel<bool>> response = null;

			var client = new RestClient(_apiServiceBaseUrl);
			response = client.Execute<ResultModel<bool>>(request);

			var data = JsonConvert.DeserializeObject<ResultModel<bool>>(response.Content);

			if (data != null && data.Result != null)
			{
				result.Result = data.Result.Response;
			}

			return result;
		}

		//public async Task<BaseResponse<ValidateAccountResponseDto>> ValidateAccount(string bankCode, string accountName, string accountNumbed)
		//{
		//	var result = new BaseResponse<ValidateAccountResponseDto>();

		//	var request = new RestRequest($"{VALIDATE_ACCOUNT_END_POINT}", Method.POST)
		//	{
		//		RequestFormat = DataFormat.Json,
		//	};

		//	IRestResponse<ResultModel<ValidateAccountResponseDto>> response = null;

		//	var client = new RestClient(_apiServiceBaseUrl);
		//	response = client.Execute<ResultModel<ValidateAccountResponseDto>>(request);

		//	var data = JsonConvert.DeserializeObject<ResultModel<ValidateAccountResponseDto>>(response.Content);

		//	if (data != null && data.Result != null)
		//	{
		//		result.Result = data.Result.Response;
		//	}

		//	return result;
		//}


		//------Test-------
		public async Task<BaseResponse<AcceptOnboadingDataResponseDto>> TestCustomerDataAcceptance(OnboardCustomerDto onboardCustomer)
        {
			var result = new BaseResponse<AcceptOnboadingDataResponseDto> { };

			try
			{
				var request = new RestRequest($"{TEST_CUSTOMER_ACCEPTANCE_END_POINT}", Method.POST)
				{
					RequestFormat = DataFormat.Json,
				};

				request.AddJsonBody(onboardCustomer);

				IRestResponse<ResultModel<AcceptOnboadingDataResponseDto>> response = null;

				var client = new RestClient(_apiServiceBaseUrl);
				response = client.Execute<ResultModel<AcceptOnboadingDataResponseDto>>(request);

				var newResponse = JsonConvert.DeserializeObject<ResultModel<AcceptOnboadingDataResponseDto>>(response.Content);
				if (newResponse != null)
				{
					if (newResponse.Result != null && newResponse.Result.Response != null && newResponse.Result.Successful)
					{
						result.Result = newResponse.Result.Response;
						
					}
				}
				else
				{
                    result.Success = false;
					throw new Exception($"Unable to onboard customer.");
				}
			}
			catch (Exception ex)
			{
				result.Success = false;
				_logger.LogError(ex, $"[AcceptOnboadingData] - Unable to onboard customer at this time. Error: {ex.GetDetails()}");
				result.Message = $"[AcceptOnboadingDataw] - Unable to onboard customer at this time.Error: {ex.GetDetails()}";
			}

			return result;
		}


		public async Task<BaseResponse<long>> GetSize(int? documentTypeId)
		{
			var result = new BaseResponse<long>();

			var request = new RestRequest($"{FILE_SIZE_END_POINT}?documentTypeId={documentTypeId}", Method.GET)
			{
				RequestFormat = DataFormat.Json,
			};

			IRestResponse<ResultModel<long>> response = null;

			var client = new RestClient(_apiServiceBaseUrl);
			response = client.Execute<ResultModel<long>>(request);

			var data = JsonConvert.DeserializeObject<ResultModel<long>>(response.Content);

			if (data != null && data.Result != null)
			{
				result.Result = data.Result.Response;
			}

			return result;
		}

		public async Task<BaseResponse<bool>> SubmitPaymentReceipt(PortalInvoicePaymentReceiptDto model)
		{
			var result = new BaseResponse<bool>();

			var request = new RestRequest($"{SUBMIT_PAYMENT_RECEIPT_END_POINT}", Method.POST)
			{
				RequestFormat = DataFormat.Json,
			};

			request.AddJsonBody(model);

			IRestResponse<ResultModel<bool>> response = null;

			var client = new RestClient(_apiServiceBaseUrl);
			response = client.Execute<ResultModel<bool>>(request);

			var data = JsonConvert.DeserializeObject<ResultModel<bool>>(response.Content);

			if (data != null && data.Result != null)
			{
				result.Result = data.Result.Response;
			}

			return result;
		}

		public async Task<BaseResponse<List<CustomerOnboardingDocumentDto>>> GetCustomerLegalDocuments(string customerCode)
		{
			var result = new BaseResponse<List<CustomerOnboardingDocumentDto>>();

			var request = new RestRequest($"{GET_LEGAL_DOCUMENTS_END_POINT}?customerCode={customerCode}", Method.GET)
			{
				RequestFormat = DataFormat.Json,
			};

			IRestResponse<ResultModel<List<CustomerOnboardingDocumentDto>>> response = null;

			var client = new RestClient(_apiServiceBaseUrl);
			response = client.Execute<ResultModel<List<CustomerOnboardingDocumentDto>>>(request);

			var data = JsonConvert.DeserializeObject<ResultModel<List<CustomerOnboardingDocumentDto>>>(response.Content);

			if (data != null && data.Result != null)
			{
				result.Result = data.Result.Response;
			}

			return result;
		}

		public async Task<BaseResponse<bool>> UploadSignedDocuments(string customerCode, List<CustomerOnboardingDocumentDto> documents)
		{
			var result = new BaseResponse<bool>();

			var request = new RestRequest($"{UPLOAD_SIGNED_DOCUMENTS_END_POINT}?customerCode={customerCode}", Method.POST)
			{
				RequestFormat = DataFormat.Json,
			};

			request.AddJsonBody(documents);

			IRestResponse<ResultModel<bool>> response = null;

			var client = new RestClient(_apiServiceBaseUrl);
			response = client.Execute<ResultModel<bool>>(request);

			var data = JsonConvert.DeserializeObject<ResultModel<bool>>(response.Content);

			if (data != null && data.Result != null)
			{
				result.Result = data.Result.Response;
			}

			return result;
		}

		public async Task<BaseResponse<bool>> SubmitSignedDocuments(string customerCode)
		{
			var result = new BaseResponse<bool>();

			var request = new RestRequest($"{SUBMIT_SIGNED_DOCUMENTS_END_POINT}?customerCode={customerCode}", Method.GET)
			{
				RequestFormat = DataFormat.Json,
			};

			IRestResponse<ResultModel<bool>> response = null;

			var client = new RestClient(_apiServiceBaseUrl);
			response = client.Execute<ResultModel<bool>>(request);

			var data = JsonConvert.DeserializeObject<ResultModel<bool>>(response.Content);

			if (data != null && data.Result != null)
			{
				result.Result = data.Result.Response;
			}

			return result;
		}

		public async Task<BaseResponse<bool>> UploadAndSubmitSignedDocuments(string customerCode, List<CustomerOnboardingDocumentDto> documents)
		{
			var result = new BaseResponse<bool>();

			var request = new RestRequest($"{UPLOAD_AND_SUBMIT_SIGNED_DOCUMENTS_END_POINT}?customerCode={customerCode}", Method.POST)
			{
				RequestFormat = DataFormat.Json,
			};

			request.AddJsonBody(documents);

			IRestResponse<ResultModel<bool>> response = null;

			var client = new RestClient(_apiServiceBaseUrl);
			client.Timeout = 120 * 1000;

			response = client.Execute<ResultModel<bool>>(request);

			var data = JsonConvert.DeserializeObject<ResultModel<bool>>(response.Content);

			if (data != null && data.Result != null)
			{
				result.Result = data.Result.Response;
			}

			return result;
		}

	}

    
}
