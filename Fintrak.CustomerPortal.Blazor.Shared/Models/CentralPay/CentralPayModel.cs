namespace Fintrak.CustomerPortal.Blazor.Shared.Models.CentralPay
{
    public class CentralPayQueryModel
    {
        public string MerchantId { get; set; }
        public string CpayRef { get; set; }
        public string BankCode { get; set; }
        public string TransDate { get; set; }
        public string Amount { get; set; }
        public string TransactionId { get; set; }
        public string Currency { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseDesc { get; set; }

        public string Hash { get; set; }
    }
}
