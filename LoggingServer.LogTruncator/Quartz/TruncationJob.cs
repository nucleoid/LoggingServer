using System;
using System.Linq;
using System.Linq.Expressions;
using LoggingServer.Common.Extensions;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using NLog;
using Quartz;
using LogLevel = LoggingServer.Server.Domain.LogLevel;

namespace LoggingServer.LogTruncator.Quartz
{
    public class TruncationJob : IJob
    {
        public static string NowKey = "LogNow";
        public static string TruncationKey = "Truncation#";

        public IWritableRepository<LogEntry> LogEntryRepository { get; set; }
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void Execute(JobExecutionContext context)
        {
            var truncationKeys = context.MergedJobDataMap.Keys.Cast<string>().Where(x => x.Contains(TruncationKey));
            var dateTime = (DateTime)context.MergedJobDataMap[NowKey];

            foreach (var key in truncationKeys)
            {
                var truncation = context.MergedJobDataMap[key] as Truncation;
                var query = LogEntryRepository.All().Where(y => y.DateAdded <= dateTime.AddDays(-truncation.RollingDays));
                if (truncation.LogLevel.HasValue)
                    query = query.Where(GenerateLogLevelLambda(truncation.LogLevel.Value));
                var entries = query.ToList();
                if (entries.Count > 0)
                {
                    LogEntryRepository.Delete(entries);
                    Logger.Info("Deleting {0} LogEntries{1}", entries.Count, 
                        truncation.LogLevel.HasValue ? string.Format(" with log level {0}", truncation.LogLevel) : string.Empty);
                }
            }
        }

        private static Expression<Func<LogEntry, bool>> GenerateLogLevelLambda(LogLevel logLevel)
        {
            var param = Expression.Parameter(typeof(LogEntry), "l");
            Expression body = null;
            foreach (var field in EnumExtensions.BitFieldAsEnumerable(logLevel))
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
