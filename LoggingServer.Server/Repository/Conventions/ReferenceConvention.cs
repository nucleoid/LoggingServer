using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace LoggingServer.Server.Repository.Conventions
{
    public class ReferenceConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            instance.Column(instance.Property.Name + "Id");
            instance.Cascade.None();
            instance.LazyLoad();
        }
    }
}
