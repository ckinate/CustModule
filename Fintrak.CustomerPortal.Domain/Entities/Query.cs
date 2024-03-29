using Fintrak.CustomerPortal.Domain.Enums;


namespace Fintrak.CustomerPortal.Domain.Entities
{
	public class Query : BaseAuditableEntity
	{
		public ResourceType ResourceType { get; set; }
		public string? ResourceReference { get; set; }
		public string? QueryMessage { get; set; }
		public string? QueryInitiator { get; set; }
		public DateTime EntryDate { get; set; }
		public int CustomerId { get; set; }
		public Customer Customer { get; set; } = default!;
		public string? QueryResponse { get; set; }
		public bool IsPending { get; set; }
		public DateTime? ResponseDate { get; set; }

		public OnboardingStatus PreviousStatus { get; set; }
		public bool RequireDataModification { get; set; }
	}
}
