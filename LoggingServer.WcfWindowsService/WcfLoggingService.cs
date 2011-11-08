using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using LoggingServer.Server;
using Quartz;

namespace LoggingServer.WcfWindowsService
{
    public partial class WcfLoggingService : ServiceBase
    {
        private IScheduler _scheduler;
        private ServiceHost _host;

        public WcfLoggingService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _scheduler = BootStrapper.Start();
            if (_host != null)
                _host.Close();
            _host = new ServiceHost(typeof(LogReceiverServer));
            _host.Open();
        }

        protected override void OnStop()
        {
            if(_scheduler != null)
                _scheduler.Shutdown(true);
            if (_host != null)
            {
                _host.Close();
                _host = null;
            }

        }
    }
}
