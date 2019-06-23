using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Demo.Core.MVC.Utils
{
    public class EmailHandler : IEmailHandler
    {
        private readonly SmtpClient _smtpClient;
        private readonly MailAddress _from;

        public EmailHandler(EmailOptions options)
        {
            _smtpClient = new SmtpClient
            {
                UseDefaultCredentials = options.UseDefaultCredentials,
                Host = options.Host,
                Port = options.Port,
                EnableSsl = options.EnableSSL,
                Credentials = new NetworkCredential(options.UserName, options.Password)
            };

            _from = new MailAddress(options.UserName);
        }

        public async Task SendMailAsync(string subject, string body, string to, params string[] toParameters)
        {
            MailMessage mailMessage = new MailMessage
            {
                From = this._from
            };
            mailMessage.To.Add(to);

            foreach (var toParameter in toParameters)
            {
                mailMessage.To.Add(toParameter);
            }

            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.Body = body;
            mailMessage.Subject = subject;

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
