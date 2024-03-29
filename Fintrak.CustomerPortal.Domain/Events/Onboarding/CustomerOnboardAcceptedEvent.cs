using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Domain.Events.Onboarding
{
	public class CustomerOnboardAcceptedEvent : BaseEvent
	{
		public CustomerOnboardAcceptedEvent(Customer item,string notificationEmail, string adminName, string adminEmail, bool isNew = false)
		{
			Item = item;
			NotificationEmail = notificationEmail;
            AdminName = adminName;
            AdminEmail = adminEmail;
        }

		public Customer Item { get; }
		public string NotificationEmail { get; set; }
        public string AdminName { get; }
        public string AdminEmail { get; }
    }

}
