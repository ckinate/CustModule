using Fintrak.CustomerPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.CustomerPortal.Domain.Events.Queries
{
	public class RespondToQueryEvent : BaseEvent
	{
		public RespondToQueryEvent(Query item, string notificationEmail)
		{
			Item = item;
			NotificationEmail = notificationEmail;
		}

		public Query Item { get; }
		public string NotificationEmail { get; set; }
	}
}
