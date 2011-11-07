using System;
using System.Collections.Generic;
using LoggingServer.Server.Domain;
using MbUnit.Framework;

namespace LoggingServer.Tests.Server.Repository.MappingIntegrationTests
{
    [TestFixture]
    public class SearchFilterTest : BaseMappingTest<SearchFilter>
    {
        [Test, Rollback]
        public void Properties_Are_Mapped()
        {
            //Arrange
            var now = DateTime.Parse("11/7/2011");
            var filter = new SearchFilter
            {
                ComponentName = "comps",
                StartDate = now,
                EndDate = now.AddDays(1),
                ExceptionPartial = "partially",
                IsGlobal = true,
                LogLevel = LogLevel.Error,
                MachineNamePartial = "turkey",
                MessagePartial = "msg partial",
                ProjectName = "projs",
                UserName = "Mitch"
            };

            //Act
            Repository.Save(filter);
            var postFilter = Repository.Get(filter.ID);

            //Assert
            Assert.AreEqual(filter.ID, postFilter.ID);
            Assert.AreEqual("comps", postFilter.ComponentName);
            Assert.AreEqual(now.Date, postFilter.StartDate.Value.Date);
            Assert.AreEqual(now.AddDays(1).Date, postFilter.EndDate.Value.Date);
            Assert.AreEqual("partially", postFilter.ExceptionPartial);
            Assert.IsTrue(postFilter.IsGlobal);
            Assert.AreEqual(LogLevel.Error, postFilter.LogLevel);
            Assert.AreEqual("turkey", postFilter.MachineNamePartial);
            Assert.AreEqual("msg partial", postFilter.MessagePartial);
            Assert.AreEqual("projs", postFilter.ProjectName);
            Assert.AreEqual("Mitch", postFilter.UserName);
        }
    }
}
