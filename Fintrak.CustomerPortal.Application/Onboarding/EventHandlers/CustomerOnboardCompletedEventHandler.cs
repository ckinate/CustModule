using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Domain.Events.Onboarding;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Fintrak.CustomerPortal.Application.Onboarding.EventHandlers;
public class CustomerOnboardCompletedEventHandler : INotificationHandler<CustomerOnboardCompletedEvent>
{
	private readonly ILogger<CustomerOnboardCompletedEventHandler> _logger;
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IIdentityService _identityService;
	private readonly IEmailService _emailService;
	private readonly IConfiguration _configuration;

	public CustomerOnboardCompletedEventHandler(ILogger<CustomerOnboardCompletedEventHandler> logger, 
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

	public async Task Handle(CustomerOnboardCompletedEvent notification, CancellationToken cancellationToken)
	{
		try
		{
			if (string.IsNullOrEmpty(notification.NotificationEmail))
				throw new Exception("Notification email not set.");

			//Template placeholder
			//[[PreHeaderText]], [[CompanyName]], [[CallackLink]], [[FooterInfo]]

			BodyBuilder template = _emailService.GetEmailTemplateBody("customer-data-submission");
			
			var body = template.HtmlBody.Replace("[[PreHeaderText]]", "");
            body = body.Replace("[[Salutation]]", $"Dear NIBSS");
            body = body.Replace("[[CompanyName]]", notification.Item.Name);
			body = body.Replace("[[FooterInfo]]", "NIBSS, Plot 1230, Ahmadu Bello Way, Bar Beach, Victoria Island, P. M. B. 12617, Lagos.");

			var mailSubject = $"Attention: {notification.Item.Name} Submitted Onboarding Data";

			await _emailService.SendEmailAsync("NIBSS", notification.NotificationEmail, mailSubject, body);


            //-------------------------------
            template = _emailService.GetEmailTemplateBody("customer-data-submission2");

            body = template.HtmlBody.Replace("[[PreHeaderText]]", "");
            body = body.Replace("[[Salutation]]", $"Dear {notification.AdminName}");
            body = body.Replace("[[CompanyName]]", notification.Item.Name);
            body = body.Replace("[[FooterInfo]]", "NIBSS, Plot 1230, Ahmadu Bello Way, Bar Beach, Victoria Island, P. M. B. 12617, Lagos.");

            mailSubject = $"Attention: Customer Onboarding Data Summission";

            await _emailService.SendEmailAsync(notification.AdminName, notification.AdminEmail, mailSubject, body);
        }
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unable to send onboarding email.", null);
		}
	}
}

