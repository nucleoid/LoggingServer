using System;
using System.Collections.Generic;
using LoggingServer.Server.Domain;
using MbUnit.Framework;

namespace LoggingServer.Tests.Server.Repository.MappingIntegrationTests
{
    [TestFixture]
    public class ProjectTest : BaseMappingTest<Project>
    {
        [Test, Rollback]
        public void Properties_Are_Mapped()
        {
            //Arrange
            var now = DateTime.Parse("11/7/2011");
            var project = new Project
            {
                ID = Guid.NewGuid(),
                Name = "projy",
                Description = "projs",
                DateAdded = now,
                Components = new List<Component>()
            };
            project.Components.Add(new Component { ID = Guid.NewGuid(), DateAdded = now });
            project.Components.Add(new Component { ID = Guid.NewGuid(), DateAdded = now });

            //Act
            Repository.Save(project);
            var postProject = Repository.Get(project.ID);

            //Assert
            Assert.AreEqual(project.ID, postProject.ID);
            Assert.AreEqual("projy", postProject.Name);
            Assert.AreEqual("projs", postProject.Description);
            Assert.AreEqual(now.Date, postProject.DateAdded.Date);
            Assert.AreEqual(2, postProject.Components.Count);
            Assert.AreEqual(1, postProject.Version);
        }
    }
}
