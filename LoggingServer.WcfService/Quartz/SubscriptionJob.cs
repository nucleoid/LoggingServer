using System;
using System.Linq;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using LoggingServer.Server.Tasks;
using NLog;
using Quartz;

namespace LoggingServer.WcfService.Quartz
{
    public class SubscriptionJob : IJob
    {
        public static string NowKey = "LogNow";

        public ISubscriptionTasks SubscriptionTasks { get; set; }
        public IReadableRepository<LogEntry> LogEntryRepository { get; set; }
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public void Execute(JobExecutionContext context)
        {
            var now = DateTime.Now.Date.AddHours(8);
            var debug = LogEntryRepository.All();
            var dailyLogs = debug.Where(x => x.DateAdded >= now.AddDays(-1) && x.DateAdded <= now).ToList();
            _logger.Info("Retrieved {0} daily logs for subscription notification", dailyLogs.Count);
            SubscriptionTasks.AsyncNotify(dailyLogs, true);
        }
    }
}