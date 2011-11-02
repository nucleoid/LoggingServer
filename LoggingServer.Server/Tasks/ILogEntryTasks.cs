using System;
using System.Linq;
using LoggingServer.Server.Domain;

namespace LoggingServer.Server.Tasks
{
    public interface ILogEntryTasks
    {
        LogEntry Get(Guid id);
        IQueryable<LogEntry> Paged(int? pageIndex, int? pageSize, SearchFilter filter);
        int Count(SearchFilter filter);
        IQueryable<LogEntry> AddFilterToQuery(SearchFilter filter, IQueryable<LogEntry> queryable);
    }
}
