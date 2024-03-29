using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using System.Net.Http.Json;

namespace Fintrak.CustomerPortal.Blazor.Client.Services
{
	public class ProductService : IProductService
	{
		private readonly HttpClient _http;

		public ProductService(HttpClient http)
		{
			_http = http;
		}

		public async Task<BaseResponse<List<LookupModel<string, int>>>> GetProductLookup(string customerCode)
		{
			var response = new BaseResponse<List<LookupModel<string, int>>>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<List<LookupModel<string, int>>>>($"api/Products/lookup?customerCode={customerCode}");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<List<CustomFieldDto>>> GetProductCustomFields(int productId)
		{
			var response = new BaseResponse<List<CustomFieldDto>>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<List<CustomFieldDto>>>($"api/Products/CustomFields?productId={productId}");
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

	public interface IProductService
	{
		Task<BaseResponse<List<LookupModel<string, int>>>> GetProductLookup(string customerCode);

		Task<BaseResponse<List<CustomFieldDto>>> GetProductCustomFields(int productId);
	}
}
