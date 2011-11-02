
using System;
using System.Linq;
using System.Linq.Expressions;
using LoggingServer.Common.Extensions;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;

namespace LoggingServer.Server.Tasks
{
    public class LogEntryTasks : ILogEntryTasks
    {
        private readonly IReadableRepository<LogEntry> _logEntryRepository;

        public static int DefaultPageSize = 500;

        public LogEntryTasks(IReadableRepository<LogEntry> logEntryRepository)
        {
            _logEntryRepository = logEntryRepository;
        }

        public LogEntry Get(Guid id)
        {
            return _logEntryRepository.Get(id);
        }

        public IQueryable<LogEntry> Paged(int? pageIndex, int? pageSize, SearchFilter filter)
        {
            var page = !pageIndex.HasValue | (pageIndex < 1) ? 1 : pageIndex.Value;
            var size = !pageSize.HasValue | pageSize < 0 ? DefaultPageSize : pageSize.Value;
            IQueryable<LogEntry> queryable = _logEntryRepository.All().OrderByDescending(x => x.DateAdded);
            if (filter != null)
                queryable = AddFilterToQuery(filter, queryable);
            queryable = queryable.Skip((page - 1) * size).Take(size);
            return queryable;
        }

        public int Count(SearchFilter filter)
        {
            IQueryable<LogEntry> queryable = _logEntryRepository.All();
            if (filter != null)
                return AddFilterToQuery(filter, queryable).Count();
            return queryable.Count();
        }

        public IQueryable<LogEntry> AddFilterToQuery(SearchFilter filter, IQueryable<LogEntry> queryable)
        {
            if (!string.IsNullOrEmpty(filter.ProjectName))
                queryable = queryable.Where(x => x.Component.Project.Name.ToLowerInvariant().Contains(filter.ProjectName.ToLowerInvariant()));
            if (!string.IsNullOrEmpty(filter.ComponentName))
                queryable = queryable.Where(x => x.Component.Name.ToLowerInvariant().Contains(filter.ComponentName.ToLowerInvariant()));
            if (!string.IsNullOrEmpty(filter.ExceptionPartial))
                queryable = queryable.Where(x => x.ExceptionMessage.ToLowerInvariant().Contains(filter.ExceptionPartial.ToLowerInvariant()));
            if (!string.IsNullOrEmpty(filter.MachineNamePartial))
                queryable = queryable.Where(x => x.MachineName.ToLowerInvariant().Contains(filter.MachineNamePartial.ToLowerInvariant()));
            if (!string.IsNullOrEmpty(filter.MessagePartial))
                queryable = queryable.Where(x => x.LogMessage.ToLowerInvariant().Contains(filter.MessagePartial.ToLowerInvariant()));
            if (filter.LogLevel.HasValue)
                queryable = queryable.Where(GenerateLogLevelLambda(filter));
            if (filter.StartDate.HasValue)
                queryable = queryable.Where(x => x.DateAdded >= filter.StartDate.Value);
            if (filter.EndDate.HasValue)
                queryable = queryable.Where(x => x.DateAdded <= filter.EndDate.Value);
            return queryable;
        }

        private static Expression<Func<LogEntry, bool>> GenerateLogLevelLambda(SearchFilter filter)
        {
            var param = Expression.Parameter(typeof (LogEntry), "l");
            Expression body = null;
            foreach (var field in EnumExtensions.BitFieldAsEnumerable(filter.LogLevel.Value))
            {
                if (body == null)
                    body = Expression.Equal(Expression.Property(param, "LogLevel"), Expression.Constant(field));
                else
                    body = Expression.OrElse(body, Expression.Equal(Expression.Property(param, "LogLevel"), Expression.Constant(field)));
            }
            var lambda = Expression.Lambda<Func<LogEntry, bool>>(body, param);
            return lambda;
        }
    }
}
