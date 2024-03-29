using Fintrak.CustomerPortal.Blazor.Shared.Models;
using System.Net.Http.Json;

namespace Fintrak.CustomerPortal.Blazor.Client.Services
{
	public class CountryService : ICountryService
	{
		private readonly HttpClient _http;

		public CountryService(HttpClient http)
		{
			_http = http;
		}

		public async Task<BaseResponse<List<LookupModel<string, int>>>> GetCountryLookup()
		{
			var response = new BaseResponse<List<LookupModel<string, int>>>();

			try
			{
				response = await _http.GetFromJsonAsync< BaseResponse<List<LookupModel<string, int>>>>($"api/Countries/lookup");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<List<LookupModel<string, int>>>> GetStateLookup(int? countryId)
		{
			var response = new BaseResponse<List<LookupModel<string, int>>>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<List<LookupModel<string, int>>>>($"api/States/lookup?countryId={countryId}");
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
		Task<BaseResponse<List<LookupModel<string, int>>>> GetCountryLookup();

		Task<BaseResponse<List<LookupModel<string, int>>>> GetStateLookup(int? countryId);
	}
}
