namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Billings
{
    public class CentralPayLogDto
    {
		public string RequestId { get; set; }
		public string ResourceType { get; set; }
		public string ResourceId { get; set; }
		public string ResourceDescription { get; set; }
		public string Currency { get; set; }
		public string Amount { get; set; }
		public string PaymentMode { get; set; }
		public string MerchantId { get; set; }
		public string ResponseUrl { get; set; }
		public string BaseUrl { get; set; }
		public string Hash { get; set; }
		public DateTime EntryDate { get; set; }

		public string ResponseReference { get; set; }
		public string ResponseCode { get; set; }
		public string ResponseMessage { get; set; }
		public string ResponseDescription { get; set; }

		public DateTime? CompletionDate { get; set; }
	}
}
