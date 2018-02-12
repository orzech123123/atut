using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Options;

namespace Atut.Services
{
    public class EmailLabsMailService : IEmailService
    {
        private readonly IOptions<EmailSettings> _emailSettings;

        public EmailLabsMailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public Task SendAsync(IdentityMessage message)
        {
            return SendEmailAsync(message.Destination, message.Subject, message.Body);
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mailMessage = new MailMessage(_emailSettings.Value.MailSender, email, subject, message);
            mailMessage.IsBodyHtml = true;

            var client = new SmtpClient
            {
                Port = _emailSettings.Value.MailPort,
                Host = _emailSettings.Value.MailHost,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.Value.MailUsername, _emailSettings.Value.MailPassword)
            };
            client.SendCompleted += (s, e) => {
                client.Dispose();
                mailMessage.Dispose();
            };

            return client.SendMailAsync(mailMessage);
        }
    }
}
