using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Infrastructure.Integration.Models.Docusign;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fintrak.CustomerPortal.Blazor.Server.Controllers
{
	public class DocusignController : ControllerBase
	{
		private readonly ICustomerIntegrationService _customerIntegrationService;

		public DocusignController(ICustomerIntegrationService customerIntegrationService)
        {
			_customerIntegrationService = customerIntegrationService;
        }

		[HttpGet]
		public IActionResult Index()
		{
            return Ok();
		}

		[HttpPost]
		//[IgnoreAntiforgeryToken]
		public async Task<ActionResult<SampleDocumentSignedCompletetionInfo>> Verification()  // IMPORTANT: DO NOT USE AN ARGUMENT TO THIS FUNCTION
		{
			try
			{
				using (var reader = new StreamReader(Request.Body))
				{
					var body = reader.ReadToEnd();

					var envelopeInformation = JsonConvert.DeserializeObject<DocusignWebhookCompletedModel>(body);

					var documentSignedCompletetionInfo = new SampleDocumentSignedCompletetionInfo
					{
						//Email = envelopeInformation.EnvelopeStatus.RecipientStatuses[0].RecipientStatus.Email,
						//DocumentId = envelopeInformation.EnvelopeStatus.EnvelopeID,
						//Status = envelopeInformation.EnvelopeStatus.Status,
						//TimeSigned = envelopeInformation.EnvelopeStatus.Completed,
					};

					//back office portal service
					await _customerIntegrationService.ProcessDocusignRequest(envelopeInformation);

					return documentSignedCompletetionInfo;
				}
			}
			catch (Exception ex)
			{
				throw;
			}		
		}


	}
}
