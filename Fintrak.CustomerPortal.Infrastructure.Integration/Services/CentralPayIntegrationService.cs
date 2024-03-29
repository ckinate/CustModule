using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.CentralPay;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace Fintrak.CustomerPortal.Infrastructure.Integration.Services
{
    public class CentralPayIntegrationService : ICentralPayIntegrationService
    {
        private readonly ILogger<CentralPayIntegrationService> _logger;
        private readonly IConfiguration _configuration;

        public CentralPayIntegrationService(ILogger<CentralPayIntegrationService> logger, IConfiguration configuration) 
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<BaseResponse<CentralPayQueryModel>> QueryTransaction(string transactionId)
        {
            var result = new BaseResponse<CentralPayQueryModel>();

            var baseUrl = _configuration["CentralPay:BaseUrl"];
            var queryUrl = _configuration["CentralPay:QueryEndpoint"];
            var secretKey = _configuration["CentralPay:SecretKey"];
            var merchantId = _configuration["CentralPay:MerchantId"];

            var hash = $"{transactionId}{merchantId}{secretKey}";

            var request = new RestRequest($"{queryUrl}?merchant_id={merchantId}&transaction_id={transactionId}hash={hash}", Method.GET)
            {
                RequestFormat = DataFormat.Json,
            };

            IRestResponse<CentralPayQueryModel> response = null;

            var client = new RestClient(baseUrl);
            response = client.Execute<CentralPayQueryModel>(request);

            var data = JsonConvert.DeserializeObject<CentralPayQueryModel>(response.Content);

            if (data != null)
            {
                result.Result = data;
                result.Message = data.ResponseDesc;
            }
            else
            {
                result.Success = false;
                result.Message = "Unable to verify payment at this time.";
            }

            return result;
        }
    }
}
