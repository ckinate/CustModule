using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Infrastructure.Integration.Services;
using Fintrak.CustomerPortal.Application.Common.Interfaces;

namespace Fintrak.CustomerPortal.Blazor.Server.Controllers
{
    public class IndustriesController : ApiControllerBase
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IConfiguration _configuration;
		private readonly ICustomerIntegrationService _customerIntegrationService;

		public IndustriesController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, ICustomerIntegrationService customerIntegrationService)
		{
			_webHostEnvironment = webHostEnvironment;
			_configuration = configuration;
			_customerIntegrationService = customerIntegrationService;
		}

        [HttpGet("lookup")]
        public async Task<ActionResult<BaseResponse<List<LookupModel<string, int>>>>> GetLookup([FromQuery]int? sectorId)
        {

            var response = new BaseResponse<List<LookupModel<string, int>>> { Result = new List<LookupModel<string, int>>() };

            var entities = await GetIndustries(sectorId);

            foreach (var entity in entities)
            {
                response.Result.Add(new LookupModel<string, int>
                {
                    Text = entity.Text,
                    Value = int.Parse(entity.Value),
                    AlternateText = entity.Text,
                    AlternateText2 = entity.Text,
                    HasAdditionalData = entity.HasAdditionalData,
                    AdditionalData = entity.AdditionalData,
                });
            }

            return response;
        }

        private async Task<List<LookupModel>> GetIndustries(int? sectorId)
        {
            var response = new List<LookupModel>();

            var useOfflineData = bool.Parse(_configuration["UseOfflineData"].ToString());
            if (useOfflineData)
            {
                var rootPath = _webHostEnvironment.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "JsonData/Industries.json");
                var jsonData = System.IO.File.ReadAllText(fullPath);

                if (string.IsNullOrWhiteSpace(jsonData))
                    return new List<LookupModel>();

                response = JsonConvert.DeserializeObject<List<LookupModel>>(jsonData);
            }
            else
            {
                var result = await _customerIntegrationService.GetIndustryLookUp(sectorId, false);
                response = result.Result;
            }

            return response;
        }
    }
}
