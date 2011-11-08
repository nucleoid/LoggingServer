using LoggingServer.Server.Autofac;
using LoggingServer.Server.Quartz;
using LoggingServer.WcfWindowsService.Quartz;
using Quartz;
using Quartz.Impl;

namespace LoggingServer.WcfWindowsService
{
    public static class BootStrapper
    {
        public static IScheduler Start()
        {
            if (DependencyContainer.Container == null)
            {
                DependencyContainer.Register(new DBModule(), new RepositoryModule(), new TaskModule());
                DependencyContainer.BuildContainer();
            }
            return StartScheduler();
        }

        private static IScheduler StartScheduler()
        {
            ISchedulerFactory factory = new StdSchedulerFactory();
            var scheduler = factory.GetScheduler();
            scheduler.JobFactory = new AutofacJobFactory(DependencyContainer.Container);
            scheduler.Start();

            var trigger = TriggerUtils.MakeDailyTrigger(8, 0);
            trigger.Name = "Subscription Job Trigger";
            var detail = new JobDetail("Subscription Job Detail", null, typeof(SubscriptionJob));
            scheduler.ScheduleJob(detail, trigger);
            return scheduler;
        }
    }
}