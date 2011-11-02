using System.Net.Mail;
using LoggingServer.Server.Tasks;
using MbUnit.Framework;
using Rhino.Mocks;

namespace LoggingServer.Tests.Server.Tasks
{
    [TestFixture]
    public class EmailTasksTest
    {
        private EmailTasks _tasks;

        [SetUp]
        public void Setup()
        {
            _tasks = new EmailTasks();
        }

        [Test]
        public void SendEmail_Sends_Message()
        {
            //Arrange
            var message = new MailMessage();
            var client = MockRepository.GenerateMock<ISmtpMailer>();
            client.Expect(x => x.Send(message));

            //Act
            _tasks.SendEmail(client, message);

            //Assert
            client.VerifyAllExpectations();
        }

        [Test]
        public void GenerateMessage_Constructs_MailMessage()
        {
            //Arrange
            const string from = "me@mail.com";
            const string to = "test@test.com,blah@test.com";
            const string subject = "things";
            const string body = "kitty sleep";

            //Act
            var message = _tasks.GenerateMessage(from, to, subject, body);

            //Assert
            Assert.AreEqual(from, message.From.Address);
            Assert.AreEqual("test@test.com, blah@test.com", message.To.ToString());
            Assert.AreEqual(subject, message.Subject);
            Assert.AreEqual(body, message.Body);
        }

        [Test]
        public void GenerateClient_Uses_Config_Host_Value()
        {
            //Act
            var client = _tasks.GenerateClient();

            //Assert
            Assert.AreEqual("localhost", client.GetField("_host"));
        }
    }
}
