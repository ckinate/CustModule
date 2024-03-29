using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Billings
{
	public class BillInvoiceDto
	{
		public int Id { get; set; }

		public string Reference { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public virtual string SourceDocument { get; set; }

		public virtual string SourceReference { get; set; }

		public decimal UntaxedAmount { get; set; }

		public decimal TaxAmount { get; set; }

		public decimal TotalAmount { get; set; }

		public decimal OutstandingAmount { get; set; }

		public DateTime InvoiceDate { get; set; }

		public DateTime? DueDate { get; set; }

		public string BankName { get; set; }

		public string BankCode { get; set; }

		public string AccountName { get; set; }

		public string AccountCode { get; set; }

		public InvoiceStatus Status { get; set; }

		public InvoicePaymentStatus PaymentStatus { get; set; }

        public bool AwaitingPayment { get; set; }
    }
}
