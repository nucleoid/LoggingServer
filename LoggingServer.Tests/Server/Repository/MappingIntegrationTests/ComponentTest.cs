using System;
using System.Collections.Generic;
using LoggingServer.Server.Autofac;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using MbUnit.Framework;

namespace LoggingServer.Tests.Server.Repository.MappingIntegrationTests
{
    [TestFixture]
    public class ComponentTest : BaseMappingTest<Component>
    {
        [Test, Rollback]
        public void Properties_Are_Mapped()
        {
            //Arrange
            var now = DateTime.Parse("11/7/2011");
            var project = new Project {DateAdded = now};
            var projectRepo = DependencyContainer.Resolve<IWritableRepository<Project>>();
            projectRepo.Save(project);
            var component = new Component
            {
                ID = Guid.NewGuid(),
                Name = "compy",
                Description = "comps",
                DateAdded = now,
                Project = project,
                LogEntries = new List<LogEntry>()
            };
            component.LogEntries.Add(new LogEntry { DateAdded = now, LongDate = now });
            component.LogEntries.Add(new LogEntry { DateAdded = now, LongDate = now });

            //Act
            Repository.Save(component);
            var postComponent = Repository.Get(component.ID);

            //Assert
            Assert.AreEqual(component.ID, postComponent.ID);
            Assert.AreEqual("compy", postComponent.Name);
            Assert.AreEqual("comps", postComponent.Description);
            Assert.AreEqual(now.Date, postComponent.DateAdded.Date);
            Assert.AreEqual(project.ID, postComponent.Project.ID);
            Assert.AreEqual(2, postComponent.LogEntries.Count);
            Assert.AreEqual(1, postComponent.Version);
        }
    }
}
