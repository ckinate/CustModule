using MimeKit;

namespace Fintrak.CustomerPortal.Application.Common.Interfaces
{
	public interface IEmailService
	{
		Task SendEmailAsync(string fullName, string email, string subject, string message);

		string GetEmailTemplate(string templateName);

		BodyBuilder GetEmailTemplateBody(string templateName);
	}
}
