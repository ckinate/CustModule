namespace Fintrak.CustomerPortal.Infrastructure.Integration.Models
{
	public class AccountValidationResultModel
	{
		public AccountValidationModel Result { get; set; }
		public bool Success { get; set; }
	}

	public class AccountValidationModel
	{
		public string BankCode { get; set; }
		public string AccountName { get; set; }
		public string AccountNumber { get; set; }

		public string Message { get; set; }
		public bool Valid { get; set; }
	}
}
