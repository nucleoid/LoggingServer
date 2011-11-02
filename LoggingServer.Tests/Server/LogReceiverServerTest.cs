using System;
using System.Collections.Generic;
using System.Reflection;
using LoggingServer.Server;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using LoggingServer.Server.Tasks;
using MbUnit.Framework;
using NLog.LogReceiverService;
using Rhino.Mocks;
using System.Linq;

namespace LoggingServer.Tests.Server
{
    [TestFixture]
    public class LogReceiverServerTest
    {
        private IWritableRepository<LogEntry> _logEntryRepository;
        private IWritableRepository<Component> _componentRepository;
        private IWritableRepository<Project> _projectRepository;
        private ISubscriptionTasks _subscriptionTasks;
        private LogReceiverServer _server;

        [SetUp]
        public void Setup()
        {
            _logEntryRepository = MockRepository.GenerateMock<IWritableRepository<LogEntry>>();
            _componentRepository = MockRepository.GenerateMock<IWritableRepository<Component>>();
            _projectRepository = MockRepository.GenerateMock<IWritableRepository<Project>>();
            _subscriptionTasks = MockRepository.GenerateMock<ISubscriptionTasks>();
            _server = new LogReceiverServer(_logEntryRepository, _componentRepository, _projectRepository, _subscriptionTasks);
        }

        [Test]
        public void ProcessLogMessages_Sets_All_Default_LogEntry_Properties()
        {
            //Arrange
            var now = DateTime.Parse("10/18/2011");
            var component = new Component { ID = Guid.NewGuid(), Name = "Sorrow", Description = "Foghorns"};
            var entry = new LogEntry { BaseDirectory = "sploosh", CallSite = "rest", Counter = 3,
                DateAdded = now, EntryAssemblyCompany = "Longhorn", EntryAssemblyDescription = "Foghorns", EntryAssemblyGuid = component.ID,
                EntryAssemblyProduct = "Constant", EntryAssemblyTitle = "Sorrow", EntryAssemblyVersion = "1.4", EnvironmentKey = "Yellow, not Green",
                ExceptionMessage = "These are NOT the droids you are looking for", ExceptionMethod = "BackFlip", ExceptionStackTrace = "from railing",
                ExceptionString = "excepted", ExceptionType = "UnAuthorizedBackflipping", ID = Guid.NewGuid(), LogID = Guid.NewGuid(), LogIdentity = "Sky",
                LogLevel = LogLevel.Fatal, LogMessage = "Attraction", Logger = "The second one", LongDate = now.AddDays(1), MachineName = "Mr. Wiggles",
                NotificationsQueued = true, ProcessID = "The third one", ProcessInfo = "ideapad", ProcessName = "svchost.exe", ProcessTime = TimeSpan.FromHours(4),
                StackTrace = "Someone getting the best of you", ThreadID = "That other one", ThreadName = "That other 1", Version = 56, 
                WindowsIdentity = "Emperor", Component = component};
            var logEvent = new NLogEvent { Values = "0|1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35" };
            var events = new NLogEvents { Events = new[] { logEvent } };
            events.LayoutNames = new StringCollection { "BaseDirectory", "CallSite", "Counter", "DateAdded", 
                "EntryAssemblyCompany", "EntryAssemblyDescription", "EntryAssemblyGuid", "EntryAssemblyProduct", "EntryAssemblyTitle", "EntryAssemblyVersion", 
                "EnvironmentKey", "ExceptionMessage", "ExceptionMethod", "ExceptionStackTrace", "ExceptionString", "ExceptionType", "ID", "LogID", 
                "LogIdentity", "LogLevel", "LogMessage", "Logger", "LongDate", "MachineName", "NotificationsQueued", "ProcessID", "ProcessInfo", 
                "ProcessName", "ProcessTime", "StackTrace", "ThreadID", "ThreadName", "Version", "WindowsIdentity"};
            events.Strings = new StringCollection {entry.BaseDirectory, entry.CallSite,
                entry.Counter.ToString(), entry.DateAdded.ToString(), entry.EntryAssemblyCompany, entry.EntryAssemblyDescription, entry.EntryAssemblyGuid.ToString(),
                entry.EntryAssemblyProduct, entry.EntryAssemblyTitle, entry.EntryAssemblyVersion, entry.EnvironmentKey, entry.ExceptionMessage,
                entry.ExceptionMethod, entry.ExceptionStackTrace, entry.ExceptionString, entry.ExceptionType, entry.ID.ToString(), entry.LogID.ToString(),
                entry.LogIdentity, entry.LogLevel.ToString(), entry.LogMessage, entry.Logger, entry.LongDate.ToString(), entry.MachineName, 
                entry.NotificationsQueued.ToString(), entry.ProcessID, entry.ProcessInfo, entry.ProcessName, entry.ProcessTime.ToString(), entry.StackTrace,
                entry.ThreadID, entry.ThreadName, entry.Version.ToString(), entry.WindowsIdentity};

            _componentRepository.Expect(x => x.Get(Arg<Guid>.Is.Equal(component.ID))).Return(component);
            _logEntryRepository.Expect(x => x.Save(Arg<List<LogEntry>>.Matches(y => y.Count == 1 &&
                y.First().Equals(entry))));
            _subscriptionTasks.Expect(x => x.AsyncNotify(Arg<List<LogEntry>>.Matches(y => y.Count == 1 &&
                y.First().Equals(entry))));

            //Act
            _server.ProcessLogMessages(events);

            //Assert
            _logEntryRepository.VerifyAllExpectations();
            _componentRepository.VerifyAllExpectations();
            _subscriptionTasks.VerifyAllExpectations();
        }

