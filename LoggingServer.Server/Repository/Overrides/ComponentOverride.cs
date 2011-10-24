
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using LoggingServer.Server.Domain;

namespace LoggingServer.Server.Repository.Overrides
{
    public class ComponentOverride : IAutoMappingOverride<Component>
    {
        public void Override(AutoMapping<Component> mapping)
        {
            mapping.HasMany(x => x.LogEntries).KeyColumn("EntryAssemblyGuid")
            .Cascade.AllDeleteOrphan()
            .LazyLoad()
            .Inverse();
        }
    }
}
