using Fintrak.CustomerPortal.Application.Users.Commands;
using Fintrak.CustomerPortal.Application.Users.Queries;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintrak.CustomerPortal.Blazor.Server.Controllers
{
	[Authorize]
	public class UserController : ApiControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<BaseResponse<UserDto>>> Get()
		{
			return await Mediator.Send(new GetUserDetailQuery());
		}

		[HttpPost("changepassword")]
		public async Task<ActionResult<BaseResponse<bool>>> ChangePassword(ChangePasswordDto item)
		{
			var command = new ChangePasswordCommand { Item = item };
			return await Mediator.Send(command);
		}

		[AllowAnonymous]
		[HttpPost("lockuser")]
		public async Task<ActionResult<BaseResponse<bool>>> LockUser(LockUserDto item)
		{
			var command = new LockUserCommand { Item = item };
			return await Mediator.Send(command);
		}

        [HttpPost("acceptterms")]
        public async Task<ActionResult<BaseResponse<bool>>> AcceptTerms(AcceptTermsDto acceptTerms)
        {
            var command = new AcceptTermsCommand {};
            return await Mediator.Send(command);
        }

        //[AllowAnonymous]
        //[HttpPost("changeadmin")]
        //public async Task<ActionResult<BaseResponse<bool>>> ChangeAdmin(ChangeAdminDto item)
        //{
        //    var command = new ChangeAdminCommand { Item = item };
        //    return await Mediator.Send(command);
        //}
    }
}
