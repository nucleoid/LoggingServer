using Autofac;
using LoggingServer.Server.Autofac;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using MbUnit.Framework;

namespace LoggingServer.Tests.Server.Autofac
{
    [TestFixture]
    public class RepositoryModuleTest
    {
        [Test]
        public void Load_Registers_Repositories()
        {
            //Arrange
            DependencyContainer.Reset();

            //Act
            DependencyContainer.Register(new RepositoryModule());
            DependencyContainer.BuildContainer();

            //Assert
            Assert.IsTrue(DependencyContainer.Container.IsRegistered<IReadableRepository<Project>>());
            Assert.IsTrue(DependencyContainer.Container.IsRegistered<IWritableRepository<Project>>());
        }
    }
}
