
using System;
using System.Collections.Generic;
using System.Linq;
using LoggingServer.Server.Domain;

namespace LoggingServer.Tests.Server
{
    public static class DomainTestHelper
    {
        /// <summary>
        /// Return 16 entries
        /// </summary>
        public static IQueryable<LogEntry> GenerateLogEntries(DateTime now)
        {
            var project = new Project { Name = "shattered" };
            var otherProject = new Project { Name = "sweet" };
            var dreamComponent = new Component { Name = "dreams", Project = project };
            var treatComponent = new Component { Name = "treats", Project = project };
            var danceComponent = new Component { Name = "dance", Project = otherProject };
            return new List<LogEntry>
            {
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000001"), Component = dreamComponent, DateAdded = now, LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000002"), Component = treatComponent, DateAdded = now, LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000003"), Component = danceComponent, DateAdded = now, LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000004"), Component = dreamComponent, DateAdded = now.AddDays(5), LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000005"), Component = dreamComponent, DateAdded = now.AddDays(8), LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000006"), Component = dreamComponent, DateAdded = now.AddDays(10), LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000007"), Component = dreamComponent, DateAdded = now, LogLevel = LogLevel.Debug, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000008"), Component = dreamComponent, DateAdded = now, LogLevel = LogLevel.Fatal, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000009"), Component = dreamComponent, DateAdded = now, LogLevel = LogLevel.Error, MachineName = "ScrappyJake", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000010"), Component = dreamComponent, DateAdded = now, LogLevel = LogLevel.Error, MachineName = "LazyLou", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000011"), Component = dreamComponent, DateAdded = now, LogLevel = LogLevel.Error, MachineName = "AbrahamTheUgly", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000012"), Component = dreamComponent, DateAdded = now, LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that other thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000013"), Component = dreamComponent, DateAdded = now, LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Performed hate crime", LogMessage = "Something Went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000014"), Component = dreamComponent, DateAdded = now, LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went right!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000015"), Component = dreamComponent, DateAdded = now, LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "You're doing it wrong bro..."},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000016"), Component = danceComponent, DateAdded = now.AddDays(11), LogLevel = LogLevel.Info, MachineName = "BoPeep", 
                    ExceptionMessage = "Ain't a thing", LogMessage = "It's all good"},
            }.AsQueryable();
        }
    }
}
