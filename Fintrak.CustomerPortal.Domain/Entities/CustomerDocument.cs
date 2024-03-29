using Fintrak.CustomerPortal.Domain.Enums;

namespace Fintrak.CustomerPortal.Domain.Entities
{
	public class CustomerDocument : BaseAuditableEntity
	{
		public int? CustomerId { get; set; }
		public Customer? Customer { get; set; }

        public int DocumentTypeId { get; set; }

        public string? Title { get; set; }

		public virtual bool HasIssueDate { get; set; }

		public virtual DateTime? IssueDate { get; set; }

        public virtual bool HasExpiryDate { get; set; }

        public virtual DateTime? ExpiryDate { get; set; }

        public string? Location { get; set; }

        public virtual DocumentSource Source { get; set; }

        public bool ViewableByCustomer { get; set; }

        public bool RequireCustomerSignature { get; set; }

        public bool CustomerHaveSigned { get; set; }

        public bool Completed { get; set; }

        public string? Remark { get; set; }
    }
}
