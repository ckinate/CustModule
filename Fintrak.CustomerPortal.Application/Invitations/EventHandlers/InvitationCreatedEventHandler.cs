using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Domain.Events.Invitations;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Fintrak.CustomerPortal.Application.Invitations.EventHandlers
{
	public class InvitationCreatedEventHandler : INotificationHandler<InvitationCreatedEvent>
	{
		private readonly ILogger<InvitationCreatedEventHandler> _logger;
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IIdentityService _identityService;
		private readonly IEmailService _emailService;
		private readonly IConfiguration _configuration;

		public InvitationCreatedEventHandler(ILogger<InvitationCreatedEventHandler> logger,
			IApplicationDbContext context,
			ICurrentUserService currentUserService,
			IIdentityService identityService,
			IEmailService emailService, IConfiguration configuration)
		{
			_logger = logger;
			_context = context;
			_currentUserService = currentUserService;
			_identityService = identityService;
			_emailService = emailService;
			_configuration = configuration;
		}

		public async Task Handle(InvitationCreatedEvent notification, CancellationToken cancellationToken)
		{
			try
			{
				//Template placeholder
				//[[PreHeaderText]], [[CompanyName]], [[CallackLink]], [[FooterInfo]]

				//https://localhost:7266/Identity/Account/Register?invitationCode=TestCode&returnUrl=
				var portalBaseUrl = _configuration["PortalUrl"];
				var callBackUrl = $"{portalBaseUrl}/Identity/Account/Register?invitationCode={notification.Item.Code}&returnUrl=";
				BodyBuilder template = _emailService.GetEmailTemplateBody("customer-invitation");
				//var body = string.Format(template.HtmlBody, notification.Item.CompanyName, notification.Item.AdminName, notification.Item.AdminEmail, callBackUrl);

				var body = template.HtmlBody.Replace("[[PreHeaderText]]", "");
                body = body.Replace("[[Salutation]]", $"Dear {notification.Item.AdminName}");
                body = body.Replace("[[CompanyName]]", notification.Item.CompanyName);
				body = body.Replace("[[AdminName]]", notification.Item.AdminName);
				body = body.Replace("[[AdminEmail]]", notification.Item.AdminEmail);
				body = body.Replace("[[CallackLink]]", callBackUrl);
				body = body.Replace("[[FooterInfo]]", "NIBSS, Plot 1230, Ahmadu Bello Way, Bar Beach, Victoria Island, P. M. B. 12617, Lagos.");

				var mailSubject = "NIBSS Customer Invitation Mail";

				await _emailService.SendEmailAsync(notification.Item.AdminName, notification.Item.AdminEmail, mailSubject, body);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unable to send invitation email.", null);
			}

			////Check invitation
			//if (notification.IsNew)
			//{
			//	var invitationCode = await _identityService.GetInvitationCodeAsync(_currentUserService.UserId);

			//	if (!string.IsNullOrEmpty(invitationCode))
			//	{
			//		var invitationEntity = await _context.Invitations.FirstOrDefaultAsync(c => c.Code == invitationCode && !c.Used);
			//		if(invitationEntity != null)
			//		{
			//			invitationEntity.Used = true;
			//		}
			//	}

			//}

			//return Task.CompletedTask;
		}
	}
}
