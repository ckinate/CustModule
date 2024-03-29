namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Enums
{
	public enum InvoiceStatus
	{
		Draft = 1,
		Posted = 2,
		Canceled = 3
	}

	public enum InvoicePaymentStatus
	{
		NotPaid = 1,
		PartialPaid = 2,
		FullyPaid = 3
	}
}
