namespace Fintrak.CustomerPortal.Domain.Entities
{
	public class CustomerSignatory : BaseAuditableEntity
	{
		public int? CustomerId { get; set; }
		public Customer? Customer { get; set; }

		public string? FirstName { get; set; }
		public string? MiddleName { get; set; }
		public string? LastName { get; set; }

		public string? Name { get; set; }

		public string? Email { get; set; }

        public string? MobileNumberCallCode { get; set; }
        public string? MobileNumber { get; set; }

        public string? Designation { get; set; }
    }
}
