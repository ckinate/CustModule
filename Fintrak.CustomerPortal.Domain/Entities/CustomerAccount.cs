using Fintrak.CustomerPortal.Domain.Enums;

namespace Fintrak.CustomerPortal.Domain.Entities
{
	public class CustomerAccount : BaseAuditableEntity
	{
		public int? CustomerId { get; set; }
		public Customer? Customer { get; set; }

        public AccountType AccountType { get; set; }
        public string? BankName { get; set; }
		public string? BankCode { get; set; }
		public string? BankAddress { get; set; }
		public string? AccountName { get; set; }
		public string? AccountNumber { get; set; }
        public int? CountryId { get; set; }
        public string? Country { get; set; }
		public bool IsLocalAccount { get; set; }
		public bool Validated { get; set; }
	}
}
