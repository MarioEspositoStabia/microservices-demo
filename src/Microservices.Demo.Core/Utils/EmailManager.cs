using System.Net;
using System.Net.Mail;
using System.Text;

namespace Microservices.Demo.Core.Utils
{
    public static class EmailManager
    {
        private static readonly SmtpClient _smtpClient;

        static EmailManager()
        {
            _smtpClient = new SmtpClient();
        }

        public static void SetSmptpClient(
            string host,
            int port,
            bool enableSSL,
            NetworkCredential networkCredential,
            bool useDefaultCredentials = false)
        {
            _smtpClient.UseDefaultCredentials = useDefaultCredentials;
            _smtpClient.Host = host;
            _smtpClient.Port = port;
            _smtpClient.EnableSsl = enableSSL;
            _smtpClient.Credentials = networkCredential;
        }

        public static void SendMail(string subject, string body, string from, string to, params string[] toParameters)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(from);
            mailMessage.To.Add(to);
            foreach (var toParameter in toParameters)
            {
                mailMessage.To.Add(toParameter);
            }

            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.Body = body;
            mailMessage.Subject = subject;
            _smtpClient.Send(mailMessage);
        }
    }
}
