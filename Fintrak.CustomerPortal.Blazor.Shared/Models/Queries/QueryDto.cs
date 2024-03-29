using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Queries
{
	public class QueryDto : BaseDto
	{
		public ResourceType ResourceType { get; set; }
		public string? ResourceReference { get; set; }
		public string? QueryMessage { get; set; }
		public string? QueryInitiator { get; set; }
		public DateTime EntryDate { get; set; }
		public string? QueryTo { get; set; }
		public string? QueryResponse { get; set; }
		public bool IsPending { get; set; }
		public DateTime? ResponseDate { get; set; }

		public string? CustomerName { get; set; }
		public string? CustomerCode { get; set; }

		public bool RequireDataModification { get; set; }
		public OnboardingStatus PreviousStatus { get; set; }
	}
}
