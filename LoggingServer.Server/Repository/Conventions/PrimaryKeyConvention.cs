using System;
using FluentNHibernate.Conventions;

namespace LoggingServer.Server.Repository.Conventions
{
    public class PrimaryKeyConvention : IIdConvention
    {
        public void Apply(FluentNHibernate.Conventions.Instances.IIdentityInstance instance)
        {
            instance.UnsavedValue(Guid.Empty.ToString());
            instance.GeneratedBy.GuidComb();
        }
    }
}
