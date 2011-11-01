using System.Web.Hosting;
using System.Web.Mvc;
using LoggingServer.Interface.Controllers;
using LoggingServer.Server.Autofac;
using LoggingServer.Server.Automapper;
using MbUnit.Framework;
using Autofac;

namespace LoggingServer.Tests.Server.Autofac
{
    [TestFixture]
    public class MVCModuleTest
    {
        [Test]
        public void Load_Registers_MVC_Types()
        {
            //Arrange
            DependencyContainer.Reset();

            //Act
            DependencyContainer.Register(new MVCModule(typeof(HomeController).Assembly));
            DependencyContainer.BuildContainer();

            //Assert
//            Assert.IsTrue(DependencyContainer.Container.IsRegistered<IModelBinder>()); //no binders yet
            Assert.IsTrue(DependencyContainer.Container.IsRegistered<IModelBinderProvider>());
            Assert.IsTrue(DependencyContainer.Container.IsRegistered<HomeController>());
            Assert.IsTrue(DependencyContainer.Container.IsRegistered<VirtualPathProvider>());
            Assert.IsTrue(DependencyContainer.Container.IsRegistered<FilterResolver>());
        }
    }
}
