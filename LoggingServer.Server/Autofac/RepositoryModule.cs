using Autofac;
using LoggingServer.Server.Repository;

namespace LoggingServer.Server.Autofac
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ReadableRepository<>))
                .As(typeof(IReadableRepository<>));

            builder.RegisterGeneric(typeof(WriteableRepository<>))
                .As(typeof(IWritableRepository<>));
        }
    }
}
