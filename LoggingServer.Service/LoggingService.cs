using System;
using System.ServiceModel;
using System.ServiceProcess;
using LoggingServer.Server;
using LoggingServer.Server.Autofac;
using Autofac;

namespace LoggingServer.Service
{
    public partial class LoggingService : ServiceBase
    {
        private ServiceHost _serviceHost;

        public LoggingService()
        {
            InitializeComponent();
            BootStrapper.Start();
            AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            var server = DependencyContainer.Container.Resolve<LogReceiverServer>();
            _serviceHost = new ServiceHost(server, new Uri("http://localhost:8000/"));
            _serviceHost.Open();
        }

        protected override void OnStop()
        {
            _serviceHost.Close();
        }
    }
}
