using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding
{
	public class ChangeCustomerStateDto
	{
		public int CustomerId { get; set; }
		public OnboardingStatus Status { get; set; }
	}
}
