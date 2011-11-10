using System;
using System.Collections.Generic;
using System.Linq;
using LoggingServer.LogTruncator;
using LoggingServer.LogTruncator.Quartz;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using MbUnit.Framework;
using NLog;
using Quartz;
using Quartz.Spi;
using Rhino.Mocks;
using LogLevel = LoggingServer.Server.Domain.LogLevel;

namespace LoggingServer.Tests.LogTruncator.Quartz
{
    [TestFixture]
    public class TruncationJobTest
    {
        [SetUp]
        public void Setup()
        {
            LogManager.Configuration = null;
        }

        [Test]
        [Row(5, null, 7)]
        [Row(5, LogLevel.Error, 1)]
        [Row(5, LogLevel.Error | LogLevel.Info, 2)]
        [Row(0, LogLevel.Error, 3)]
        [Row(0, null, 21)]
        public void Execute_Deletes_LogEntries_By_Time_And_LogLevel(int days, LogLevel? logLevel, int count)
        {
            //Arrange
            var now = DateTime.Parse("10/31/2011");
            var logEntryRep = MockRepository.GenerateMock<IWritableRepository<LogEntry>>();
            logEntryRep.Expect(x => x.All()).Return(GenerateLogs(now));
            logEntryRep.Expect(x => x.Delete(Arg<IList<LogEntry>>.Matches(y => y.Count == count)));
            var job = new TruncationJob { LogEntryRepository = logEntryRep};
            var jobDetail = new JobDetail("blag", null, typeof(TruncationJob)) {JobDataMap = 
                    new JobDataMap(new Dictionary<string, object>
                    {
                        {TruncationJob.NowKey, now},
                        {string.Format("{0}1", TruncationJob.TruncationKey), new Truncation {LogLevel = logLevel, RollingDays = days}}
                    })};
            var trigger = TriggerUtils.MakeImmediateTrigger(0, TimeSpan.FromSeconds(2));
            var bundle = new TriggerFiredBundle(jobDetail, trigger, null, false, null, null, null, null);
            var jobExec = new JobExecutionContext(null, bundle, null);

            //Act
            job.Execute(jobExec);

            //Assert
            logEntryRep.VerifyAllExpectations();
        }

        [Test]
        public void Execute_With_No_Matching_Entries_Does_Nothing()
        {
            //Arrange
            var now = DateTime.Parse("10/31/2011");
            var logEntryRep = MockRepository.GenerateMock<IWritableRepository<LogEntry>>();
            logEntryRep.Expect(x => x.All()).Return(GenerateLogs(now));
            logEntryRep.Expect(x => x.Delete(Arg<IList<LogEntry>>.Is.Anything)).Repeat.Never();
            var job = new TruncationJob { LogEntryRepository = logEntryRep };
            var jobDetail = new JobDetail("blag", null, typeof(TruncationJob))
            {
                JobDataMap = new JobDataMap(new Dictionary<string, object>
                    {
                        {TruncationJob.NowKey, now},
                        {string.Format("{0}1", TruncationJob.TruncationKey), new Truncation {LogLevel = null, RollingDays = 6}}
                    })
            };
            var trigger = TriggerUtils.MakeImmediateTrigger(0, TimeSpan.FromSeconds(2));
            var bundle = new TriggerFiredBundle(jobDetail, trigger, null, false, null, null, null, null);
            var jobExec = new JobExecutionContext(null, bundle, null);

            //Act
            job.Execute(jobExec);

            //Assert
            logEntryRep.VerifyAllExpectations();
        }

        [Test]
        public void Execute_With_Multiple_Truncations()
        {
            //Arrange
            var now = DateTime.Parse("10/31/2011");
            var logEntryRep = MockRepository.GenerateMock<IWritableRepository<LogEntry>>();
            logEntryRep.Expect(x => x.All()).Return(GenerateLogs(now)).Repeat.Twice();
            logEntryRep.Expect(x => x.Delete(Arg<IList<LogEntry>>.Matches(y => y.Count == 1)));
            logEntryRep.Expect(x => x.Delete(Arg<IList<LogEntry>>.Matches(y => y.Count == 3)));
            var job = new TruncationJob { LogEntryRepository = logEntryRep };
            var jobDetail = new JobDetail("blag", null, typeof(TruncationJob))
            {
                JobDataMap =
                    new JobDataMap(new Dictionary<string, object>
                    {
                        {TruncationJob.NowKey, now},
                        {string.Format("{0}1", TruncationJob.TruncationKey), new Truncation {LogLevel = LogLevel.Debug, RollingDays = 5}},
                        {string.Format("{0}2", TruncationJob.TruncationKey), new Truncation {LogLevel = LogLevel.Debug, RollingDays = 0}}
                    })
            };
            var trigger = TriggerUtils.MakeImmediateTrigger(0, TimeSpan.FromSeconds(2));
            var bundle = new TriggerFiredBundle(jobDetail, trigger, null, false, null, null, null, null);
            var jobExec = new JobExecutionContext(null, bundle, null);

            //Act
            job.Execute(jobExec);

            //Assert
            logEntryRep.VerifyAllExpectations();
        }

        /// <summary>
        /// 21 LogEntries
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        private IQueryable<LogEntry> GenerateLogs(DateTime now)
        {
            return new List<LogEntry>
                       {
                           new LogEntry {DateAdded = now.AddDays(-5), LogLevel = LogLevel.Debug},
                           new LogEntry {DateAdded = now.AddDays(-5), LogLevel = LogLevel.Error},
                           new LogEntry {DateAdded = now.AddDays(-5), LogLevel = LogLevel.Fatal},
                           new LogEntry {DateAdded = now.AddDays(-5), LogLevel = LogLevel.Info},
                           new LogEntry {DateAdded = now.AddDays(-5), LogLevel = LogLevel.Off},
                           new LogEntry {DateAdded = now.AddDays(-5), LogLevel = LogLevel.Trace},
                           new LogEntry {DateAdded = now.AddDays(-5), LogLevel = LogLevel.Warn},

                           new LogEntry {DateAdded = now.AddDays(-1), LogLevel = LogLevel.Debug},
                           new LogEntry {DateAdded = now.AddDays(-1), LogLevel = LogLevel.Error},
                           new LogEntry {DateAdded = now.AddDays(-1), LogLevel = LogLevel.Fatal},
                           new LogEntry {DateAdded = now.AddDays(-1), LogLevel = LogLevel.Info},
                           new LogEntry {DateAdded = now.AddDays(-1), LogLevel = LogLevel.Off},
                           new LogEntry {DateAdded = now.AddDays(-1), LogLevel = LogLevel.Trace},
                           new LogEntry {DateAdded = now.AddDays(-1), LogLevel = LogLevel.Warn},

                           new LogEntry {DateAdded = now, LogLevel = LogLevel.Debug},
                           new LogEntry {DateAdded = now, LogLevel = LogLevel.Error},
                           new LogEntry {DateAdded = now, LogLevel = LogLevel.Fatal},
                           new LogEntry {DateAdded = now, LogLevel = LogLevel.Info},
                           new LogEntry {DateAdded = now, LogLevel = LogLevel.Off},
                           new LogEntry {DateAdded = now, LogLevel = LogLevel.Trace},
                           new LogEntry {DateAdded = now, LogLevel = LogLevel.Warn},
                       }.AsQueryable();
        }
    }
}
