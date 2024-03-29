namespace Fintrak.CustomerPortal.Domain.Entities
{
	public class CustomerCustomField : BaseAuditableEntity
	{
		public Customer Customer { get; set; }
		public int CustomerId { get; set; }

		public int CustomFieldId { get; set; }

		public string CustomField { get; set; } = default!;

		public bool IsCompulsory { get; set; }

		public string Response { get; set; } = default!;
	}
}
