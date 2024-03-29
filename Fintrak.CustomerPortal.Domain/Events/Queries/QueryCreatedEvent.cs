using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Domain.Events.Queries
{
	public class QueryCreatedEvent : BaseEvent
	{
		public QueryCreatedEvent(Query item, string adminName, string adminemail)
		{
			Item = item;
			AdminName = adminName;
			AdminEmail = adminemail;
		}

		public Query Item { get; }
		public string AdminName { get; }
		public string AdminEmail { get; }
	}
}
