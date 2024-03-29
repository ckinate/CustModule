using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Domain.Events.Queries;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Fintrak.CustomerPortal.Application.Queries.EventHandlers
{
	public class ResponseToQueryEventHandler : INotificationHandler<RespondToQueryEvent>
	{
		private readonly ILogger<ResponseToQueryEventHandler> _logger;
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IIdentityService _identityService;
		private readonly IEmailService _emailService;
		private readonly IConfiguration _configuration;

		public ResponseToQueryEventHandler(ILogger<ResponseToQueryEventHandler> logger,
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

		public async Task Handle(RespondToQueryEvent notification, CancellationToken cancellationToken)
		{
			try
			{
				if (string.IsNullOrEmpty(notification.NotificationEmail))
					throw new Exception("Notification email not set.");

				//Template placeholder
				//[[PreHeaderText]], [[CompanyName]], [[CallackLink]], [[FooterInfo]]

				BodyBuilder template = _emailService.GetEmailTemplateBody("customer-query-response");

				var body = template.HtmlBody.Replace("[[PreHeaderText]]", "");
				body = body.Replace("[[Salutation]]", $"Dear NIBSS");
				body = body.Replace("[[CompanyName]]", notification.Item.Customer.Name);
				body = body.Replace("[[FooterInfo]]", "NIBSS, Plot 1230, Ahmadu Bello Way, Bar Beach, Victoria Island, P. M. B. 12617, Lagos.");

				var mailSubject = $"Attention: {notification.Item.Customer.Name} Submitted Onboarding Data";

				await _emailService.SendEmailAsync("NIBSS", notification.NotificationEmail, mailSubject, body);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unable to send query response email.", null);
			}
		}
	}
}
