using System;
using System.ServiceProcess;
using Quartz;

namespace LoggingServer.LogTruncator
{
    public partial class TruncationService : ServiceBase
    {
        public IScheduler Scheduler { get; private set; }

        public TruncationService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Scheduler = BootStrapper.Start(true, args, DateTime.Now);
        }

        protected override void OnStop()
        {
            if(Scheduler != null)
                Scheduler.Shutdown(true);
        }
    }
}
