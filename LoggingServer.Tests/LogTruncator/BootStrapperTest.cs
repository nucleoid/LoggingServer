using System;
using System.Linq;
using LoggingServer.Common.Targets;
using LoggingServer.LogTruncator;
using LoggingServer.LogTruncator.Quartz;
using LoggingServer.Server.Autofac;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using MbUnit.Framework;
using NHibernate;
using NLog;
using NLog.Targets.Wrappers;
using Rhino.Mocks;
using LogLevel = NLog.LogLevel;

namespace LoggingServer.Tests.LogTruncator
{
    [TestFixture]
    public class BootStrapperTest
    {
        [Test]
        public void Start_Registers_Modules_And_Builds_Container()
        {
            //Arrange
            DependencyContainer.Reset();
            const string logTruncatorAssemblyGuid = "27875b7d-a0aa-46e3-99d5-22e489954bb7";

            //Act
            BootStrapper.Start(false, new string[0], DateTime.Parse("10/31/2011"));

            //Assert
            Assert.IsNotNull(DependencyContainer.Resolve<ISession>());
            Assert.IsNotNull(DependencyContainer.Resolve<IWritableRepository<LogEntry>>());
            Assert.Contains(LogManager.Configuration.LoggingRules[0].Levels, LogLevel.Info);
            var target = ((LogManager.Configuration.LoggingRules[0].Targets[0] as AsyncTargetWrapper).WrappedTarget as FallbackGroupTarget).Targets[0] as LoggingServerTarget;
            Assert.AreEqual("development", target.EnvironmentKey);
            Assert.AreEqual("http://localhost/LoggingServer.svc", target.EndpointAddress);
            Assert.AreEqual(string.Format("'{0}'", logTruncatorAssemblyGuid), 
                target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyGuid").Layout.ToString());
        } 

        [Test]
        public void Start_Starts_Scheduler()
        {
            //Arrange
            var repository = MockRepository.GenerateMock<IWritableRepository<LogEntry>>();
            DependencyContainer.RegisterInstance(repository);
            DependencyContainer.BuildContainer();
            var now = DateTime.Now;

            //Act
            var scheduler = BootStrapper.Start(false, new [] {"Error", "5"}, now);

            //Assert
            Assert.AreEqual(now.AddDays(1).Date.ToUniversalTime(), scheduler.GetTrigger("Truncation Job Trigger", null)
                .GetNextFireTimeUtc());
            var detail = scheduler.GetJobDetail("Truncation Job Detail", null);
            Assert.AreEqual(typeof(TruncationJob), detail.JobType);
            Assert.AreEqual(now, (DateTime)detail.JobDataMap[TruncationJob.NowKey]);
            Assert.AreEqual(LoggingServer.Server.Domain.LogLevel.Error, 
                (detail.JobDataMap[string.Format("{0}1", TruncationJob.TruncationKey)] as Truncation).LogLevel);
            Assert.AreEqual(5,
                (detail.JobDataMap[string.Format("{0}1", TruncationJob.TruncationKey)] as Truncation).RollingDays);
            scheduler.Shutdown(false);
        }

        [Test]
        public void Start_Adds_Multiple_Truncations()
        {
            //Arrange
            var repository = MockRepository.GenerateMock<IWritableRepository<LogEntry>>();
            DependencyContainer.RegisterInstance(repository);
            DependencyContainer.BuildContainer();
            var now = DateTime.Parse("10/31/2011");

            //Act
            var scheduler = BootStrapper.Start(false, new [] { "Error", "5", "Debug,Info", "7" }, now);

            //Assert
            var detail = scheduler.GetJobDetail("Truncation Job Detail", null);
            Assert.AreEqual(LoggingServer.Server.Domain.LogLevel.Error,
                (detail.JobDataMap[string.Format("{0}1", TruncationJob.TruncationKey)] as Truncation).LogLevel);
            Assert.AreEqual(5,
                (detail.JobDataMap[string.Format("{0}1", TruncationJob.TruncationKey)] as Truncation).RollingDays);
            Assert.AreEqual(LoggingServer.Server.Domain.LogLevel.Debug | LoggingServer.Server.Domain.LogLevel.Info,
                (detail.JobDataMap[string.Format("{0}2", TruncationJob.TruncationKey)] as Truncation).LogLevel);
            Assert.AreEqual(7,
                (detail.JobDataMap[string.Format("{0}2", TruncationJob.TruncationKey)] as Truncation).RollingDays);
            scheduler.Shutdown(false);
        }

        [Test, ExpectedArgumentException("Wrong number of arguments, has to be even")]
        public void Start_Fails_With_Bad_Number_Of_Args()
        {
            //Arrange
            var repository = MockRepository.GenerateMock<IWritableRepository<LogEntry>>();
            DependencyContainer.RegisterInstance(repository);
            DependencyContainer.BuildContainer();
            var now = DateTime.Parse("10/31/2011");

            //Act
            BootStrapper.Start(false, new [] { "Error", "5", "Debug | Info" }, now);
        }

        [Test, ExpectedArgumentException("A LogLevel argument failed to parse!")]
        public void Start_Fails_With_Bad_LogLevel_Arg()
        {
            //Arrange
            var repository = MockRepository.GenerateMock<IWritableRepository<LogEntry>>();
            DependencyContainer.RegisterInstance(repository);
            DependencyContainer.BuildContainer();
            var now = DateTime.Parse("10/31/2011");

            //Act
            BootStrapper.Start(false, new[] { "asdf", "5" }, now);
        }

        [Test, ExpectedArgumentException("A RollingDays argument failed to parse!, integer required")]
        public void Start_Fails_With_Bad_RollingDays_Arg()
        {
            //Arrange
            var repository = MockRepository.GenerateMock<IWritableRepository<LogEntry>>();
            DependencyContainer.RegisterInstance(repository);
            DependencyContainer.BuildContainer();
            var now = DateTime.Parse("10/31/2011");

            //Act
            BootStrapper.Start(false, new[] { "Error", "f" }, now);
        }
    }
}
