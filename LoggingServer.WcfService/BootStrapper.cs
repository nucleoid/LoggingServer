using System;
using LoggingServer.Server.Autofac;
using LoggingServer.Server.Quartz;
using Quartz;
using Quartz.Impl;

namespace LoggingServer.WcfService
{
    public static class BootStrapper
    {
        public static IScheduler Start(bool runMigrations, DateTime now)
        {
            if (DependencyContainer.Container == null)
            {
                DependencyContainer.Register(new DBModule(runMigrations), new RepositoryModule());
                DependencyContainer.BuildContainer();
            }
            return StartScheduler(now);
        }

        private static IScheduler StartScheduler(DateTime now)
        {
            ISchedulerFactory factory = new StdSchedulerFactory();
            var scheduler = factory.GetScheduler();
            scheduler.JobFactory = new AutofacJobFactory(DependencyContainer.Container);
            scheduler.Start();

            var trigger = TriggerUtils.MakeDailyTrigger(0, 0);
            trigger.Name = "Truncation Job Trigger";
//            var detail = new JobDetail("Truncation Job Detail", null, typeof(TruncationJob));
//            scheduler.ScheduleJob(detail, trigger);
            return scheduler;
        }
    }
}