using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository.Conventions;

namespace LoggingServer.Server.Repository
{
    public class AutoPersistenceModelGenerator
    {
        public static AutoPersistenceModel Generate()
        {
            var mappings = AutoMap.AssemblyOf<Project>(new AutomappingConfiguration());
            mappings.Conventions.Setup(GetConventions());
            mappings.UseOverridesFromAssemblyOf<AutoPersistenceModelGenerator>();

            return mappings;
        }

        private static Action<IConventionFinder> GetConventions()
        {
            return c =>
            {
                c.Add<PrimaryKeyConvention>();
                c.Add<TableNameConvention>();
                c.Add<ReferenceConvention>();
                c.Add<HasManyConvention>();
            };
        }
    }
}
