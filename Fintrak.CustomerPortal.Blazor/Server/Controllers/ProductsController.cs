using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using Fintrak.CustomerPortal.Application.Common.Interfaces;

namespace Fintrak.CustomerPortal.Blazor.Server.Controllers
{
	public class ProductsController : ApiControllerBase
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IConfiguration _configuration;
		private readonly ICustomerIntegrationService _customerIntegrationService;

		public ProductsController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, ICustomerIntegrationService customerIntegrationService)
		{
			_webHostEnvironment = webHostEnvironment;
			_configuration = configuration;
			_customerIntegrationService = customerIntegrationService;
		}

        [HttpGet("lookup")]
        public async Task<ActionResult<BaseResponse<List<LookupModel<string, int>>>>> GetLookup([FromQuery]string customerCode)
        {

            var response = new BaseResponse<List<LookupModel<string, int>>> { Result = new List<LookupModel<string, int>>() };

            var entities = await GetProducts(customerCode);

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

        [HttpGet("documents")]
        public async Task<ActionResult<BaseResponse<List<LookupModel<string, int>>>>> GetDocuments([FromQuery] int productId, [FromQuery] string customerCode)
        {

            var response = new BaseResponse<List<LookupModel<string, int>>> { Result = new List<LookupModel<string, int>>() };

            var entities = await GetProductDocuments(productId, customerCode);

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

		[HttpGet("customfields")]
		public async Task<ActionResult<BaseResponse<List<CustomFieldDto>>>> GetCustomFields([FromQuery] int productId)
		{

			var response = new BaseResponse<List<CustomFieldDto>> { Result = new List<CustomFieldDto>() };

			var entities = await GetProductCustomFields(productId);

			foreach (var entity in entities)
			{
				response.Result.Add(new CustomFieldDto
				{
                    CustomFieldId = entity.CustomFieldId,
                    CustomField = entity.CustomField,
                    IsCompulsory = entity.IsCompulsory,
                    Response = entity.Response,				
				});
			}

			return response;
		}

		private async Task<List<LookupModel>> GetProducts(string customerCode)
        {
            var response = new List<LookupModel>();

            var useOfflineData = bool.Parse(_configuration["UseOfflineData"].ToString());
            if (useOfflineData)
            {
                var rootPath = _webHostEnvironment.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "JsonData/Products.json");
                var jsonData = System.IO.File.ReadAllText(fullPath);

                if (string.IsNullOrWhiteSpace(jsonData))
                    return new List<LookupModel>();

                response = JsonConvert.DeserializeObject<List<LookupModel>>(jsonData);
            }
            else
            {
                var result = await _customerIntegrationService.GetProductLookUp(customerCode, false);
                response = result.Result;
            }

            return response;
        }

        private async Task<List<LookupModel>> GetProductDocuments(int productId, string customerCode)
        {
            var response = new List<LookupModel>();

            var useOfflineData = bool.Parse(_configuration["UseOfflineData"].ToString());
            if (useOfflineData)
            {
                var rootPath = _webHostEnvironment.ContentRootPath;
                var fullPath = Path.Combine(rootPath, "JsonData/ProducteDocuments.json");
                var jsonData = System.IO.File.ReadAllText(fullPath);

                if (string.IsNullOrWhiteSpace(jsonData))
                    return new List<LookupModel>();

                response = JsonConvert.DeserializeObject<List<LookupModel>>(jsonData);
            }
            else
            {
                var result = await _customerIntegrationService.GetProductDocumentLookUp(productId, customerCode, false);
                response = result.Result;
            }

            return response;
        }

		private async Task<List<CustomFieldDto>> GetProductCustomFields(int productId)
		{
			var response = new List<CustomFieldDto>();

			var useOfflineData = bool.Parse(_configuration["UseOfflineData"].ToString());
			if (useOfflineData)
			{
                return response;
			}
			else
			{
				var result = await _customerIntegrationService.GetProductCustomFields(productId);
				response = result.Result;
			}

			return response;
		}
	}
}
