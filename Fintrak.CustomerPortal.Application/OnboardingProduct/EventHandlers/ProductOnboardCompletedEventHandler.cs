﻿using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Domain.Events.OnboardingProduct;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Fintrak.CustomerPortal.Application.OnboardingProduct.EventHandlers
{
	public class ProductOnboardCompletedEventHandler : INotificationHandler<ProductOnboardCompletedEvent>
	{
		private readonly ILogger<ProductOnboardCompletedEventHandler> _logger;
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IIdentityService _identityService;
		private readonly IEmailService _emailService;
		private readonly IConfiguration _configuration;

		public ProductOnboardCompletedEventHandler(ILogger<ProductOnboardCompletedEventHandler> logger,
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

		public async Task Handle(ProductOnboardCompletedEvent notification, CancellationToken cancellationToken)
		{
			try
			{
				if (string.IsNullOrEmpty(notification.NotificationEmail))
					throw new Exception("Notification email not set.");

				//Template placeholder
				//[[PreHeaderText]], [[CompanyName]], [[CallackLink]], [[FooterInfo]]

				BodyBuilder template = _emailService.GetEmailTemplateBody("product-data-submission");

				var body = template.HtmlBody.Replace("[[PreHeaderText]]", "");
				body = body.Replace("[[Salutation]]", $"Dear NIBSS");
				body = body.Replace("[[CustomerName]]", notification.CustomerName);
				body = body.Replace("[[ProductName]]", notification.Item.ProductName);
				body = body.Replace("[[ProductCode]]", notification.Item.ProductCode);
				body = body.Replace("[[FooterInfo]]", "NIBSS, Plot 1230, Ahmadu Bello Way, Bar Beach, Victoria Island, P. M. B. 12617, Lagos.");

				var mailSubject = $"Attention: Customer {notification.CustomerName} Submitted Product {notification.Item.ProductName} Onboarding Data";

				await _emailService.SendEmailAsync("NIBSS", notification.NotificationEmail, mailSubject, body);


				//-------------------------------
				template = _emailService.GetEmailTemplateBody("product-data-submission2");

				body = template.HtmlBody.Replace("[[PreHeaderText]]", "");
				body = body.Replace("[[Salutation]]", $"Dear {notification.AdminName}");
				body = body.Replace("[[CustomerName]]", notification.CustomerName);
				body = body.Replace("[[ProductName]]", notification.Item.ProductName);
				body = body.Replace("[[ProductCode]]", notification.Item.ProductCode);
				body = body.Replace("[[FooterInfo]]", "NIBSS, Plot 1230, Ahmadu Bello Way, Bar Beach, Victoria Island, P. M. B. 12617, Lagos.");

				mailSubject = $"Attention: Product Onboarding Data Summission";

				await _emailService.SendEmailAsync(notification.AdminName, notification.AdminEmail, mailSubject, body);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unable to send product onboarding email.", null);
			}
		}
	}
}