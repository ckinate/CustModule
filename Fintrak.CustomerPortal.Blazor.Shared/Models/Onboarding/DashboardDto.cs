using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding
{
	public class DashboardDto
	{
        public OnboardingStatusDto OnboardingStatus { get; set; } = new();

		public List<OnboardProductDto> Products { get; set; } = new();

        public RecentCustomerOnboardingTrackerDto Trackers { get; set; }
    }
}
