using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using LoggingServer.Server.Domain;

namespace LoggingServer.Server.Repository.Overrides
{
    public class LogEntryOverride : IAutoMappingOverride<LogEntry>
    {
        private const int NVarCharMax = 10000;

        public void Override(AutoMapping<LogEntry> mapping)
        {
            mapping.References(x => x.Component, "EntryAssemblyGuid").NotFound.Ignore().LazyLoad().Not.Insert().Not.Update();
            mapping.Map(x => x.ExceptionStackTrace).Length(NVarCharMax);
            mapping.Map(x => x.ExceptionString).Length(NVarCharMax);
            mapping.Map(x => x.StackTrace).Length(NVarCharMax);
            mapping.Map(x => x.ExceptionMessage).Length(NVarCharMax);
            mapping.Map(x => x.ExceptionMethod).Length(NVarCharMax);
            mapping.Map(x => x.ExceptionType).Length(NVarCharMax);
            mapping.Map(x => x.BaseDirectory).Length(NVarCharMax);
            mapping.Map(x => x.CallSite).Length(NVarCharMax);
        }
    }
}
