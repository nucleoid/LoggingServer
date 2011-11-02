using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using NLog;
using Quartz;

namespace LoggingServer.WcfService.Quartz
{
    public class SubscriptionJob : IJob
    {
        public IReadableRepository<Subscription> SubscriptionRepository { get; set; }
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void Execute(JobExecutionContext context)
        {
           
        }
    }
}