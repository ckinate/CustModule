using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Domain.Events.Invitations
{
	public class ReplacementInvitationCreatedEvent : BaseEvent
	{
		public ReplacementInvitationCreatedEvent(Invitation item)
		{
			Item = item;
		}

		public Invitation Item { get; }
	}
}
