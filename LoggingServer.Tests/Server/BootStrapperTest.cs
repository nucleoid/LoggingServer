﻿using LoggingServer.Interface.Autofac;
using LoggingServer.Interface.Controllers;
using LoggingServer.Server;
using LoggingServer.Server.Autofac;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using LoggingServer.Server.Tasks;
using MbUnit.Framework;
using NHibernate;

namespace LoggingServer.Tests.Server
{
    [TestFixture]
    public class BootStrapperTest
    {
        [SetUp]
        public void Setup()
        {
            DependencyContainer.Reset();
        }

        [Test]
        public void Start_Registers_Modules_And_Builds_Container()
        {
            //Act
            BootStrapper.Start();

            //Assert
            Assert.IsNotNull(DependencyContainer.Resolve<ISession>());
            Assert.IsNotNull(DependencyContainer.Resolve<IReadableRepository<LogEntry>>());
            Assert.IsNotNull(DependencyContainer.Resolve<LogReceiverServer>());
            Assert.IsNotNull(DependencyContainer.Resolve<ILogEntryTasks>());
        }

        [Test]
        public void Start_Assembly_Registers_All_Modules_And_Builds_Container()
        {
            //Act
            BootStrapper.Start(typeof(HomeController).Assembly);

            //Assert
            Assert.IsNotNull(DependencyContainer.Resolve<ISession>());
            Assert.IsNotNull(DependencyContainer.Resolve<IReadableRepository<LogEntry>>());
            Assert.IsNotNull(DependencyContainer.Resolve<LogReceiverServer>());
            Assert.IsNotNull(DependencyContainer.Resolve<ILogEntryTasks>());
            Assert.IsNotNull(DependencyContainer.Resolve<HomeController>());
        }

        [Test]
        public void Start_Assembly_Registers_Extras()
        {
            //Act
            BootStrapper.Start(typeof(HomeController).Assembly, new CustomTasksModule());

            //Assert
            Assert.IsNotNull(DependencyContainer.Resolve<IAuthenticationTasks>());
        }
    }
}
