using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Domain.Events.Onboarding;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Fintrak.CustomerPortal.Application.Onboarding.EventHandlers;
public class CustomerDueDiligenceUpdatedEventHandler : INotificationHandler<CustomerDueDiligenceCompletedEvent>
{
	private readonly ILogger<CustomerDueDiligenceUpdatedEventHandler> _logger;
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IIdentityService _identityService;
	private readonly IEmailService _emailService;
	private readonly IConfiguration _configuration;

	public CustomerDueDiligenceUpdatedEventHandler(ILogger<CustomerDueDiligenceUpdatedEventHandler> logger, 
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

	public async Task Handle(CustomerDueDiligenceCompletedEvent notification, CancellationToken cancellationToken)
	{
		try
		{
			//if (string.IsNullOrEmpty(notification.NotificationEmail))
			//	throw new Exception("Notification email not set.");

			//Template placeholder
			//[[PreHeaderText]], [[CompanyName]], [[CallackLink]], [[FooterInfo]]

			BodyBuilder template = _emailService.GetEmailTemplateBody("customer-data-duediligence");

			var body = template.HtmlBody.Replace("[[PreHeaderText]]", "");
			body = body.Replace("[[Salutation]]", $"Dear {notification.AdminName}");
			body = body.Replace("[[CompanyName]]", notification.Item.Name);
			body = body.Replace("[[FooterInfo]]", "NIBSS, Plot 1230, Ahmadu Bello Way, Bar Beach, Victoria Island, P. M. B. 12617, Lagos.");

			var mailSubject = $"Attention: Customer Due Diligence Completed";

			await _emailService.SendEmailAsync(notification.AdminName, notification.AdminEmail, mailSubject, body);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unable to send due diligence email.", null);
		}
	}
}

