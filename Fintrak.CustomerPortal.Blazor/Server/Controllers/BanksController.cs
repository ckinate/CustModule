using Microsoft.AspNetCore.Mvc;
using Fintrak.CustomerPortal.Blazor.Server.Models;
using Newtonsoft.Json;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Infrastructure.Integration.Services;

namespace Fintrak.CustomerPortal.Blazor.Server.Controllers
{
	public class BanksController : ApiControllerBase
	{
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IConfiguration _configuration;
		private readonly IBankIntegrationService _bankIntegrationService;

		public BanksController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IBankIntegrationService bankIntegrationService)
		{
			_webHostEnvironment = webHostEnvironment;
			_configuration = configuration;
			_bankIntegrationService = bankIntegrationService;
		}

		[HttpGet()]
		public async Task<ActionResult<List<BankModel>>> Get()
		{
			return await GetBanks();
		}

		[HttpGet("lookup")]
		public async Task<ActionResult<BaseResponse<List<LookupModel<string, string>>>>> GetLookup()
		{
			var response = new BaseResponse<List<LookupModel<string, string>>> { Result = new List<LookupModel<string, string>>() };

			var entities = await GetBanks();

			foreach (var entity in entities)
			{
				response.Result.Add(new LookupModel<string, string>
				{
					Text = entity.Name,
					Value = entity.Name,
					AlternateText = entity.Code,
					AlternateText2 = $"{entity.Name} - {entity.Code}"
				});
			}

			return response;
		}

		[HttpPost("validateaccount")]
		public async Task<ActionResult<BaseResponse<ValidateAccountResponseDto>>> ValidateAccount(ValidateAccountRequestDto validateAccountRequest)
		{
			var response = new BaseResponse<ValidateAccountResponseDto>
			{
				Result = new ValidateAccountResponseDto()
			};

			var useOfflineData = bool.Parse(_configuration["UseOfflineData"].ToString());

			if (useOfflineData)
			{
				response.Result.BankCode = validateAccountRequest.BankCode;
				response.Result.AccountName = validateAccountRequest.AccountName;
				response.Result.AccountNumber = validateAccountRequest.AccountNumber;
				response.Result.Valid = true;
				response.Result.Message = $"Validation successful for Account: {validateAccountRequest.AccountNumber} with Name: {validateAccountRequest.AccountName}";
			}
			else
			{
				var result = await _bankIntegrationService.ValidateAccount(validateAccountRequest.BankCode, validateAccountRequest.AccountNumber, validateAccountRequest.AccountName);
				if(result != null && result.Result != null) 
				{
					response.Result.BankCode = result.Result.BankCode;
					response.Result.AccountName = result.Result.AccountName;
					response.Result.AccountNumber = result.Result.AccountNumber;
					response.Result.Message = result.Result.Message;
					response.Result.Valid = result.Result.Valid;
				}
			}	

			return response;
		}

		private async Task<List<BankModel>> GetBanks()
		{
			var response = new List<BankModel>();

			var useOfflineData = bool.Parse(_configuration["UseOfflineData"].ToString());

			if (useOfflineData)
			{
				var rootPath = _webHostEnvironment.ContentRootPath;
				var fullPath = Path.Combine(rootPath, "JsonData/Banks.json");
				var jsonData = System.IO.File.ReadAllText(fullPath);

				if (string.IsNullOrWhiteSpace(jsonData))
					return new List<BankModel>();

				response = JsonConvert.DeserializeObject<List<BankModel>>(jsonData);
			}
			else
			{
				var banksFromIntegration = await _bankIntegrationService.GetBankList(true, false);
				if(banksFromIntegration != null && banksFromIntegration.Success)
				{
					foreach(var bank in banksFromIntegration.Result)
					{
						response.Add(new BankModel
						{
							Id = bank.Id,
							Code = bank.BankCode,
							Name = bank.BankName
						});
					}
				}
			}

			return response;
		}
	}
}
