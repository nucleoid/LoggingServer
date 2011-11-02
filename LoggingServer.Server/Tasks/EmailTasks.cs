using System.Configuration;
using System.Net.Mail;

namespace LoggingServer.Server.Tasks
{
    public class EmailTasks : IEmailTasks
    {
        public void SendEmail(ISmtpMailer client, MailMessage message)
        {
            client.Send(message);
        }

        public MailMessage GenerateMessage(string from, string to, string subject, string body)
        {
            return new MailMessage(from, to, subject, body);
        }

        public ISmtpMailer GenerateClient()
        {
            var host = ConfigurationManager.AppSettings["smtpHost"];
            return new SmtpMailer(host);
        }
    }
}
