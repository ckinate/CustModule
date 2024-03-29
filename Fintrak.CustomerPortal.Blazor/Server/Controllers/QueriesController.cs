using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Queries.Commands;
using Fintrak.CustomerPortal.Application.Queries.Queries;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintrak.CustomerPortal.Blazor.Server.Controllers
{
	[Authorize]
	public class QueriesController : ApiControllerBase
	{
		private readonly ICustomerIntegrationService _customerIntegrationService;

		public QueriesController(ICustomerIntegrationService customerIntegrationService)
		{
			_customerIntegrationService = customerIntegrationService;
		}

		[HttpGet()]
		public async Task<ActionResult<BaseResponse<List<QueryDto>>>> Get([FromQuery] bool loadAllQueries = false)
		{
			var result = await Mediator.Send(new GetQueriesQuery(loadAllQueries));
			return result;
		}

		[AllowAnonymous]
		[HttpGet("GetQueries")]
		public async Task<ActionResult<BaseResponse<List<QueryDto>>>> Get([FromQuery] int? customerId = null,[FromQuery] bool loadAllQueries = false)
		{
			var result = await Mediator.Send(new GetCustomerQueriesQuery(customerId,loadAllQueries));
			return result;
		}

		[AllowAnonymous]
		[HttpGet("GetProductQueries")]
		public async Task<ActionResult<BaseResponse<List<QueryDto>>>> GetProductQueries([FromQuery] int? customerProductId = null, [FromQuery] bool loadAllQueries = false)
		{
			var result = await Mediator.Send(new GetCustomerProductQueriesQuery(customerProductId, loadAllQueries));
			return result;
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<ActionResult<BaseResponse<int>>> Create(CreateQueryDto item)
		{
			var command = new CreateQueryCommand { Item = item };
			return await Mediator.Send(command);
		}

		[HttpPost("ResponseToQuery")]
		public async Task<BaseResponse<bool>> ResponseToQuery(ResponseToQueryDto item)
		{
			var notificationEmail = "";

			var notificationEmailResponse = await _customerIntegrationService.GetCustomerNotificationEmail();
			if (notificationEmailResponse != null && notificationEmailResponse.Success)
			{
				notificationEmail = notificationEmailResponse.Result;
			}

			var command = new ResponseToQueryCommand { Item = item, NotificationEmail = notificationEmail };
			return await Mediator.Send(command);
		}
	}
}
