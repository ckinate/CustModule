namespace Fintrak.CustomerPortal.Domain.Entities
{
	public class CustomerProductCustomField : BaseAuditableEntity
	{
		public CustomerProduct CustomerProduct { get; set; }
		public int CustomerProductId { get; set; }

		public int CustomFieldId { get; set; }

		public string CustomField { get; set; } = default!;

		public bool IsCompulsory { get; set; }

		public string Response { get; set; } = default!;
	}
}
