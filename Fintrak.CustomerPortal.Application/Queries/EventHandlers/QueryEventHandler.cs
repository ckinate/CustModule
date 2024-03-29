using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Domain.Events.Queries;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Fintrak.CustomerPortal.Application.Queries.EventHandlers
{
	public class QueryCreatedEventHandler : INotificationHandler<QueryCreatedEvent>
	{
		private readonly ILogger<QueryCreatedEventHandler> _logger;
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IIdentityService _identityService;
		private readonly IEmailService _emailService;
		private readonly IConfiguration _configuration;

		public QueryCreatedEventHandler(ILogger<QueryCreatedEventHandler> logger,
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

		public async Task Handle(QueryCreatedEvent notification, CancellationToken cancellationToken)
		{
			try
			{
				BodyBuilder template = _emailService.GetEmailTemplateBody("customer-query");

				var body = template.HtmlBody.Replace("[[PreHeaderText]]", "");
				body = body.Replace("[[AdminName]]", notification.AdminName);
				body = body.Replace("[[Query]]", notification.Item.QueryMessage);
				body = body.Replace("[[FooterInfo]]", "NIBSS, Plot 1230, Ahmadu Bello Way, Bar Beach, Victoria Island, P. M. B. 12617, Lagos.");

				var mailSubject = $"Attention: {notification.Item.Customer.Name} Submitted Onboarding Data";

				await _emailService.SendEmailAsync(notification.AdminName, notification.AdminEmail, mailSubject, body);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unable to send query email.", null);
			}
		}
	}
}
