
using System.Net.Mail;

namespace LoggingServer.Server.Tasks
{
    public interface IEmailTasks
    {
        void SendEmail(ISmtpMailer client, MailMessage message);
        MailMessage GenerateMessage(string from, string to, string subject, string body);
        ISmtpMailer GenerateClient();
    }
}
