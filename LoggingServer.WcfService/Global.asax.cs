using System;
using System.Web;
using Quartz;

namespace LoggingServer.WcfService
{
    public class Global : HttpApplication
    {
        private IScheduler _scheduler;

        protected void Application_Start(object sender, EventArgs e)
        {
            _scheduler = BootStrapper.Start();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            if(_scheduler != null)
                _scheduler.Shutdown(true);
        }
    }
}