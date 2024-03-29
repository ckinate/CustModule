using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Infrastructure.Integration.Caching;
using Fintrak.CustomerPortal.Infrastructure.Integration.Extensions;
using Fintrak.CustomerPortal.Infrastructure.Integration.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace Fintrak.CustomerPortal.Infrastructure.Integration.Services
{
    public class BankIntegrationService : IBankIntegrationService
    {
        private const string BANK_LIST_END_POINT = "/api/services/app/VendorIntegration/GetBanks";
        private const string ACCOUNT_VALIDATION_END_POINT = "​/api/services/app/VendorIntegration/ValidateAccountnumber";

        private readonly string _apiServiceBaseUrl;
        private readonly ILogger<BankIntegrationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICacheService _cacheService;
        private readonly CacheOption _cacheOption;

        public BankIntegrationService(ILogger<BankIntegrationService> logger, 
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

            _apiServiceBaseUrl = _configuration["HttpClient:Services:ExpenseService:BaseUrl"];

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public async Task<BaseResponse<List<BankModel>>> GetBankList(bool onlyActive = true, bool cacheEnabled = false, bool forceCacheRefresh = false, string cacheKey = "Banks")
        {
            var result = new BaseResponse<List<BankModel>> { Result = new List<BankModel>() };

            try
            {
                var response = await GetBanks(cacheEnabled, forceCacheRefresh, cacheKey);

                if (response != null)
                {
                    var activeBanks = new List<BankModel>();
                    if (onlyActive)
                        activeBanks = response.Result.Where(c => c.IsActive).ToList();

                    result.Result = activeBanks;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[BankIntegrationService][GetBankList] - Unable to get banks at this time.");
                result.Message = $"[BankIntegrationService][GetBankList] - Unable to get banks at this time.";
                result.ValidationErrors.Add(ex.GetDetails());
				result.Success = false;
			}

            return result;
        }

        public async Task<BaseResponse<List<LookupModel<string,string>>>> GetBankLookUp(bool includeAll = false, bool cacheEnabled = false, bool forceCacheRefresh = false, string cacheKey = "Banks")
        {

            var result = new BaseResponse<List<LookupModel<string, string>>> { Result = new List<LookupModel<string, string>>() };

            try
            {
                var response = await GetBanks(cacheEnabled, forceCacheRefresh, cacheKey);

                if (includeAll)
                    result.Result.Add(new LookupModel<string, string> { Text = "All", Value = Guid.Empty.ToString(), AlternateText = "--All--", AlternateText2 = "" });

                if (response != null)
                {
                    foreach (var item in response.Result)
                    {
                        if (item.IsActive)
                        {
                            var lookUpItem = new LookupModel<string, string> { Text = item.BankName, Value = item.BankCode.ToString(), AlternateText = $"{item.BankName} - {item.BankCode}", AlternateText2 = $"{item.BankCode} - {item.BankName}" };

                            lookUpItem.HasAdditionalData = true;
                            lookUpItem.AdditionalData.Add("Id", item.Id.ToString());

                            result.Result.Add(lookUpItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[BankIntegrationService][GetBankLookUp] - Unable to get banks at this time.");
                result.Message = $"[BankIntegrationService][GetBankLookUp] - Unable to get banks at this time.";
                result.ValidationErrors.Add(ex.GetDetails());
				result.Success = false;
			}

            return result;
        }

        private async Task<BankListModel> GetBanks(bool cacheEnabled = false, bool forceCacheRefresh = false, string cacheKey = "Banks")
        {
            CacheResult<BankListModel> cacheResult = null;
            if (cacheEnabled)
            {
                cacheResult = await _cacheService.GetAsync<BankListModel>(cacheKey);
            }

            if (cacheEnabled && cacheResult != null && cacheResult.ResultExist && !forceCacheRefresh)
            {
                return cacheResult.Result;
            }
            else
            {
                var request = new RestRequest(BANK_LIST_END_POINT, Method.GET)
                {
                    RequestFormat = DataFormat.Json,
                };

                IRestResponse<BankListModel> response = null;

                var client = new RestClient(_apiServiceBaseUrl);
                response = client.Execute<BankListModel>(request);

                var banks = JsonConvert.DeserializeObject<BankListModel>(response.Content);

                if (cacheEnabled)
                {
                    await _cacheService.SetAsync<BankListModel>(cacheKey, banks, _cacheOption);
                }

                return banks;
            }
            
        }

        public async Task<BaseResponse<AccountValidationModel>> ValidateAccount(string bankCode, string accountNumber, string accountName)
        {
            var result = new BaseResponse<AccountValidationModel>();

            var response = await ValidateAccount(bankCode, accountNumber);
            if(response != null && response.Successful && response.Response != null)
            {
				response.Response.BankCode = response.Response.BankCode;
				response.Response.AccountName = response.Response.AccountName;
				response.Response.AccountNumber = response.Response.AccountNumber;

				if (response.Response.Message == "Validation successful.")
				{
					response.Response.Valid = response.Response.AccountName == accountName;
					response.Response.Message = $"Account Name: {response.Response.AccountName}, Account Number: {response.Response.AccountNumber}";
				}
				else
				{
					response.Response.Valid = false;
					response.Response.Message = response.Response.Message;
				}

                result.Result = response.Response;
			}
            else
            {
                result.Success = response.Successful;
                result.Message = response.Message;
			}

            return result;
		}

        private async Task<ResponseModel<AccountValidationModel>> ValidateAccount(string bankCode, string accountNumber)
		{
			var result = new ResponseModel<AccountValidationModel> { Response = new AccountValidationModel() };

			try
			{
				if (!string.IsNullOrEmpty(bankCode) && !string.IsNullOrEmpty(accountNumber))
				{
					var request = new RestRequest(ACCOUNT_VALIDATION_END_POINT, Method.POST)
					{
						RequestFormat = DataFormat.Json,
					};

					request.AddJsonBody(new
					{
						bankCode,
						accountNumber
					});

					IRestResponse<AccountValidationResultModel> response = null;

					var client = new RestClient(_apiServiceBaseUrl);
					response = client.Execute<AccountValidationResultModel>(request);

					var validationResult = JsonConvert.DeserializeObject<AccountValidationResultModel>(response.Content);

					if (validationResult != null && validationResult.Success)
					{
						result.Response = validationResult.Result;
						result.Successful = true;
					}
				}
				else
				{
					result.Message = $"[BankIntegrationService][ValidateAccount] - Invalid parameters.";
					result.Successful = false;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"[BankIntegrationService][ValidateAccount] - Unable to validate account at this time.");
				result.Message = $"[BankIntegrationService][ValidateAccount] - Unable to validate account at this time.";
				result.ValidationErrors.Add(ex.GetDetails());
				result.Successful = false;
			}

			return result;
		}

	}

	public interface IBankIntegrationService
    {
        Task<BaseResponse<List<BankModel>>> GetBankList(bool onlyActive = true, bool cacheEnabled = false, bool forceCacheRefresh = false, string cacheKey = "Banks");

        Task<BaseResponse<List<LookupModel<string, string>>>> GetBankLookUp(bool includeAll = false, bool cacheEnabled = false, bool forceCacheRefresh = false, string cacheKey = "Banks");

        Task<BaseResponse<AccountValidationModel>> ValidateAccount(string bankCode, string accountNumber, string accountName);

	}
}
