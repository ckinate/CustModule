using Fintrak.InvitationPortal.Application.Invitations.Commands;
using Fintrak.InvitationPortal.Application.Invitations.Queries;
using Fintrak.CustomerPortal.Application.Invitations.Queries;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Invitations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintrak.CustomerPortal.Blazor.Server.Controllers
{
	[Authorize]
	public class InvitationsController : ApiControllerBase
	{
		[AllowAnonymous]
		[HttpGet()]
		public async Task<ActionResult<BaseResponse<List<InvitationDto>>>> Get([FromQuery] GetInvitationsQuery query)
		{
			var result = await Mediator.Send(query);
			return result;
		}

		[AllowAnonymous]
		[HttpGet("{code}")]
		public async Task<BaseResponse<InvitationDto>> Get(string code)
		{
			var result = await Mediator.Send(new GetInvitationQuery(code));
			return result;
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<ActionResult<BaseResponse<string>>> Create(CreateInvitationDto item)
		{
			var command = new CreateInvitationCommand { Item = item };
			return await Mediator.Send(command);
		}

		[AllowAnonymous]
		[HttpPut]
		public async Task<BaseResponse<bool>> Put(UseInvitationDto item)
		{
			var command = new UseInvitationCommand { Item = item };
			return await Mediator.Send(command);
		}

        [AllowAnonymous]
        [HttpPost("CreateReplacementInvitation")]
        public async Task<ActionResult<BaseResponse<string>>> CreateReplacementInvitation(ReplacementInvitationDto item)
        {
            var command = new ReplacementInvitationCommand { Item = item };
            return await Mediator.Send(command);
        }
    }
}
