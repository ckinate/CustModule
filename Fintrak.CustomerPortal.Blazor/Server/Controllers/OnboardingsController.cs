using Azure;
using Fintrak.CustomerPortal.Application.Billing.Commands;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Onboarding.Commands;
using Fintrak.CustomerPortal.Application.Onboarding.Queries;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using Fintrak.CustomerPortal.Infrastructure.Integration.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fintrak.CustomerPortal.Blazor.Server.Controllers
{
    [Authorize]
	public class OnboardingsController : ApiControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly ICustomerIntegrationService _customerIntegrationService;

		public OnboardingsController(IConfiguration configuration, ICustomerIntegrationService customerIntegrationService)
		{
			_configuration = configuration;
			_customerIntegrationService = customerIntegrationService;
		}

		[HttpGet("MyDashboard")]
		public async Task<ActionResult<BaseResponse<DashboardDto>>> MyDashboard()
		{
			var result = await Mediator.Send(new GetDashboardQuery());

			return result;
		}

		[HttpGet("GetStatus")]
		public async Task<ActionResult<BaseResponse<OnboardingStatusDto>>> GetOnboardingStatus()
		{
			var result = await Mediator.Send(new GetOnboardingStateQuery());
			return result;
		}

		[AllowAnonymous]
        [HttpGet("GetCustomers")]
        public async Task<ActionResult<BaseResponse<PaginatedList<OnboardCustomerDto>>>> GetCustomers([FromQuery] GetCustomersQuery query)
        {
			var result = await Mediator.Send(query);
			return result;
		}

        [AllowAnonymous]
        [HttpGet("ExportCustomers")]
        public async Task<ActionResult> Export([FromQuery] ExportCustomersQuery exportCustomersQuery)
        {
            var result = await Mediator.Send(exportCustomersQuery);

            return File(result.Content, result.ContentType, result.FileName);
        }

        [AllowAnonymous]
		[HttpGet("GetCustomer/{id}")]
		public async Task<ActionResult<BaseResponse<OnboardCustomerDto>>> GetCustomer(int id, [FromQuery] string hash )
		{
			var result = await Mediator.Send(new GetCustomerDetailByIdQuery(id, hash));
			return result;
		}

		[HttpGet("GetCustomerDetails")]
		public async Task<ActionResult<BaseResponse<OnboardCustomerDto>>> GetCustomerDetails()
		{
			var result = await Mediator.Send(new GetCustomerDetailQuery());
			return result;
		}

		[HttpGet("GetCurrentCustomer")]
		public async Task<ActionResult<BaseResponse<BasicCustomerDto>>> GetCustomer()
		{
			var result = await Mediator.Send(new GetBasicCustomerQuery());
			return result;
		}

		[HttpPost("Validate")]
		public async Task<ActionResult<BaseResponse<bool>>> Validate(OnboardCustomerDto item, int currentStep)
		{
			var command = new OnboardCustomerValidationCommand { Item = item, CurrentStep = currentStep };
			return await Mediator.Send(command);
		}

		[HttpPost]
		public async Task<ActionResult<BaseResponse<int>>> Create(OnboardCustomerDto item)
		{
			var notificationEmail = "";

			var notificationEmailResponse = await _customerIntegrationService.GetCustomerNotificationEmail();
			if(notificationEmailResponse != null && notificationEmailResponse.Success) 
			{
				notificationEmail = notificationEmailResponse.Result;
			}

			var command = new OnboardCustomerCommand { Item = item, NotificationEmail = notificationEmail };
			return await Mediator.Send(command);
		}

        [HttpPost("Save")]
        public async Task<ActionResult<BaseResponse<int>>> Save(OnboardCustomerDto item, int currentStep)
        {
            var notificationEmail = "";

            var notificationEmailResponse = await _customerIntegrationService.GetCustomerNotificationEmail();
            if (notificationEmailResponse != null && notificationEmailResponse.Success)
            {
                notificationEmail = notificationEmailResponse.Result;
            }

            var command = new SaveOnboardCustomerCommand { Item = item, CurrentStep = currentStep, NotificationEmail = notificationEmail };
            return await Mediator.Send(command);
        }

        [AllowAnonymous]
		[HttpPost("UpdateCustomerState")]
		public async Task<ActionResult<BaseResponse<bool>>> UpdateCustomerState(ChangeCustomerStateDto item)
		{
			var notificationEmail = "";

			var notificationEmailResponse = await _customerIntegrationService.GetCustomerNotificationEmail();
			if (notificationEmailResponse != null && notificationEmailResponse.Success)
			{
				notificationEmail = notificationEmailResponse.Result;
			}

			var command = new UpdateCustomerStateCommand { Item = item, NotificationEmail = notificationEmail };
			return await Mediator.Send(command);
		}

		[AllowAnonymous]
		[HttpPost("UpdateCustomer")]
		public async Task<ActionResult<BaseResponse<bool>>> UpdateCustomer(UpdateCustomerDto item)
		{
			var command = new UpdateCustomerCommand { Item = item};
			return await Mediator.Send(command);
		}

		[AllowAnonymous]
		[HttpPost("UpdateDueDeligence")]
		public async Task<ActionResult<BaseResponse<bool>>> UpdateDueDeligence(UpdateDueDeligenceDto item)
		{
			var command = new UpdateDueDeligenceCommand { Item = item };
			return await Mediator.Send(command);
		}

		[HttpGet("GetTinValidationPatterns")]
		public ActionResult<BaseResponse<List<string>>> GetTinValidationPatterns()
		{
			var response = new BaseResponse<List<string>>(); ;

			var tinPatterns = _configuration["TinValidation:Patterns"];
			if (!string.IsNullOrEmpty(tinPatterns))
			{
				var cleanTinPatterns = tinPatterns.Replace(@"\\",@"\");

				response.Result = cleanTinPatterns.Split(',').ToList();
			}

			return response;
		}

		[HttpGet("CustomerContactLookup")]
		public async Task<ActionResult<BaseResponse<List<LookupModel<string, int>>>>> GetCustomerContacts()
		{
			var result = new BaseResponse<List<LookupModel<string, int>>> { Result = new() };

			var response = await Mediator.Send(new GetCustomerContactsQuery());
			if(response != null && response.Result != null) 
			{
				foreach(var item in response.Result)
				{
					var data = new LookupModel<string, int>
					{
						Text = item.Name,
						Value = item.Id.Value,
						AlternateText = item.Name,
						AlternateText2 = item.Name,
						HasAdditionalData = false,
						AdditionalData = null,
					};

					result.Result.Add(data);
				}
			}

			return result;
		}

		[HttpGet("ParentLookup")]
		public async Task<ActionResult<BaseResponse<List<LookupModel>>>> GetParents()
		{
			var result = new BaseResponse<List<LookupModel>> { Result = new() };

			result = await Mediator.Send(new GetParentsQuery());
			
			return result;
		}

		[HttpGet("CustomerAccountLookup")]
		public async Task<ActionResult<BaseResponse<List<LookupModel<string, int>>>>> GetCustomerAccounts([FromQuery]AccountType? accountType)
		{
			var result = new BaseResponse<List<LookupModel<string, int>>> { Result = new() };

			var response = await Mediator.Send(new GetCustomerAccountsQuery(accountType));
			if (response != null && response.Result != null)
			{
				foreach (var item in response.Result)
				{
					var data = new LookupModel<string, int>
					{
						Text = $"{item.AccountName} - {item.AccountNumber}",
						Value = item.Id.Value,
						AlternateText = $"{item.AccountNumber} - {item.AccountName}",
						AlternateText2 = $"{item.AccountName} - {item.AccountNumber} - {item.BankCode}",
						HasAdditionalData = true,
					};

					data.AdditionalData.Add("BankCode", item.BankCode);
					data.AdditionalData.Add("BankName", item.BankName);
					data.AdditionalData.Add("AccountName", item.AccountName);
					data.AdditionalData.Add("AccountNumber", item.AccountNumber);

					result.Result.Add(data);
				}
			}
			
			return result;
		}

        [HttpGet("GetCustomerCustomFields")]
        public async Task<ActionResult<BaseResponse<List<CustomFieldResponseDto>>>> GetCustomerCustomFields()
        {
            var result = new BaseResponse<List<CustomFieldResponseDto>> { Result = new() };

            result = await Mediator.Send(new GetCustomerCustomFieldsQuery { });

            return result;
        }

		[HttpGet("LegalDocuments")]
		public async Task<ActionResult<BaseResponse<List<CustomerOnboardingDocumentDto>>>> GetLegalDocuments()
		{
			var result = await Mediator.Send(new GetMyLegalDocumentsQuery());
			return result;
		}

		[HttpPost("SubmitLegalDocuments")]
		public async Task<ActionResult<BaseResponse<bool>>> SubmitLegalDocuments(SignedDocuments signedDocuments)
		{
			var result = await Mediator.Send(new SubmitLegalDocumentsCommand { Item = signedDocuments.Documents });
			return result;
		}

		[HttpPost("SubmitPaymentReceipt")]
		public async Task<ActionResult<BaseResponse<bool>>> SubmitPaymentReceipt(PortalInvoicePaymentReceiptDto paymentReceiptDocument)
		{
			var result = await Mediator.Send(new SubmitPaymentReceiptCommand { Item = paymentReceiptDocument });
			return result;
		}
	}
}
