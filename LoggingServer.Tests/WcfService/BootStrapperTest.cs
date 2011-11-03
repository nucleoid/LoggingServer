using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoggingServer.Server.Autofac;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using LoggingServer.Server.Tasks;
using LoggingServer.WcfService;
using LoggingServer.WcfService.Quartz;
using MbUnit.Framework;
using NHibernate;
using Rhino.Mocks;

namespace LoggingServer.Tests.WcfService
{
    [TestFixture]
    public class BootStrapperTest
    {
        [Test]
        public void Start_Registers_Modules_And_Builds_Container()
        {
            //Arrange
            DependencyContainer.Reset();

            //Act
            var scheduler = BootStrapper.Start(false);

            //Assert
            Assert.IsNotNull(DependencyContainer.Resolve<ISession>());
            Assert.IsNotNull(DependencyContainer.Resolve<IReadableRepository<LogEntry>>());
            Assert.IsNotNull(DependencyContainer.Resolve<ISubscriptionTasks>());
            scheduler.Shutdown(false);
        }

        [Test]
        public void Start_Starts_Scheduler()
        {
            //Arrange
            var repository = MockRepository.GenerateMock<IReadableRepository<LogEntry>>();
            var tasks = MockRepository.GenerateMock<ISubscriptionTasks>();
            DependencyContainer.RegisterInstance(repository);
            DependencyContainer.RegisterInstance(tasks);
            DependencyContainer.BuildContainer();
            var now = DateTime.Now;

            //Act
            var scheduler = BootStrapper.Start(false);

            //Assert
            if(now.Hour >= 8)
                Assert.AreEqual(now.Date.AddDays(1).AddHours(8).ToUniversalTime(), scheduler.GetTrigger("Subscription Job Trigger", null).GetNextFireTimeUtc());
            else
                Assert.AreEqual(now.Date.AddHours(8).ToUniversalTime(), scheduler.GetTrigger("Subscription Job Trigger", null).GetNextFireTimeUtc());
            var detail = scheduler.GetJobDetail("Subscription Job Detail", null);
            Assert.AreEqual(typeof(SubscriptionJob), detail.JobType);
            scheduler.Shutdown(false);
        }
    }
}
