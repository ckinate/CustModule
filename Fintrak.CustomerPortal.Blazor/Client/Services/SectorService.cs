using Fintrak.CustomerPortal.Blazor.Shared.Models;
using System.Net.Http.Json;

namespace Fintrak.CustomerPortal.Blazor.Client.Services
{
	public class SectorService : ISectorService
	{
		private readonly HttpClient _http;

		public SectorService(HttpClient http)
		{
			_http = http;
		}

		public async Task<BaseResponse<List<LookupModel<string, int>>>> GetSectorLookup()
		{
			var response = new BaseResponse<List<LookupModel<string, int>>>();

			try
			{
				response = await _http.GetFromJsonAsync< BaseResponse<List<LookupModel<string, int>>>>($"api/Sectors/lookup");
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

	public interface ISectorService
	{
		Task<BaseResponse<List<LookupModel<string, int>>>> GetSectorLookup();
	}
}
