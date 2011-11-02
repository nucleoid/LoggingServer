using System.Net.Mail;

namespace LoggingServer.Server.Tasks
{
    public class SmtpMailer :ISmtpMailer
    {
        private readonly string _host;

        public SmtpMailer(string host)
        {
            _host = host;
        }

        public void Send(MailMessage message)
        {
            using(var client = new SmtpClient(_host))
            {
                client.Send(message);
            }
        }
    }
}
