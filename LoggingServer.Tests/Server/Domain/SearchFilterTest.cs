using System;
using LoggingServer.Server.Domain;
using MbUnit.Framework;

namespace LoggingServer.Tests.Server.Domain
{
    [TestFixture]
    public class SearchFilterTest
    {
        [Test]
        public void Description_Minimum()
        {
            //Arrange
            var filter = new SearchFilter { ID = Guid.NewGuid() };

            //Act
            var description = filter.Description;

            //Assert
            Assert.AreEqual("All - Global: False", description);
        }

        [Test]
        public void Description_Global_Minimum()
        {
            //Arrange
            var filter = new SearchFilter { ID = Guid.NewGuid(), IsGlobal = true };

            //Act
            var description = filter.Description;

            //Assert
            Assert.AreEqual("All - Global: True", description);
        }

        [Test]
        public void Description_Full()
        {
            //Arrange
            var now = DateTime.Parse("11/1/2011");
            var filter = new SearchFilter
            {
                ID = Guid.NewGuid(),
                ProjectName = "LoggingServer",
                ComponentName = "LoggingServer.Server",
                UserName = "Mitch",
                LogLevel = LogLevel.Debug | LogLevel.Error, //tostring orders by ordinal value
                StartDate = now.AddDays(-5),
                EndDate = now,
                ExceptionPartial = "throws",
                MessagePartial = "uh oh",
                MachineNamePartial = "TroyMcClure",
                IsGlobal = true
            };

            //Act
            var description = filter.Description;

            //Assert
            Assert.AreEqual("Project: LoggingServer - Component: LoggingServer.Server - User: Mitch - Log Level: Error, Debug - Start: 10/27/2011 12:00:00 AM - End: 11/1/2011 12:00:00 AM - Exception: throws - Message: uh oh - Machine Name: TroyMcClure - Global: True", description);
        }
    }
}
