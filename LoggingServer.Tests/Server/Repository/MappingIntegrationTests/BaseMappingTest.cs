using LoggingServer.Server;
using LoggingServer.Server.Autofac;
using LoggingServer.Server.Repository;
using MbUnit.Framework;

namespace LoggingServer.Tests.Server.Repository.MappingIntegrationTests
{
    public abstract class BaseMappingTest<T> where T : class
    {
        protected IWritableRepository<T> Repository { get; set; }

        [SetUp]
        public void Setup()
        {
            DependencyContainer.Reset();
            BootStrapper.Start();
            Repository = DependencyContainer.Resolve<IWritableRepository<T>>();
        }
    }
}
