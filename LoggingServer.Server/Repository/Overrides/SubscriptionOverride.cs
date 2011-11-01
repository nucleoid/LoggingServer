using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using LoggingServer.Server.Domain;

namespace LoggingServer.Server.Repository.Overrides
{
    public class SubscriptionOverride : IAutoMappingOverride<Subscription>
    {
        public void Override(AutoMapping<Subscription> mapping)
        {
            mapping.IgnoreProperty(x => x.Emails);
        }
    }
}
