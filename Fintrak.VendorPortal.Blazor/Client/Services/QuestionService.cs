using Fintrak.VendorPortal.Blazor.Shared.Models;
using Fintrak.VendorPortal.Blazor.Shared.Models.Onboarding;
using System.Net.Http.Json;

namespace Fintrak.VendorPortal.Blazor.Client.Services
{
	public class QuestionService : IQuestionService
	{
		private readonly HttpClient _http;

		public QuestionService(HttpClient http)
		{
			_http = http;
		}

		public async Task<BaseResponse<List<QuestionDto>>> GetQuestions()
		{
			var response = new BaseResponse<List<QuestionDto>>();

			try
			{
				response.Result = await _http.GetFromJsonAsync<List<QuestionDto>>($"api/Questionnaires");
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

	public interface IQuestionService
	{
		Task<BaseResponse<List<QuestionDto>>> GetQuestions();
	}
}
