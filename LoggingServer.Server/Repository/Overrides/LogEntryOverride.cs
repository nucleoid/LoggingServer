using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using LoggingServer.Server.Domain;

namespace LoggingServer.Server.Repository.Overrides
{
    public class LogEntryOverride : IAutoMappingOverride<LogEntry>
    {
        public void Override(AutoMapping<LogEntry> mapping)
        {
            mapping.References(x => x.Component, "EntryAssemblyGuid").NotFound.Ignore().LazyLoad().Not.Insert().Not.Update();
        }
    }
}
