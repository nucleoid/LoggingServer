using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using LoggingServer.Server.Domain;

namespace LoggingServer.Server.Repository.Overrides
{
    public class ProjectOverride : IAutoMappingOverride<Project>
    {
        public void Override(AutoMapping<Project> mapping)
        {
            mapping.Map(x => x.Name).Unique();
            mapping.Cache.ReadWrite();
        }
    }
}
