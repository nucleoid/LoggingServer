using NLog.Common;

namespace LoggingServer.Common.Targets
{
    public interface IWritableTarget
    {
        void WriteLogs(AsyncLogEventInfo[] logEvents);
    }
}
