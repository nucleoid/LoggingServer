using System.ServiceProcess;

namespace LoggingServer.Service
{
    static class Program
    {
        static void Main()
        {
            var servicesToRun = new ServiceBase[] { new LoggingService() };
            ServiceBase.Run(servicesToRun);
        }
    }
}
