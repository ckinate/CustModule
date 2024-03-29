using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Infrastructure.Integration.Extensions;
using Fintrak.CustomerPortal.Infrastructure.Integration.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace Fintrak.CustomerPortal.Infrastructure.Integration.Services
{
    public class TokenIntegrationService : ITokenIntegrationService
    {
        private const string TOKEN_END_POINT = "/api/TokenAuth/Authenticate";

        private readonly string _apiServiceBaseUrl;
        private readonly ILogger<TokenIntegrationService> _logger;
        private readonly IConfiguration _configuration;

        public TokenIntegrationService(ILogger<TokenIntegrationService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            _apiServiceBaseUrl = _configuration["HttpClient:Services:ExpenseService:BaseUrl"];

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public BaseResponse<TokenModel> GetToken(string loginId, string password)
        {
            var result = new BaseResponse<TokenModel> { };

            try
            {
                var request = new RestRequest(TOKEN_END_POINT, Method.POST)
                {
                    RequestFormat = DataFormat.Json,
                };

                request.AddJsonBody(new
                {
                    userNameOrEmailAddress = loginId,
                    password = password 
                });

                IRestResponse<TokenModel> response = null;

                var client = new RestClient(_apiServiceBaseUrl);
                response = client.Execute<TokenModel>(request);

                var token = JsonConvert.DeserializeObject<TokenModel>(response.Content);

                result.Result = token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TokenIntegrationService][GetToken] - Unable to get token at this time.");
                result.Message = $"[TokenIntegrationService][GetToken] - Unable to get token at this time.";
                result.ValidationErrors.Add(ex.GetDetails());
                result.Success = false;
            }

            return result;
        }
    }

    public interface ITokenIntegrationService
    {
        BaseResponse<TokenModel> GetToken(string loginId, string password);
    }
}
