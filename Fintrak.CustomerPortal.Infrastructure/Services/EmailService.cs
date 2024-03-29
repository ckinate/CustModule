using Fintrak.CustomerPortal.Application.Common.Interfaces;
using MimeKit.Text;
using MimeKit;
using Microsoft.AspNetCore.Hosting;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace Fintrak.CustomerPortal.Infrastructure.Services
{
	public class EmailService : IEmailService
	{
		private readonly IHostingEnvironment _env;
		private readonly IConfiguration _configuration;

		public EmailService(IHostingEnvironment env, IConfiguration configuration)
		{
			_env = env;
			_configuration = configuration;
		}

		public Task SendEmailAsync(string fullName, string email, string subject, string message)
		{
			var fromName = _configuration["EmailSettings:SenderName"];
			var fromEmail = _configuration["EmailSettings:SenderEmail"];
			var fromPassword = _configuration["EmailSettings:SenderPassword"];
			var mailHostServer = _configuration["EmailSettings:HostServer"];
			var mailHostPort = int.Parse(_configuration["EmailSettings:HostPort"].ToString());
			var useSSL = bool.Parse(_configuration["EmailSettings:EnableSSL"].ToString());
			

			var mail = new MimeMessage();
			mail.To.Add(new MailboxAddress(fullName, email));
			mail.From.Add(new MailboxAddress(fromName, fromEmail));

			mail.Subject = subject;
			//We will say we are sending HTML. But there are options for plaintext etc. 
			mail.Body = new TextPart(TextFormat.Html)
			{
				Text = message
			};

			//Be careful that the SmtpClient class is the one from Mailkit not the framework!
			using (var emailClient = new SmtpClient())
			{
				//The last parameter here is to use SSL (Which you should!)
				emailClient.Connect(mailHostServer, mailHostPort, useSSL);

				//Remove any OAuth functionality as we won't be using it. 
				emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

				if(!string.IsNullOrEmpty(fromPassword))
					emailClient.Authenticate(fromEmail, fromPassword);

				emailClient.Send(mail);

				emailClient.Disconnect(true);
			}

			return Task.CompletedTask;
		}

		public string GetEmailTemplate(string templateName)
		{
			//Get TemplateFile located at wwwroot/Templates/EmailTemplate/Register_EmailTemplate.html  
			var pathToFile = _env.WebRootPath
					+ Path.DirectorySeparatorChar.ToString()
					+ "templates"
					+ Path.DirectorySeparatorChar.ToString()
					+ "emailtemplate"
					+ Path.DirectorySeparatorChar.ToString()
					+ $"{templateName}.html";

			var builder = new BodyBuilder();

			using (StreamReader SourceReader = File.OpenText(pathToFile))
			{
				builder.HtmlBody = SourceReader.ReadToEnd();
			}

			return builder.HtmlBody;
		}

		public BodyBuilder GetEmailTemplateBody(string templateName)
		{
			//Get TemplateFile located at wwwroot/Templates/EmailTemplate/Register_EmailTemplate.html  
			var pathToFile = _env.WebRootPath
					+ Path.DirectorySeparatorChar.ToString()
					+ "templates"
					+ Path.DirectorySeparatorChar.ToString()
					+ "emailtemplate"
					+ Path.DirectorySeparatorChar.ToString()
					+ $"{templateName}.html";

			var builder = new BodyBuilder();

			using (StreamReader SourceReader = File.OpenText(pathToFile))
			{
				builder.HtmlBody = SourceReader.ReadToEnd();
			}

			return builder;
		}
	}
}