        [Test]
        public void ProcessLogMessages_Sets_LogEntry_Component()
        {
            //Arrange
            object[] guidObjects = typeof(LogReceiverServerTest).Assembly.GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
            string assemblyTitle = (typeof(LogReceiverServerTest).Assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute).Title;
            string assemblyDescription = (typeof(LogReceiverServerTest).Assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0] as AssemblyDescriptionAttribute).Description;
            string assemblyGuid = ((System.Runtime.InteropServices.GuidAttribute) guidObjects[0]).Value;
            var entry = new LogEntry
            {
                EntryAssemblyGuid = new Guid(assemblyGuid), 
                EntryAssemblyTitle = assemblyTitle, 
                EntryAssemblyDescription = assemblyDescription
            };
            var logEvent = new NLogEvent { Values = "0|1|2|3" };
            var events = new NLogEvents { Events = new[] { logEvent } };
            events.LayoutNames = new StringCollection { "EntryAssemblyGuid", "EntryAssemblyTitle", "EntryAssemblyDescription" };
            events.Strings = new StringCollection { entry.EntryAssemblyGuid.ToString(), entry.EntryAssemblyTitle, entry.EntryAssemblyDescription };

            _componentRepository.Expect(x => x.Get(Arg<Guid>.Is.Equal(entry.EntryAssemblyGuid))).Return(null);
            var projectTitle = assemblyTitle.Split('.')[0];
            _projectRepository.Expect(x => x.All()).Return(new List<Project>{new Project {Name = projectTitle}}.AsQueryable());
            _componentRepository.Expect(x => x.Save(Arg<Component>.Matches(y => y.ID.ToString() == assemblyGuid && y.Name == assemblyTitle &&
                y.Description == assemblyDescription)));
            _logEntryRepository.Expect(x => x.Save(Arg<List<LogEntry>>.Matches(y => y.Count == 1 &&
                y.First().Component.ID.ToString() == assemblyGuid)));
            
            //Act
            _server.ProcessLogMessages(events);

            //Assert
            _logEntryRepository.VerifyAllExpectations();
            _componentRepository.VerifyAllExpectations();
        }

        [Test]
        public void ProcessLogMessages_Updates_LogEntry_Component()
        {
            //Arrange
            object[] guidObjects = typeof(LogReceiverServerTest).Assembly.GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
            string assemblyTitle = (typeof(LogReceiverServerTest).Assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute).Title;
            string assemblyDescription = (typeof(LogReceiverServerTest).Assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0] as AssemblyDescriptionAttribute).Description;
            string assemblyGuid = ((System.Runtime.InteropServices.GuidAttribute)guidObjects[0]).Value;
            var entry = new LogEntry
            {
                EntryAssemblyGuid = new Guid(assemblyGuid),
                EntryAssemblyTitle = assemblyTitle,
                EntryAssemblyDescription = assemblyDescription
            };
            var logEvent = new NLogEvent { Values = "0|1|2|3" };
            var events = new NLogEvents { Events = new[] { logEvent } };
            events.LayoutNames = new StringCollection { "EntryAssemblyGuid", "EntryAssemblyTitle", "EntryAssemblyDescription" };
            events.Strings = new StringCollection { entry.EntryAssemblyGuid.ToString(), entry.EntryAssemblyTitle, entry.EntryAssemblyDescription };
            var component = new Component {ID = new Guid(assemblyGuid)};

            _componentRepository.Expect(x => x.Get(Arg<Guid>.Is.Equal(component.ID))).Return(component);
            _componentRepository.Expect(x => x.Save(Arg<Component>.Matches(y => y.ID.ToString() == assemblyGuid && y.Name == assemblyTitle &&
                y.Description == assemblyDescription)));
            var projectTitle = assemblyTitle.Split('.')[0];
            _projectRepository.Expect(x => x.All()).Return(new List<Project> { new Project { Name = projectTitle } }.AsQueryable());
            _logEntryRepository.Expect(x => x.Save(Arg<List<LogEntry>>.Matches(y => y.Count == 1 &&
                y.First().Component.ID.ToString() == assemblyGuid)));


