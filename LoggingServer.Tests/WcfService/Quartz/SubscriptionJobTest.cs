
using System;
using System.Collections.Generic;
using System.Linq;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using LoggingServer.Server.Tasks;
using LoggingServer.Tests.Server;
using LoggingServer.WcfService.Quartz;
using MbUnit.Framework;
using Quartz;
using Quartz.Spi;
using Rhino.Mocks;

namespace LoggingServer.Tests.WcfService.Quartz
{
    [TestFixture]
    public class SubscriptionJobTest
    {
        [Test]
        public void Execute_Finds_Daily_Logs_And_Sends_Daily_Subscription_Notifications()
        {
            //Arrange
            var now = DateTime.Now.Date.AddHours(8);
            var logEntryRep = MockRepository.GenerateMock<IReadableRepository<LogEntry>>();
            logEntryRep.Expect(x => x.All()).Return(DomainTestHelper.GenerateLogEntries(now));
            var subscriptionTasks = MockRepository.GenerateMock<ISubscriptionTasks>();
            subscriptionTasks.Expect(x => x.AsyncNotify(Arg<IList<LogEntry>>.Matches(y => y.All(z => z.DateAdded >= now.AddDays(-1) && 
                z.DateAdded <= now) && y.Count == 12), Arg<bool>.Is.Equal(true)));
            var job = new SubscriptionJob { LogEntryRepository = logEntryRep, SubscriptionTasks = subscriptionTasks };
            var jobDetail = new JobDetail("blag", null, typeof(SubscriptionJob))
            {
                JobDataMap = new JobDataMap(new Dictionary<string, object> { { SubscriptionJob.NowKey, now } })
            };
            var trigger = TriggerUtils.MakeImmediateTrigger(0, TimeSpan.FromSeconds(2));
            var bundle = new TriggerFiredBundle(jobDetail, trigger, null, false, null, null, null, null);
            var jobExec = new JobExecutionContext(null, bundle, null);

            //Act
            job.Execute(jobExec);

            //Assert
            logEntryRep.VerifyAllExpectations();
            subscriptionTasks.VerifyAllExpectations();
        }
    }
}
