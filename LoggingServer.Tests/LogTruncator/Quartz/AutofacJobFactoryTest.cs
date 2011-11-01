using System;
using LoggingServer.LogTruncator;
using LoggingServer.LogTruncator.Quartz;
using LoggingServer.Server.Autofac;
using MbUnit.Framework;
using Quartz;
using Quartz.Spi;

namespace LoggingServer.Tests.LogTruncator.Quartz
{
    [TestFixture]
    public class AutofacJobFactoryTest
    {
        [Test]
        public void NewJob_Injects_Properties()
        {
            //Arrange
            BootStrapper.Start(false, null, DateTime.Parse("10/31/2011"));
            var jobDetail = new JobDetail("blag", null, typeof(TruncationJob));
            var trigger = TriggerUtils.MakeImmediateTrigger(0, TimeSpan.FromSeconds(2));
            var bundle = new TriggerFiredBundle(jobDetail, trigger, null, false, null, null, null, null);
            var factory = new AutofacJobFactory(DependencyContainer.Container);

            //Act
            var job = factory.NewJob(bundle) as TruncationJob;

            //Assert
            Assert.IsNotNull(job.LogEntryRepository);
        }
    }
}