            //Act
            _server.ProcessLogMessages(events);

            //Assert
            _logEntryRepository.VerifyAllExpectations();
            _componentRepository.VerifyAllExpectations();
        }

        [Test]
        public void ProcessLogMessages_Sets_Component_Project()
        {
            //Arrange
            object[] guidObjects = typeof(LogReceiverServerTest).Assembly.GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
            string assemblyTitle = (typeof(LogReceiverServerTest).Assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute).Title;
            string assemblyDescription = (typeof(LogReceiverServerTest).Assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0] as AssemblyDescriptionAttribute).Description;
            string assemblyGuid = ((System.Runtime.InteropServices.GuidAttribute)guidObjects[0]).Value;
            var entry = new LogEntry
            {
                EntryAssemblyGuid = new Guid(assemblyGuid),
                EntryAssemblyTitle = assemblyTitle,
                EntryAssemblyDescription = assemblyDescription
            };
            var logEvent = new NLogEvent { Values = "0|1|2|3" };
            var events = new NLogEvents { Events = new[] { logEvent } };
            events.LayoutNames = new StringCollection { "EntryAssemblyGuid", "EntryAssemblyTitle", "EntryAssemblyDescription" };
            events.Strings = new StringCollection { entry.EntryAssemblyGuid.ToString(), entry.EntryAssemblyTitle, entry.EntryAssemblyDescription };

            var projectTitle = assemblyTitle.Split('.')[0];

            _componentRepository.Expect(x => x.Get(Arg<Guid>.Is.Equal(entry.EntryAssemblyGuid))).Return(null);
            _projectRepository.Expect(x => x.All()).Return(new List<Project>().AsQueryable());
            _projectRepository.Expect(x => x.Save(Arg<Project>.Matches(y => y.Name == projectTitle)));
            _componentRepository.Expect(x => x.Save(Arg<Component>.Matches(y => y.Project.Name == projectTitle)));
            _logEntryRepository.Expect(x => x.Save(Arg<List<LogEntry>>.Matches(y => y.Count == 1 &&
                y.First().Component.ID.ToString() == assemblyGuid)));

            //Act
            _server.ProcessLogMessages(events);

            //Assert
            _logEntryRepository.VerifyAllExpectations();
            _componentRepository.VerifyAllExpectations();
            _projectRepository.VerifyAllExpectations();
        }

        [Test]
        public void ProcessLogMessages_Updates_LogEntry_Component_Project()
        {
            //Arrange
            object[] guidObjects = typeof(LogReceiverServerTest).Assembly.GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
            string assemblyTitle = (typeof(LogReceiverServerTest).Assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute).Title;
            string assemblyDescription = (typeof(LogReceiverServerTest).Assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0] as AssemblyDescriptionAttribute).Description;
            string assemblyGuid = ((System.Runtime.InteropServices.GuidAttribute)guidObjects[0]).Value;
            var entry = new LogEntry
            {
                EntryAssemblyGuid = new Guid(assemblyGuid),
                EntryAssemblyTitle = assemblyTitle,
                EntryAssemblyDescription = assemblyDescription
            };
            var logEvent = new NLogEvent { Values = "0|1|2|3" };
            var events = new NLogEvents { Events = new[] { logEvent } };
            events.LayoutNames = new StringCollection { "EntryAssemblyGuid", "EntryAssemblyTitle", "EntryAssemblyDescription" };
            events.Strings = new StringCollection { entry.EntryAssemblyGuid.ToString(), entry.EntryAssemblyTitle, entry.EntryAssemblyDescription };
            var component = new Component { ID = new Guid(assemblyGuid), Project = new Project { Name = "otherProject" } };

            _componentRepository.Expect(x => x.Get(Arg<Guid>.Is.Equal(component.ID))).Return(component);
            _projectRepository.Expect(x => x.All()).Return(new List<Project>().AsQueryable());
            var projectTitle = assemblyTitle.Split('.')[0];
            _projectRepository.Expect(x => x.Save(Arg<Project>.Matches(y => y.Name == projectTitle)));
            _componentRepository.Expect(x => x.Save(Arg<Component>.Matches(y => y.ID.ToString() == assemblyGuid && y.Name == assemblyTitle &&
                y.Description == assemblyDescription && y.Project.Name == projectTitle)));
            _logEntryRepository.Expect(x => x.Save(Arg<List<LogEntry>>.Matches(y => y.Count == 1 &&
                y.First().Component.ID.ToString() == assemblyGuid)));

            //Act
            _server.ProcessLogMessages(events);

            //Assert
            _logEntryRepository.VerifyAllExpectations();
            _componentRepository.VerifyAllExpectations();
            _projectRepository.VerifyAllExpectations();
        }
    }
}
