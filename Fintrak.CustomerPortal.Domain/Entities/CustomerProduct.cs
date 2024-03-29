using Fintrak.CustomerPortal.Domain.Enums;

namespace Fintrak.CustomerPortal.Domain.Entities
{
	public class CustomerProduct : BaseAuditableEntity
	{
		public Customer Customer { get; set; }
		public int CustomerId { get; set; }

		public int ProductId { get; set; }
		public string ProductName { get; set; } = default!;
		public string ProductCode { get; set; } = default!;

		public CustomerContactPerson CustomerContactPerson { get; set; }
		public int CustomerContactPersonId { get; set; }

		public OperationMode OperationMode { get; set; }

		public CustomerAccount CustomerAccount { get; set; }
		public int CustomerAccountId { get; set; }

		public string Reason { get; set; } = default!;

		public virtual string Website { get; set; } = default!;

        public virtual string Remark { get; set; } = default!;
        public virtual string CustomerCode { get; set; } = default!;
        public virtual string CustomerMis { get; set; } = default!;

        //Draft,Submitted,Completed
        public OnboardingProductStatus Status { get; set; }

		public IList<CustomerProductCustomField> CustomerProductCustomFields { get; private set; } = new List<CustomerProductCustomField>();
		public IList<CustomerProductDocument> CustomerProductDocuments { get; private set; } = new List<CustomerProductDocument>();
	}
}
