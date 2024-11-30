using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using SurveyBasket.API.Settings;

namespace SurveyBasket.API.Services.EmailService
{
	public class EmailSender(IOptions<MailOptions> mailOptions) : IEmailSender
	{
		private readonly MailOptions _mailOptions = mailOptions.Value;

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var message = new MimeMessage
			{
				Sender = MailboxAddress.Parse(_mailOptions.Email),
				Subject = subject
			};

			message.To.Add(MailboxAddress.Parse(email));

			var builder = new BodyBuilder
			{
				HtmlBody = htmlMessage
			};

			message.Body = builder.ToMessageBody();

			using var smtp = new SmtpClient();


			smtp.Connect(_mailOptions.Host, _mailOptions.Port, SecureSocketOptions.StartTls);
			smtp.Authenticate(_mailOptions.Email, _mailOptions.Password);
			await smtp.SendAsync(message);
			smtp.Disconnect(true);
		}
	}
}