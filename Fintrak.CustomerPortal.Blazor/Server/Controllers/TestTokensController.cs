using Microsoft.AspNetCore.Mvc;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Infrastructure.Integration.Services;
using Fintrak.CustomerPortal.Infrastructure.Integration.Models;

namespace Fintrak.CustomerPortal.Blazor.Server.Controllers
{
	public class TestTokensController : ApiControllerBase
	{
		private readonly ITokenIntegrationService _tokenIntegration;

		public TestTokensController(ITokenIntegrationService tokenIntegration)
		{
			_tokenIntegration = tokenIntegration;
		}

		[HttpPost("gettoken")]
		public async Task<ActionResult<BaseResponse<TokenModel>>> GetToken(GetTokenInput input)
		{
			var response = _tokenIntegration.GetToken(input.LoginId, input.Password);
			
			return response;
		}

	
	}
}
