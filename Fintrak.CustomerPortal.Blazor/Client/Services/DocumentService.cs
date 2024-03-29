using Fintrak.CustomerPortal.Blazor.Shared.Models;
using System.Net.Http.Json;

namespace Fintrak.CustomerPortal.Blazor.Client.Services
{
	public class DocumentService : IDocumentService
	{
		private readonly HttpClient _http;

		public DocumentService(HttpClient http)
		{
			_http = http;
		}

		public async Task<BaseResponse<List<string>>> GetDocumentRequirements()
		{
			var response = new BaseResponse<List<string>>();

			try
			{
				response.Result = await _http.GetFromJsonAsync<List<string>>($"api/Documents/documentrequirements");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<List<string>>> GetFileTypes()
		{
			var response = new BaseResponse<List<string>>();

			try
			{
				response.Result = await _http.GetFromJsonAsync<List<string>>($"api/Documents/filetypes");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<long>> GetMaximumFileSize(int? documentTypeId)
		{
			var response = new BaseResponse<long>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<long>>($"api/DocumentTypes/documenttypesize?documentTypeId={documentTypeId}");
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

	public interface IDocumentService
	{
		Task<BaseResponse<List<string>>> GetDocumentRequirements();

		Task<BaseResponse<List<string>>> GetFileTypes();

		Task<BaseResponse<long>> GetMaximumFileSize(int? documentTypeId);
	}
}
