using LoggingServer.Server.Autofac;
using MbUnit.Framework;
using NHibernate;
using NHibernate.Cfg;
using Autofac;

namespace LoggingServer.Tests.Server.Autofac
{
    [TestFixture]
    public class DBModuleTest
    {
        [Test]
        public void Load_Configures_DB_And_Registers_Instances()
        {
            //Act
            DependencyContainer.Register(new DBModule());
            DependencyContainer.BuildContainer();

            //Assert
            Assert.LessThan(0, DependencyContainer.Resolve<Configuration>().ClassMappings.Count);
            Assert.IsTrue(DependencyContainer.Container.IsRegistered<ISessionFactory>());
            Assert.IsTrue(DependencyContainer.Container.IsRegistered<ISession>());
        }
    }
}
