using System.ServiceProcess;

namespace LoggingServer.LogTruncator
{
    static class Program
    {
        static void Main()
        {
            ServiceBase.Run(new ServiceBase[] { new TruncationService() });
        }
    }
}
