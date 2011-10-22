using System;
using LoggingServer.Server;

namespace LoggingServer.WcfService
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            BootStrapper.Start();
        }
    }
}