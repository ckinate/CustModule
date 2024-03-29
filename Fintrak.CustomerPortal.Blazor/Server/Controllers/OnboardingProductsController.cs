using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.OnboardingProduct.Commands;
using Fintrak.CustomerPortal.Application.OnboardingProduct.Queries;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintrak.CustomerPortal.Blazor.Server.Controllers
{
	[Authorize]
	public class OnboardingProductsController : ApiControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly ICustomerIntegrationService _customerIntegrationService;

		public OnboardingProductsController(IConfiguration configuration, ICustomerIntegrationService customerIntegrationService)
		{
			_configuration = configuration;
			_customerIntegrationService = customerIntegrationService;
		}

		[HttpGet("GetProductStatus")]
		public async Task<ActionResult<BaseResponse<OnboardingProductStatus>>> GetOnboardingStatus()
		{
			var result = await Mediator.Send(new GetOnboardingProductStateQuery());
			return result;
		}

		[HttpGet("GetMyProducts")]
		public async Task<ActionResult<BaseResponse<List<OnboardProductDto>>>> GetProducts([FromQuery] GetMyProductsQuery query)
		{
			var result = await Mediator.Send(query);
			return result;
		}

		[AllowAnonymous]
        [HttpGet("GetProducts")]
        public async Task<ActionResult<BaseResponse<PaginatedList<OnboardProductDto>>>> GetProducts([FromQuery] GetProductsQuery query)
        {
			var result = await Mediator.Send(query);
			return result;
		}

		[AllowAnonymous]
		[HttpGet("GetProduct/{id}")]
		public async Task<ActionResult<BaseResponse<OnboardProductDto>>> GetProduct(int id, [FromQuery] string hash )
		{
			var result = await Mediator.Send(new GetProductDetailByIdQuery(id, hash));
			return result;
		}

		[HttpGet("GetCustomerProductDetails/{id}")]
		public async Task<ActionResult<BaseResponse<OnboardProductDto>>> GetCustomerProductDetails(int id)
		{
			var result = await Mediator.Send(new GetProductDetailQuery (id ));
			return result;
		}

		[HttpPost("CreateProduct")]
		public async Task<ActionResult<BaseResponse<int>>> CreateProduct(OnboardProductDto item)
		{
			var notificationEmail = "";

			var notificationEmailResponse = await _customerIntegrationService.GetCustomerNotificationEmail();
			if(notificationEmailResponse != null && notificationEmailResponse.Success) 
			{
				notificationEmail = notificationEmailResponse.Result;
			}

			var command = new OnboardProductCommand { Item = item, NotificationEmail = notificationEmail };

			return await Mediator.Send(command);
		}

		[AllowAnonymous]
		[HttpPost("UpdateProductState")]
		public async Task<ActionResult<BaseResponse<bool>>> UpdateProductState(ChangeProductStateDto item)
		{
			var notificationEmail = "";

			var notificationEmailResponse = await _customerIntegrationService.GetCustomerNotificationEmail();
			if (notificationEmailResponse != null && notificationEmailResponse.Success)
			{
				notificationEmail = notificationEmailResponse.Result;
			}

			var command = new UpdateProductStateCommand { Item = item, NotificationEmail = notificationEmail };
			return await Mediator.Send(command);
		}

		[AllowAnonymous]
		[HttpPost("UpdateProduct")]
		public async Task<ActionResult<BaseResponse<bool>>> UpdateProduct(UpdateProductDto item)
		{
			var command = new UpdateProductCommand { Item = item};
			return await Mediator.Send(command);
		}
	}
}
