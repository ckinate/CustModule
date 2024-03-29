using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct
{
	public class ChangeProductStateDto
	{
		public int CustomerProductId { get; set; }
        public string Remark { get; set; }
        public OnboardingProductStatus Status { get; set; }
	}
}
