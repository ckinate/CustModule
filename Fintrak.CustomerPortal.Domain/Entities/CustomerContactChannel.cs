using Fintrak.CustomerPortal.Domain.Enums;

namespace Fintrak.CustomerPortal.Domain.Entities
{
	public class CustomerContactChannel : BaseAuditableEntity
	{
		public int? CustomerId { get; set; }
		public Customer? Customer { get; set; }

		//Email, Phone
		public ChannelType ChannelType { get; set; }
		public string? Email { get; set; }

		public string? MobilePhoneCallCode { get; set; }
		public string? MobilePhoneNumber { get; set; }
	}
}
