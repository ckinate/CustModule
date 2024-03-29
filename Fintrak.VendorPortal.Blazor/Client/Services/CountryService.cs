using Fintrak.VendorPortal.Blazor.Shared.Models;
using System.Net.Http.Json;

namespace Fintrak.VendorPortal.Blazor.Client.Services
{
	public class CountryService : ICountryService
	{
		private readonly HttpClient _http;

		public CountryService(HttpClient http)
		{
			_http = http;
		}

		public async Task<BaseResponse<List<LookupModel<string, string>>>> GetCountryLookup()
		{
			var response = new BaseResponse<List<LookupModel<string, string>>>();

			try
			{
				response = await _http.GetFromJsonAsync< BaseResponse<List<LookupModel<string, string>>>>($"api/Countries/lookup");
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

	public interface ICountryService
	{
		Task<BaseResponse<List<LookupModel<string, string>>>> GetCountryLookup();
	}
}
