
using System.Collections.Generic;
using LoggingServer.Server.Domain;

namespace LoggingServer.Server.Tasks
{
    public interface ISubscriptionTasks
    {
        void AsyncNotify(IList<LogEntry> data, bool isDaily);
        void Notify(IList<LogEntry> data, bool isDaily);
    }
}
