using Fintrak.CustomerPortal.Blazor.Shared.Extensions;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding
{
	public class RecentCustomerOnboardingTrackerDto
	{
		public List<CustomerOnboardingStageStatusDto> StatgeStatuses { get; set; } = new();

		public List<OnboardingTrackerDto> Trackers { get; set; } = new();
	}

	public class CustomerOnboardingStageStatusDto
	{
		public string Stage { get; set; }
		public bool IsCurrent { get; set; }
		public int Order { get; set; }
	}


	public class OnboardingTrackerDto
	{
		public virtual ResourceType ResourceType { get; set; }

		public virtual string ResourceId { get; set; }

		public virtual string Description { get; set; }

		public virtual bool HasHtml { get; set; }

		public virtual string Html { get; set; }

		public virtual string Stage { get; set; }

		public virtual string StageDisplay { get; set; }

		public virtual string NextStage { get; set; }

		public virtual string NextStageDisplay { get; set; }

		public virtual string Status { get; set; }

		public virtual string StatusDisplay { get; set; }

		public virtual DateTime Date { get; set; }

		public string RelativeDate { get; set; }
    }
}
