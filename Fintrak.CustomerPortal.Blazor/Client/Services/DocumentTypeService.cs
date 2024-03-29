using Fintrak.CustomerPortal.Blazor.Shared.Models;
using System.Net.Http.Json;

namespace Fintrak.CustomerPortal.Blazor.Client.Services
{
	public class DocumentTypeService : IDocumentTypeService
	{
		private readonly HttpClient _http;

		public DocumentTypeService(HttpClient http)
		{
			_http = http;
		}

		public async Task<BaseResponse<List<LookupModel<string, int>>>> GetDocumentTypeLookup()
		{
			var response = new BaseResponse<List<LookupModel<string, int>>>();

			try
			{
				response = await _http.GetFromJsonAsync< BaseResponse<List<LookupModel<string, int>>>>($"api/DocumentTypes/lookup");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<List<LookupModel<string, int>>>> GetDocumentTypeLookupByInstitutionType(int institutionTypeId)
		{
			var response = new BaseResponse<List<LookupModel<string, int>>>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<List<LookupModel<string, int>>>>($"api/DocumentTypes/institutiondocuments?institutionTypeId={institutionTypeId}");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<List<LookupModel<string, int>>>> GetDocumentTypeLookupByProduct(int productId, string customerCode)
		{
			var response = new BaseResponse<List<LookupModel<string, int>>>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<List<LookupModel<string, int>>>>($"api/Products/documents?productId={productId}&customerCode={customerCode}");
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

	public interface IDocumentTypeService
	{
		Task<BaseResponse<List<LookupModel<string, int>>>> GetDocumentTypeLookup();

		Task<BaseResponse<List<LookupModel<string, int>>>> GetDocumentTypeLookupByInstitutionType(int institutionTypeId);

		Task<BaseResponse<List<LookupModel<string, int>>>> GetDocumentTypeLookupByProduct(int productId, string customerCode);
	}
}
