using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Invitations;
using System.Net.Http.Json;

namespace Fintrak.CustomerPortal.Blazor.Client.Invitations
{
	public class InvitationService : IInvitationService
	{
		private readonly HttpClient _http;

		public InvitationService(HttpClient http)
		{
			_http = http;
		}

		public async Task<BaseResponse<InvitationDto>> GetInvitationByCode(string code)
		{
			var response = new BaseResponse<InvitationDto>();

			try
			{
				response = await _http.GetFromJsonAsync<BaseResponse<InvitationDto>>($"api/Invitations/{code}");
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

	public interface IInvitationService
	{
		Task<BaseResponse<InvitationDto>> GetInvitationByCode(string code);
	}
}
