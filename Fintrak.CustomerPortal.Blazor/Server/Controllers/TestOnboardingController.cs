using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Microsoft.AspNetCore.Mvc;

namespace Fintrak.CustomerPortal.Blazor.Server.Controllers
{
	public class TestOnboardingController : ApiControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly ICustomerIntegrationService _customerIntegrationService;

		public TestOnboardingController( IConfiguration configuration, ICustomerIntegrationService customerIntegrationService)
		{
			_configuration = configuration;
			_customerIntegrationService = customerIntegrationService;
		}

		[HttpPost("TestCustomerAcceptance")]
		public async Task<BaseResponse<AcceptOnboadingDataResponseDto>> TestCustomerDataAcceptance(OnboardCustomerDto model)
		{
			var testMode = bool.Parse(_configuration["TestMode"].ToString());
			if (testMode)
			{
				return await _customerIntegrationService.TestCustomerDataAcceptance(model);
			}

			return null;
		}
	}
}
