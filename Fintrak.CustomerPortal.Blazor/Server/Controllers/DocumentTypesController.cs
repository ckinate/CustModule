using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Infrastructure.Integration.Services;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Azure;

namespace Fintrak.CustomerPortal.Blazor.Server.Controllers
{
    public class DocumentTypesController : ApiControllerBase
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IConfiguration _configuration;
		private readonly ICustomerIntegrationService _customerIntegrationService;

		public DocumentTypesController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, ICustomerIntegrationService customerIntegrationService)
		{
			_webHostEnvironment = webHostEnvironment;
			_configuration = configuration;
			_customerIntegrationService = customerIntegrationService;
		}

        [HttpGet("institutiondocuments")]
        public async Task<ActionResult<BaseResponse<List<LookupModel<string, int>>>>> GetInstitutionDocuments([FromQuery] int? institutionTypeId)
        {

            var response = new BaseResponse<List<LookupModel<string, int>>> { Result = new List<LookupModel<string, int>>() };

            var entities = await GetInstitutionTypeDocuments(institutionTypeId);

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

		[HttpGet("documenttypesize")]
		public async Task<ActionResult<BaseResponse<long>>> GetDocumentTypeSize([FromQuery] int? documentTypeId)
		{
			var response = new BaseResponse<long> ();

			response = await _customerIntegrationService.GetSize(documentTypeId);
			
			return response;
		}

		private async Task<List<LookupModel>> GetInstitutionTypeDocuments(int? institutionTypeId)
        {
            var response = new List<LookupModel>();

            var useOfflineData = bool.Parse(_configuration["UseOfflineData"].ToString());
            if (useOfflineData)
            {
                var rootPath = _webHostEnvironment.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "JsonData/InstitutionTypeDocuments.json");
                var jsonData = System.IO.File.ReadAllText(fullPath);

                if (string.IsNullOrWhiteSpace(jsonData))
                    return new List<LookupModel>();

                response = JsonConvert.DeserializeObject<List<LookupModel>>(jsonData);
            }
            else
            {
                var result = await _customerIntegrationService.GetInstitutionTypeDocumentLookUp(institutionTypeId, false);
                response = result.Result;
            }

            return response;
        }
    }
}
