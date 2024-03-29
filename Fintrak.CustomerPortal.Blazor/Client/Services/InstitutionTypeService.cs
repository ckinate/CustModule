using Fintrak.CustomerPortal.Blazor.Shared.Models;
using System.Net.Http.Json;

namespace Fintrak.CustomerPortal.Blazor.Client.Services
{
	public class InstitutionTypeService : IInstitutionTypeService
	{
		private readonly HttpClient _http;

		public InstitutionTypeService(HttpClient http)
		{
			_http = http;
		}

		public async Task<BaseResponse<List<LookupModel<string, int>>>> GetInstitutionTypeLookup()
		{
			var response = new BaseResponse<List<LookupModel<string, int>>>();

			try
			{
				response = await _http.GetFromJsonAsync< BaseResponse<List<LookupModel<string, int>>>>($"api/InstitutionTypes/lookup");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<List<LookupModel<string, int>>>> GetParentInstitutionTypeLookup()
		{
			var response = new BaseResponse<List<LookupModel<string, int>>>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<List<LookupModel<string, int>>>>($"api/InstitutionTypes/lookup?parentOnly=true");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<List<LookupModel<string, int>>>> GetChildrenInstitutionTypeLookup(int parentId)
		{
			var response = new BaseResponse<List<LookupModel<string, int>>>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<List<LookupModel<string, int>>>>($"api/InstitutionTypes/lookup?parentId={parentId}");
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

	public interface IInstitutionTypeService
	{		
		Task<BaseResponse<List<LookupModel<string, int>>>> GetInstitutionTypeLookup();
		Task<BaseResponse<List<LookupModel<string, int>>>> GetParentInstitutionTypeLookup();
		Task<BaseResponse<List<LookupModel<string, int>>>> GetChildrenInstitutionTypeLookup(int parentId);
	}
}
