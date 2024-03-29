using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct
{
    public class UpdateProductDto
    {
        public int CustomerProductId { get; set; }
        public bool OnlyStatus { get; set; }
		public string OnboardingCode { get; set; }
        public string Remark { get; set; }
        public OnboardingProductStatus? Status { get; set; }
    }
}
