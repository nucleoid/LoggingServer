
using System.Collections.Generic;
using LoggingServer.Server.Domain;

namespace LoggingServer.Server.Tasks
{
    public interface ISubscriptionTasks
    {
        void AsyncNotify(IList<LogEntry> data);
        void Notify(IList<LogEntry> data);
    }
}
