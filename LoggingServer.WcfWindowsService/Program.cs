using System.ServiceProcess;

namespace LoggingServer.WcfWindowsService
{
    static class Program
    {
        static void Main()
        {
            var servicesToRun = new ServiceBase[] { new WcfLoggingService() };
            ServiceBase.Run(servicesToRun);
        }
    }
}
