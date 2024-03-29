using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Domain.Events.OnboardingProduct
{
	public class ProductOnboardStatusEvent : BaseEvent
	{
		public ProductOnboardStatusEvent(CustomerProduct item, string customerName, string notificationEmail, string adminName, string adminEmail, bool isNew = false)
		{
			Item = item;
			CustomerName = customerName;
			NotificationEmail = notificationEmail;
			AdminName = adminName;
			AdminEmail = adminEmail;
		}

		public CustomerProduct Item { get; }
        public string CustomerName { get; set; }
        public string NotificationEmail { get; set; }
		public string AdminName { get; }
		public string AdminEmail { get; }
	}
}
