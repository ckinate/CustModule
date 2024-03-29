using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding
{
    public class OnboardingStatusDto
    {
        public OnboardingStatus Status { get; set; }
        public bool AcceptTerms { get; set; }
    }
}
