using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding
{
	public class UpdateCustomerDto
	{
        public int CustomerId { get; set; }
		public string? CustomerCode { get; set; } = default!;
		public bool OnlyStatus { get; set; }
		public CustomerStage? Stage { get; set; }
		public OnboardingStatus? Status { get; set; }
	}
}
