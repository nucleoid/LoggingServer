
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using LoggingServer.Common;
using LoggingServer.LogTruncator.Quartz;
using LoggingServer.Server.Autofac;
using LoggingServer.Server.Quartz;
using NLog;
using Quartz;
using Quartz.Impl;

namespace LoggingServer.LogTruncator
{
    public static class BootStrapper
    {
        public static IScheduler Start(bool runMigrations, string[] args, DateTime now)
        {
            if(DependencyContainer.Container == null)
            {
                DependencyContainer.Register(new DBModule(runMigrations), new RepositoryModule());
                DependencyContainer.BuildContainer();
            }
            var environment = ConfigurationManager.AppSettings["environment"];
            var loggingServerEndPoint = ConfigurationManager.AppSettings["loggingServerEndPoint"];
            LogManager.Configuration = NLogConfiguration.ConfigureServerLogger(null, environment, loggingServerEndPoint,
                Assembly.GetExecutingAssembly(), LogLevel.Info);

            return StartScheduler(args, now);
        }

        private static IScheduler StartScheduler(string[] args, DateTime now)
        {
            if (args == null || args.Length == 0)
                return null;
            if (args.Length % 2 != 0)
                throw new ArgumentException("Wrong number of arguments, has to be even");

            var dictionary = ParseArgs(args, now);

            ISchedulerFactory factory = new StdSchedulerFactory();
            var scheduler = factory.GetScheduler();
            scheduler.JobFactory = new AutofacJobFactory(DependencyContainer.Container);
            scheduler.Start();

            var trigger = TriggerUtils.MakeDailyTrigger(0, 0);
            trigger.Name = "Truncation Job Trigger";
            var detail = new JobDetail("Truncation Job Detail", null, typeof(TruncationJob))
                {
                    JobDataMap = new JobDataMap(dictionary)
                };
            scheduler.ScheduleJob(detail, trigger);
            return scheduler;
        }

        private static Dictionary<string, object> ParseArgs(string[] args, DateTime now)
        {
            var dictionary = new Dictionary<string, object> {{TruncationJob.NowKey, now}};
            for (int i = 0; i < args.Length; i = i + 2)
            {
                Server.Domain.LogLevel? usableLogLevel = null;
                if (!string.IsNullOrEmpty(args[i]))
                {
                    Server.Domain.LogLevel parsedLogLevel;
                    if (!TryParseEnum(args[i], out parsedLogLevel))
                        throw new ArgumentException("A LogLevel argument failed to parse!");
                    usableLogLevel = parsedLogLevel;
                }

                int rollingDays;
                if (!int.TryParse(args[i + 1], out rollingDays))
                    throw new ArgumentException("A RollingDays argument failed to parse!, integer required");
                dictionary.Add(string.Format("{0}{1}", TruncationJob.TruncationKey, dictionary.Count),
                               new Truncation {LogLevel = usableLogLevel, RollingDays = rollingDays});
            }
            return dictionary;
        }

        /// <summary>
        /// Had to use .Net 3.5 because of fluent migrator and 3.5 doesn't have Enum.Tryparse
        /// </summary>
        /// <param name="s"></param>
        /// <param name="parsedLogLevel"></param>
        /// <returns></returns>
        private static bool TryParseEnum(string value, out Server.Domain.LogLevel parsedLogLevel)
        {
            try
            {
                parsedLogLevel = (Server.Domain.LogLevel)Enum.Parse(typeof(Server.Domain.LogLevel), value);
            }catch(Exception)
            {
                parsedLogLevel = default(Server.Domain.LogLevel);
                return false;
            }
            return true;
        }
    }
}
