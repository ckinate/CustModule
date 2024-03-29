using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Domain.Events.OnboardingProduct;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Fintrak.CustomerPortal.Application.OnboardingProduct.EventHandlers
{
	public class ProductOnboardStatusEventHandler : INotificationHandler<ProductOnboardStatusEvent>
	{
		private readonly ILogger<ProductOnboardStatusEventHandler> _logger;
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IIdentityService _identityService;
		private readonly IEmailService _emailService;
		private readonly IConfiguration _configuration;

		public ProductOnboardStatusEventHandler(ILogger<ProductOnboardStatusEventHandler> logger,
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

		public async Task Handle(ProductOnboardStatusEvent notification, CancellationToken cancellationToken)
		{
			try
			{
				if (string.IsNullOrEmpty(notification.NotificationEmail))
					throw new Exception("Notification email not set.");

				//Template placeholder
				//[[PreHeaderText]], [[CompanyName]], [[CallackLink]], [[FooterInfo]]

				BodyBuilder template = _emailService.GetEmailTemplateBody("product-data-state");

				var body = template.HtmlBody.Replace("[[PreHeaderText]]", "");
				body = body.Replace("[[Salutation]]", $"Dear {notification.AdminName}");
				body = body.Replace("[[CustomerName]]", notification.CustomerName);
				body = body.Replace("[[ProductName]]", notification.Item.ProductName);
				body = body.Replace("[[ProductCode]]", notification.Item.ProductCode);

				if (notification.Item.Status == Domain.Enums.OnboardingProductStatus.Queried)
				{
					body = body.Replace("[[Message]]", $"Your product onboarding information has been quried, please kindly response to the query on the portal.");
				}
				else if (notification.Item.Status == Domain.Enums.OnboardingProductStatus.Processing)
				{
					body = body.Replace("[[Message]]", $"Your product {notification.Item.ProductName} onboarding information is currently been processed.");
				}
				else if (notification.Item.Status == Domain.Enums.OnboardingProductStatus.Completed)
				{
					body = body.Replace("[[Message]]", $"Congratulation, your product {notification.Item.ProductName} onboarding information has been approved.");
				}
				else
				{
					body = body.Replace("[[Message]]", $"Product  {notification.Item.ProductName} onboarding status is {notification.Item.Status}");
				}

				body = body.Replace("[[FooterInfo]]", "NIBSS, Plot 1230, Ahmadu Bello Way, Bar Beach, Victoria Island, P. M. B. 12617, Lagos.");

				var mailSubject = $"Attention: Customer {notification.CustomerName} Product {notification.Item.ProductName} Registration Status.";

				await _emailService.SendEmailAsync(notification.AdminName, notification.AdminEmail, mailSubject, body);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unable to send product onboarding email.", null);
			}
		}
	}
}
