namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Billings
{
	public class PortalInvoicePaymentReceiptDto
	{
		public string InvoiceCode { get; set; }

		public decimal AmountPaid { get; set; }

		public string ReceiptNo { get; set; }

		public virtual byte[] FileData { get; set; }
		public virtual string FileExtensionType { get; set; }
		public long FileSize { get; set; }
        public string SelectedFileName { get; set; }

        public bool ExcludeWitholdingTax { get; set; }
		public decimal? WitholdingTaxValue { get; set; }
		public bool ExcludeVat { get; set; }
		public decimal? VatValue { get; set; }
	}
}
