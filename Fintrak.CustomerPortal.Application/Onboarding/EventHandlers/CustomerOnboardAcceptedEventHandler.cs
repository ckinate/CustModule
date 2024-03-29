using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Domain.Events.Onboarding;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Fintrak.CustomerPortal.Application.Onboarding.EventHandlers;
public class CustomerOnboardAcceptedEventHandler : INotificationHandler<CustomerOnboardAcceptedEvent>
{
	private readonly ILogger<CustomerOnboardAcceptedEventHandler> _logger;
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IIdentityService _identityService;
	private readonly IEmailService _emailService;
	private readonly IConfiguration _configuration;

	public CustomerOnboardAcceptedEventHandler(ILogger<CustomerOnboardAcceptedEventHandler> logger, 
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

	public async Task Handle(CustomerOnboardAcceptedEvent notification, CancellationToken cancellationToken)
	{
		try
		{
			if (string.IsNullOrEmpty(notification.NotificationEmail))
				throw new Exception("Notification email not set.");

			//Template placeholder
			//[[PreHeaderText]], [[CompanyName]], [[CallackLink]], [[FooterInfo]]

			BodyBuilder template = _emailService.GetEmailTemplateBody("customer-data-acceptance");
			
			var body = template.HtmlBody.Replace("[[PreHeaderText]]", "");
            body = body.Replace("[[Salutation]]", $"Dear {notification.AdminName}");
            body = body.Replace("[[CompanyName]]", notification.Item.Name);
			body = body.Replace("[[CustomerCode]]", notification.Item.Code);
			body = body.Replace("[[FooterInfo]]", "NIBSS, Plot 1230, Ahmadu Bello Way, Bar Beach, Victoria Island, P. M. B. 12617, Lagos.");

			var mailSubject = $"Attention: {notification.Item.Name} Registration Accepted.";

			await _emailService.SendEmailAsync(notification.AdminName, notification.AdminEmail, mailSubject, body);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unable to send onboarding email.", null);
		}
	}
}

