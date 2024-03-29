using Fintrak.VendorPortal.Blazor.Shared.Models;
using Fintrak.VendorPortal.Blazor.Shared.Models.Onboarding;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace Fintrak.VendorPortal.Blazor.Client.Services
{
	public class BankService : IBankService
	{
		private readonly HttpClient _http;

		public BankService(HttpClient http)
		{
			_http = http;
		}

		public async Task<BaseResponse<List<LookupModel<string, string>>>> GetBankLookup()
		{
			var response = new BaseResponse<List<LookupModel<string, string>>>();

			try
			{
				response = await _http.GetFromJsonAsync< BaseResponse<List<LookupModel<string, string>>>>($"api/Banks/lookup");
			}
			catch (Exception exception)
			{
				response.Success = false;
				response.Message = exception.Message;
				//exception.Redirect();
			}

			return response;
		}

		public async Task<BaseResponse<ValidateAccountResponseDto>> ValidateAccount(string bankCode, string accountName, string accountNumber)
		{
			var response = new BaseResponse<ValidateAccountResponseDto>();

			try
			{
				var request = new ValidateAccountRequestDto
				{
					BankCode = bankCode,
					AccountName = accountName,	
					AccountNumber= accountNumber
				};

				var requestJson = JsonSerializer.Serialize(request);
				var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

				var result = await _http.PostAsync($"api/Banks/validateaccount", requestContent);
				result.EnsureSuccessStatusCode();

				response = await result.Content.ReadFromJsonAsync<BaseResponse<ValidateAccountResponseDto>>();
				//response = JsonSerializer.Deserialize<BaseResponse<ValidateAccountResponseDto>>(content);
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

	public interface IBankService
	{
		Task<BaseResponse<List<LookupModel<string, string>>>> GetBankLookup();

		Task<BaseResponse<ValidateAccountResponseDto>> ValidateAccount(string bankCode, string accountName, string accountNumber);
	}
}
