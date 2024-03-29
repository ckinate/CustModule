using Fintrak.CustomerPortal.Blazor.Shared.Models;
using System.Net.Http.Json;

namespace Fintrak.CustomerPortal.Blazor.Client.Services
{
	public class IndustryService : IIndustryService
	{
		private readonly HttpClient _http;

		public IndustryService(HttpClient http)
		{
			_http = http;
		}

		public async Task<BaseResponse<List<LookupModel<string, int>>>> GetIndustryLookup(int? setorId)
		{
			var response = new BaseResponse<List<LookupModel<string, int>>>();

			try
			{
				response = await _http.GetFromJsonAsync< BaseResponse<List<LookupModel<string, int>>>>($"api/Industries/lookup?setorId={setorId}");
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

	public interface IIndustryService
	{
		Task<BaseResponse<List<LookupModel<string, int>>>> GetIndustryLookup(int? categoryId);
	}
}
