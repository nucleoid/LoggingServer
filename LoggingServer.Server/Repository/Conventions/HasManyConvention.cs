
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace LoggingServer.Server.Repository.Conventions
{
    public class HasManyConvention : IHasManyConvention
    {
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Key.Column(instance.EntityType.Name + "Id");
            instance.Cascade.AllDeleteOrphan();
            instance.LazyLoad();
            instance.Inverse();
        }
    }
}
