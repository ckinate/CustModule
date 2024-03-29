using Fintrak.CustomerPortal.Blazor.Shared.Models.Invitations;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using MediatR;
using Fintrak.InvitationPortal.Application.Invitations.Queries;
using Fintrak.InvitationPortal.Application.Invitations.Commands;

namespace Fintrak.CustomerPortal.Blazor.Server.Services
{
	public class RegisterService : IRegisterService
	{
		private readonly ISender _mediator;
		private readonly IConfiguration _configuration;

		public RegisterService(ISender mediator, IConfiguration configuration)
		{
			_mediator = mediator;
			_configuration = configuration;
		}

		public async Task<BaseResponse<InvitationDto>> GetInvitation(string code)
		{
			var result = await _mediator.Send(new GetInvitationQuery(code));
			return result;
		}

		public async Task<BaseResponse<bool>> UseInvitation(string code, string loginId)
		{
			var command = new UseInvitationCommand { Item = new UseInvitationDto {  Code = code, LoginId = loginId } };
			var result = await _mediator.Send(command);
			return result;
		}

		
	}

	public interface IRegisterService
	{
		Task<BaseResponse<InvitationDto>> GetInvitation(string code);

		Task<BaseResponse<bool>> UseInvitation(string code, string loginId);
	}
}
