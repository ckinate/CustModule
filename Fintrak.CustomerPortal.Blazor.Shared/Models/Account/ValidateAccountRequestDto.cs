namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Account
{
	public class ValidateAccountRequestDto
	{
		public string BankCode { get; set; }
		public string AccountName { get; set; }
		public string AccountNumber { get; set; }
	}

	public class ValidateAccountResponseDto
	{
		public string BankCode { get; set; }
		public string AccountName { get; set; }
		public string AccountNumber { get; set; }

		public string Message { get; set; }
		public bool Valid { get; set; }
	}
}
