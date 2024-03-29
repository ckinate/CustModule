using Fintrak.VendorPortal.Blazor.Shared.Models;
using System.Net.Http.Json;

namespace Fintrak.VendorPortal.Blazor.Client.Services
{
	public class SubCategoryService : ISubCategoryService
	{
		private readonly HttpClient _http;

		public SubCategoryService(HttpClient http)
		{
			_http = http;
		}

		public async Task<BaseResponse<List<LookupModel<string, int>>>> GetSubCategoryLookup(int? categoryId)
		{
			var response = new BaseResponse<List<LookupModel<string, int>>>();

			try
			{
				response = await _http.GetFromJsonAsync< BaseResponse<List<LookupModel<string, int>>>>($"api/SubCategories/lookup?categoryId={categoryId}");
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

	public interface ISubCategoryService
	{
		Task<BaseResponse<List<LookupModel<string, int>>>> GetSubCategoryLookup(int? categoryId);
	}
}
