
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using LoggingServer.Server.Tasks;
using MbUnit.Framework;
using Rhino.Mocks;

namespace LoggingServer.Tests.Server.Tasks
{
    [TestFixture]
    public class SubscriptionTasksTest
    {
        private SubscriptionTasks _tasks;
        private IReadableRepository<Subscription> _subscriptionRepository;
        private IEmailTasks _emailTasks;
        private ILogEntryTasks _logEntryTasks;
        private DateTime _now;

        [SetUp]
        public void Setup()
        {
            _subscriptionRepository = MockRepository.GenerateMock<IReadableRepository<Subscription>>();
            _emailTasks = MockRepository.GenerateMock<IEmailTasks>();
            _logEntryTasks = MockRepository.GenerateMock<ILogEntryTasks>();
            _tasks = new SubscriptionTasks(_subscriptionRepository, _emailTasks, _logEntryTasks);
            _now = DateTime.Parse("11/2/2011");
        }

        [Test]
        public void Notify_Emails_Subscriptions()
        {
            //Arrange
            var filter1 = new SearchFilter { ID = Guid.NewGuid(), LogLevel = LogLevel.Error | LogLevel.Debug, ComponentName = "dreams" };
            var filter2 = new SearchFilter { ID = Guid.NewGuid(), LogLevel = LogLevel.Fatal, ComponentName = "dreams" };
            var sub1 = new Subscription {ID = Guid.NewGuid(), EmailList = "klink@test.com,blah@test.com", Filter = filter1, IsDailyOverview = true};
            var sub2 = new Subscription {ID = Guid.NewGuid(), EmailList = "test@test.com,blah@test.com", Filter = filter1, IsDailyOverview = false};
            var sub3 = new Subscription {ID = Guid.NewGuid(), EmailList = "elephant@room.com", Filter = filter2, IsDailyOverview = false};
            var subscriptions = new List<Subscription> {sub1, sub2, sub3}.AsQueryable();
            _subscriptionRepository.Expect(x => x.All()).Return(subscriptions);
            var filtered1 = DomainTestHelper.GenerateLogEntries(_now).Take(12);
            var filtered2 = DomainTestHelper.GenerateLogEntries(_now).Take(1);
            _logEntryTasks.Expect(x => x.AddFilterToQuery(Arg<SearchFilter>.Matches(y => y.ID == filter1.ID),
                Arg<IQueryable<LogEntry>>.Matches(z => z.Count() == 16))).Return(filtered1);
            _logEntryTasks.Expect(x => x.AddFilterToQuery(Arg<SearchFilter>.Matches(y => y.ID == filter2.ID),
                Arg<IQueryable<LogEntry>>.Matches(z => z.Count() == 16))).Return(filtered2);
            var client = MockRepository.GenerateMock<ISmtpMailer>();
            _emailTasks.Expect(x => x.GenerateClient()).Return(client).Repeat.Twice();
            var message1 = new MailMessage("me@mail.com", "test@test.com,blah@test.com", string.Format("Log Subscription: {0}", filter1.Description),
                string.Format("<a href=\"http://localhost:61065/Logs/SavedSearch/{0}\">New matching log</a>", filter1.ID));
            _emailTasks.Expect(x => x.GenerateMessage(Arg<string>.Is.Equal(message1.From), Arg<string>.Is.Equal("test@test.com,blah@test.com"), 
                Arg<string>.Is.Equal(message1.Subject), Arg<string>.Is.Equal(message1.Body))).Return(message1);
            var message2 = new MailMessage("me@mail.com", "elephant@room.com", string.Format("Log Subscription: {0}", filter2.Description),
                string.Format("<a href=\"http://localhost:61065/Logs/SavedSearch/{0}\">New matching log</a>", filter2.ID));
            _emailTasks.Expect(x => x.GenerateMessage(Arg<string>.Is.Equal(message2.From), Arg<string>.Is.Equal("elephant@room.com"), 
                Arg<string>.Is.Equal(message2.Subject), Arg<string>.Is.Equal(message2.Body))).Return(message2);
            _emailTasks.Expect(x => x.SendEmail(Arg<ISmtpMailer>.Is.NotNull, Arg<MailMessage>.Matches(y => y.Body == message1.Body)));
            _emailTasks.Expect(x => x.SendEmail(Arg<ISmtpMailer>.Is.NotNull, Arg<MailMessage>.Matches(y => y.Body == message2.Body)));

            //Act
            _tasks.Notify(DomainTestHelper.GenerateLogEntries(_now).ToList());

            //Assert
            _subscriptionRepository.VerifyAllExpectations();
            _emailTasks.VerifyAllExpectations();
        }
    }
}
