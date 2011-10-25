using Autofac;
using LoggingServer.Server.Autofac;
using LoggingServer.Server.Tasks;
using MbUnit.Framework;

namespace LoggingServer.Tests.Server.Autofac
{
    [TestFixture]
    public class TaskModuleTest
    {
        [Test]
        public void Load_Registers_Task_Types()
        {
            //Arrange
            DependencyContainer.Reset();

            //Act
            DependencyContainer.Register(new TaskModule());
            DependencyContainer.BuildContainer();

            Assert.IsTrue(DependencyContainer.Container.IsRegistered<ILogEntryTasks>());
        }
    }
}
