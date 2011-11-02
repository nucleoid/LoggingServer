using System.Net.Mail;

namespace LoggingServer.Server.Tasks
{
    public interface ISmtpMailer
    {
        void Send(MailMessage message);
    }
}
