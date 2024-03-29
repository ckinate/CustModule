using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Domain.Events.Onboarding;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Fintrak.CustomerPortal.Application.Onboarding.EventHandlers;
public class CustomerOnboardStatusEventHandler : INotificationHandler<CustomerOnboardStatusEvent>
{
	private readonly ILogger<CustomerOnboardStatusEventHandler> _logger;
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IIdentityService _identityService;
	private readonly IEmailService _emailService;
	private readonly IConfiguration _configuration;

	public CustomerOnboardStatusEventHandler(ILogger<CustomerOnboardStatusEventHandler> logger, 
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

	public async Task Handle(CustomerOnboardStatusEvent notification, CancellationToken cancellationToken)
	{
		try
		{
			if (string.IsNullOrEmpty(notification.NotificationEmail))
				throw new Exception("Notification email not set.");

			//Template placeholder
			//[[PreHeaderText]], [[CompanyName]], [[CallackLink]], [[FooterInfo]]

			BodyBuilder template = _emailService.GetEmailTemplateBody("customer-data-state");
			
			var body = template.HtmlBody.Replace("[[PreHeaderText]]", "");
            body = body.Replace("[[Salutation]]", $"Dear {notification.AdminName}");
            body = body.Replace("[[CompanyName]]", notification.Item.Name);

			
			if(notification.Item.Status == Domain.Enums.OnboardingStatus.Queried)
			{
				body = body.Replace("[[Message]]", $"Your onboarding information has been quried, please kindly response to the query on the portal.");
			}
			else if (notification.Item.Status == Domain.Enums.OnboardingStatus.Processing)
			{
				body = body.Replace("[[Message]]", $"Your onboarding information is currently been processed.");
			}
			else if (notification.Item.Status == Domain.Enums.OnboardingStatus.Completed)
			{
				if (notification.Item.DueDiligenceCompleted)
				{
					body = body.Replace("[[Message]]", $"Congratulation, your onboarding information has been approved.");
				}
				else
				{
					body = body.Replace("[[Message]]", $"Congratulation, your onboarding information has been approved. However, we are still during our due deligence on your company.");
				}
			}
			else
			{
				body = body.Replace("[[Message]]", $"{notification.Item.Name} onboarding status is {notification.Item.Status}");
			}

			body = body.Replace("[[FooterInfo]]", "NIBSS, Plot 1230, Ahmadu Bello Way, Bar Beach, Victoria Island, P. M. B. 12617, Lagos.");

			var mailSubject = $"Attention: {notification.Item.Name} Registration Status.";

			await _emailService.SendEmailAsync(notification.AdminName, notification.AdminEmail, mailSubject, body);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unable to send onboarding email.", null);
		}
	}
}

