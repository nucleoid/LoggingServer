using System;
using LoggingServer.Server.Autofac;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using MbUnit.Framework;

namespace LoggingServer.Tests.Server.Repository.MappingIntegrationTests
{
    [TestFixture]
    public class LogEntryTest : BaseMappingTest<LogEntry>
    {
        [Test, Rollback]
        public void Properties_Are_Mapped()
        {
            //Arrange
            var now = DateTime.Parse("11/7/2011");
            var component = new Component { ID = Guid.NewGuid(), DateAdded = now };
            var componentRepo = DependencyContainer.Resolve<IWritableRepository<Component>>();
            componentRepo.Save(component);
            var logEntry = new LogEntry
            {
                BaseDirectory = "baseDir",
                CallSite = "caller",
                Component = component,
                Counter = 4,
                DateAdded = now,
                EntryAssemblyCompany = "entryAssemblyComp",
                EntryAssemblyDescription = "entryAssemblyDesc",
                EntryAssemblyGuid = Guid.NewGuid(),
                EntryAssemblyProduct = "entryAssemblyProd",
                EntryAssemblyTitle = "entryAssemblyTitl",
                EntryAssemblyVersion = "entryAssemblyVers",
                EnvironmentKey = "dev",
                ExceptionMessage = "oopsy",
                ExceptionMethod = "oops()",
                ExceptionStackTrace = "somewhere",
                ExceptionString = "all oops",
                ExceptionType = "type - oops",
                LogID = Guid.NewGuid(),
                LogIdentity = "ident",
                LogLevel = LogLevel.Fatal,
                LogMessage = "blah blah",
                Logger = "this one",
                LongDate = now,
                MachineName = "turk",
                ProcessID = "procs",
                ProcessInfo = "infos",
                ProcessName = "proccy",
                ProcessTime = TimeSpan.FromMilliseconds(5),
                StackTrace = "all the way",
                ThreadID = "543",
                ThreadName = "this 543",
                WindowsIdentity = "Mitch"
            };

            //Act
            Repository.Save(logEntry);
            var postLogEntry = Repository.Get(logEntry.ID);

            Assert.AreEqual(logEntry.ID, postLogEntry.ID);
            Assert.AreEqual("baseDir", postLogEntry.BaseDirectory);
            Assert.AreEqual("caller", postLogEntry.CallSite);
            Assert.AreEqual(component.ID, postLogEntry.Component.ID);
            Assert.AreEqual(4, postLogEntry.Counter);
            Assert.AreEqual(now.Date, postLogEntry.DateAdded.Date);
            Assert.AreEqual("entryAssemblyComp", postLogEntry.EntryAssemblyCompany);
            Assert.AreEqual("entryAssemblyDesc", postLogEntry.EntryAssemblyDescription);
            Assert.AreEqual(logEntry.EntryAssemblyGuid, postLogEntry.EntryAssemblyGuid);
            Assert.AreEqual("entryAssemblyProd", postLogEntry.EntryAssemblyProduct);
            Assert.AreEqual("entryAssemblyTitl", postLogEntry.EntryAssemblyTitle);
            Assert.AreEqual("entryAssemblyVers", postLogEntry.EntryAssemblyVersion);
            Assert.AreEqual("dev", postLogEntry.EnvironmentKey);
            Assert.AreEqual("oopsy", postLogEntry.ExceptionMessage);
            Assert.AreEqual("oops()", postLogEntry.ExceptionMethod);
            Assert.AreEqual("somewhere", postLogEntry.ExceptionStackTrace);
            Assert.AreEqual("all oops", postLogEntry.ExceptionString);
            Assert.AreEqual("type - oops", postLogEntry.ExceptionType);
            Assert.AreEqual(logEntry.LogID, postLogEntry.LogID);
            Assert.AreEqual("ident", postLogEntry.LogIdentity);
            Assert.AreEqual(LogLevel.Fatal, postLogEntry.LogLevel);
            Assert.AreEqual("blah blah", postLogEntry.LogMessage);
            Assert.AreEqual("this one", postLogEntry.Logger);
            Assert.AreEqual(now.Date, postLogEntry.LongDate);
            Assert.AreEqual("turk", postLogEntry.MachineName);
            Assert.AreEqual("procs", postLogEntry.ProcessID);
            Assert.AreEqual("infos", postLogEntry.ProcessInfo);
            Assert.AreEqual("proccy", postLogEntry.ProcessName);
            Assert.AreEqual(TimeSpan.FromMilliseconds(5), postLogEntry.ProcessTime);
            Assert.AreEqual("all the way", postLogEntry.StackTrace);
            Assert.AreEqual("543", postLogEntry.ThreadID);
            Assert.AreEqual("this 543", postLogEntry.ThreadName);
            Assert.AreEqual("Mitch", postLogEntry.WindowsIdentity);
            Assert.AreEqual(1, logEntry.Version);
        }
    }
}
