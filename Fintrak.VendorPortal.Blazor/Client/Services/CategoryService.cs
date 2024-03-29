using Fintrak.VendorPortal.Blazor.Shared.Models;
using System.Net.Http.Json;

namespace Fintrak.VendorPortal.Blazor.Client.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly HttpClient _http;

		public CategoryService(HttpClient http)
		{
			_http = http;
		}

		public async Task<BaseResponse<List<LookupModel<string, int>>>> GetCategoryLookup()
		{
			var response = new BaseResponse<List<LookupModel<string, int>>>();

			try
			{
				response = await _http.GetFromJsonAsync< BaseResponse<List<LookupModel<string, int>>>>($"api/Categories/lookup");
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

	public interface ICategoryService
	{
		Task<BaseResponse<List<LookupModel<string, int>>>> GetCategoryLookup();
	}
}
