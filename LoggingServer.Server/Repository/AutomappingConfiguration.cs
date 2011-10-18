using System;
using FluentNHibernate;
using FluentNHibernate.Automapping;

namespace LoggingServer.Server.Repository
{
    public class AutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Member member)
        {
            return base.ShouldMap(member) && member.CanWrite;
        }

        public override bool ShouldMap(Type type)
        {
            return base.ShouldMap(type) && !type.IsAbstract && type.Namespace.Contains("Domain");
        }

        public override bool IsId(Member member)
        {
            return member.Name == "ID" && (member.PropertyType == typeof(Guid) || member.PropertyType == typeof(Guid?));
        }
    }
}
