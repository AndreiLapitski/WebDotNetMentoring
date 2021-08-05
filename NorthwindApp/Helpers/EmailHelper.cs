using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace NorthwindApp.Helpers
{
    public class EmailHelper : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            string senderEmail = _configuration.GetValue<string>(Constants.SenderEmailKey);
            string password = _configuration.GetValue<string>(Constants.SenderEmailPasswordKey);
            string smtpHost = _configuration.GetValue<string>(Constants.GmailSmtpHostKey);
            int port = _configuration.GetValue<int>(Constants.PortKey);
            bool useSsl = _configuration.GetValue<bool>(Constants.UseSslKey);

            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(toEmail, senderEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart(Constants.SubtypeHtmlKey)
            {
                Text = htmlMessage
            };

            using MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(smtpHost, port, useSsl);
            await client.AuthenticateAsync(senderEmail, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
