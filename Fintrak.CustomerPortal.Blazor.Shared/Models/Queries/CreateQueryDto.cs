using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Queries
{
	public class CreateQueryDto
	{
		public ResourceType ResourceType { get; set; }
		public string? ResourceReference { get; set; }
		public string? QueryMessage { get; set; }
		public string? QueryInitiator { get; set; }
		public string? Hash { get; set; }
	}
}
