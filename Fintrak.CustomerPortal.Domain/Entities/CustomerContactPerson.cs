using System.Numerics;

namespace Fintrak.CustomerPortal.Domain.Entities
{
	public class CustomerContactPerson : BaseAuditableEntity
	{
		public int? CustomerId { get; set; }
		public Customer? Customer { get; set; }

		public string? FirstName { get; set; }
		public string? MiddleName { get; set; }
		public string? LastName { get; set; }

		public string? Name { get; set; }
		public string? Email { get; set; }
		public string? MobilePhoneCallCode { get; set; }
		public string? MobilePhoneNumber { get; set; }
		public string? Designation { get; set; }
		public bool Default { get; set; }
	}
}
