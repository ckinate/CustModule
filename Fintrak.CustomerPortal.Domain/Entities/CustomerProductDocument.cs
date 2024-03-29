namespace Fintrak.CustomerPortal.Domain.Entities
{
	public class CustomerProductDocument : BaseAuditableEntity
	{
		public CustomerProduct CustomerProduct { get; set; }
		public int CustomerProductId { get; set; }

		public int? DocumentTypeId { get; set; }
		public string DocumentTypeName { get; set; } = default!;

		public string Title { get; set; } = default!;

		public string LocationUrl { get; set; } = default!;

		public DateTime? IssueDate { get; set; }

		public bool HasExpiryDate { get; set; }

		public DateTime? ExpiryDate { get; set; }
	}
}
