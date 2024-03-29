using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Domain.Events.Invitations
{
	public class InvitationCreatedEvent : BaseEvent
	{
		public InvitationCreatedEvent(Invitation item)
		{
			Item = item;
		}

		public Invitation Item { get; }
	}
}
