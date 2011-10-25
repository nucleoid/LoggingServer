using System;
using System.Collections.Generic;
using System.Linq;
using LoggingServer.Common.Extensions;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using LoggingServer.Server.Tasks;
using MbUnit.Framework;
using Rhino.Mocks;

namespace LoggingServer.Tests.Tasks
{
    [TestFixture]
    public class LogEntryTasksTest
    {
        private IReadableRepository<LogEntry> _logEntryRepository;
        private LogEntryTasks _tasks;
        private DateTime _now;

        [SetUp]
        public void Setup()
        {
            _logEntryRepository = MockRepository.GenerateMock<IReadableRepository<LogEntry>>();
            _tasks = new LogEntryTasks(_logEntryRepository);
            _now = DateTime.Parse("8/25/2011");
        }

        [Test]
        public void Get_With_Entity_Not_Found()
        {
            //Arrange
            var id = Guid.NewGuid();
            _logEntryRepository.Expect(x => x.Get(id)).Return(null);

            //Act
            var entry = _tasks.Get(id);

            //Assert
            Assert.IsNull(entry);
            _logEntryRepository.VerifyAllExpectations();
        }

        [Test]
        public void Get_With_Entity_Found()
        {
            //Arrange
            var id = Guid.NewGuid();
            _logEntryRepository.Expect(x => x.Get(id)).Return(new LogEntry {ID = id});

            //Act
            var entry = _tasks.Get(id);

            //Assert
            Assert.AreEqual(id, entry.ID);
            _logEntryRepository.VerifyAllExpectations();
        }

        [Test]
        public void Paged_With_Defaults_Does_Not_Filter()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(1, 10, null).ToList();

            //Assert
            Assert.AreEqual(10, results.Count);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000001"), results[0].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000002"), results[1].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000003"), results[2].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000004"), results[3].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000005"), results[4].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000006"), results[5].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000007"), results[6].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000008"), results[7].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000009"), results[8].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000010"), results[9].ID);
        }

        [Test]
        public void Paged_With_Page_Number_1()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(1, 5, null).ToList();

            //Assert
            Assert.AreEqual(5, results.Count);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000001"), results[0].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000002"), results[1].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000003"), results[2].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000004"), results[3].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000005"), results[4].ID);
        }

        [Test]
        public void Paged_With_Page_Number_2()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(2, 5, null).ToList();

            //Assert
            Assert.AreEqual(5, results.Count);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000006"), results[0].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000007"), results[1].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000008"), results[2].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000009"), results[3].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000010"), results[4].ID);
        }

        [Test]
        public void Paged_With_Page_Number_5()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(5, 5, null).ToList();

            //Assert
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void Paged_With_Page_Number_Negative()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(-1, 5, null).ToList();

            //Assert
            Assert.AreEqual(5, results.Count);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000001"), results[0].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000002"), results[1].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000003"), results[2].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000004"), results[3].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000005"), results[4].ID);
        }

        [Test]
        public void Paged_With_Page_Number_Null()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(null, 5, null).ToList();

            //Assert
            Assert.AreEqual(5, results.Count);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000001"), results[0].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000002"), results[1].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000003"), results[2].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000004"), results[3].ID);
            Assert.AreEqual(Guid.Parse("00000000-0000-0000-0000-000000000005"), results[4].ID);
        }

        [Test]
        [Row(1)]
        [Row(5)]
        public void Paged_With_Varying_Page_Size(int pageSize)
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(1, pageSize, null).ToList();

            //Assert
            Assert.AreEqual(pageSize, results.Count);
        }

        [Test]
        public void Paged_With_Negative_Page_Size_Returns_Default()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(1, -1, null).ToList();

            //Assert
            Assert.AreEqual(16, results.Count); //default is 500, but we only have 16 entries
        }

        [Test]
        public void Paged_With_Page_Size_Null_Returns_Default()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(1, null, null).ToList();

            //Assert
            Assert.AreEqual(16, results.Count); //default is 500, but we only have 16 entries
        }

        [Test]
        public void Paged_With_Page_Size_Too_Large_Returns_All()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(1, 30, null).ToList();

            //Assert
            Assert.AreEqual(16, results.Count);
        }

        [Test]
        public void Paged_With_ProjectName_Filter_Filters_Using_Case_Insensitive()
        {
            //Arrange
            const string name = "sWeEt";
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(1, 5, new SearchFilter { ProjectName = name }).ToList();

            //Assert
            Assert.AreEqual(2, results.Count);
            Assert.ForAll(results, x => x.Component.Project.Name == name.ToLowerInvariant());
        }

        [Test]
        public void Paged_With_ComponentName_Filter_Filters_Using_Case_Insensitive()
        {
            //Arrange
            const string name = "TREATS";
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(1, 5, new SearchFilter { ComponentName = name }).ToList();

            //Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(name.ToLowerInvariant(), results.SingleOrDefault().Component.Name);
        }

        [Test]
        public void Paged_With_StartDate_Filter_Uses_Date_Inclusive()
        {
            //Arrange
            var date = _now.AddDays(8);
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(1, 5, new SearchFilter { StartDate = date }).ToList();

            //Assert
            Assert.AreEqual(3, results.Count);
            Assert.ForAll(results, x => x.DateAdded >= _now.AddDays(8));
        }

        [Test]
        public void Paged_With_EndDate_Filter_Uses_Date_Inclusive()
        {
            //Arrange
            var date = _now.AddDays(5);
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(1, 15, new SearchFilter { EndDate = date }).ToList();

            //Assert
            Assert.AreEqual(13, results.Count);
            Assert.ForAll(results, x => x.DateAdded <= date);
        }

        [Test]
        public void Paged_With_Start_And_End_DateRange_Filter()
        {
            //Arrange
            var startDate = _now.AddDays(2);
            var endDate = _now.AddDays(9);
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(1, 15, new SearchFilter { StartDate = startDate, EndDate = endDate }).ToList();

            //Assert
            Assert.AreEqual(2, results.Count);
            Assert.ForAll(results, x => x.DateAdded >= startDate && x.DateAdded <= endDate);
        }

        [Test]
        public void Paged_With_LogLevel_Filter()
        {
            //Arrange
            const LogLevel logLevel = LogLevel.Debug | LogLevel.Error;
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(1, 15, new SearchFilter { LogLevel = logLevel }).ToList();

            //Assert
            Assert.AreEqual(14, results.Count);
            Assert.ForAll(results, x => x.LogLevel.TestFor(LogLevel.Debug) || x.LogLevel.TestFor(LogLevel.Error));
        }

        [Test]
        public void Paged_With_MachineNamePartial_Filter_Filters_Using_Case_Insensitive()
        {
            //Arrange
            const string namePartial = "scrap";
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(1, 15, new SearchFilter { MachineNamePartial = namePartial }).ToList();

            //Assert
            Assert.AreEqual(13, results.Count);
            Assert.ForAll(results, x => x.MachineName.ToLowerInvariant().Contains(namePartial));
        }

        [Test]
        public void Paged_With_ExceptionPartial_Filter_Filters_Using_Case_Insensitive()
        {
            //Arrange
            const string exceptionPartial = "didn't do that";
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(1, 15, new SearchFilter { ExceptionPartial = exceptionPartial }).ToList();

            //Assert
            Assert.AreEqual(14, results.Count);
            Assert.ForAll(results, x => x.ExceptionMessage.ToLowerInvariant().Contains(exceptionPartial));
        }

        [Test]
        public void Paged_With_MessagePartial_Filter_Filters_Using_Case_Insensitive()
        {
            //Arrange
            const string messagePartial = "something went";
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(1, 15, new SearchFilter { MessagePartial = messagePartial }).ToList();

            //Assert
            Assert.AreEqual(14, results.Count);
            Assert.ForAll(results, x => x.LogMessage.ToLowerInvariant().Contains(messagePartial));
        }

        [Test]
        public void Paged_With_Full_Filter_Filters_Everything()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(GenerateLogEntries());

            //Act
            var results = _tasks.Paged(1, 15, new SearchFilter
            {
                ProjectName = "Shattered",
                ComponentName = "Dreams",
                StartDate = _now,
                EndDate = _now,
                LogLevel = LogLevel.Error,
                ExceptionPartial = "didn't do that one thing",
                MachineNamePartial = "scrappyJoe",
                MessagePartial = "something went wrong!"
            }).ToList();

            //Assert
            Assert.AreEqual(1, results.Count);
            var result = results.SingleOrDefault();
            Assert.AreEqual("shattered", result.Component.Project.Name);
            Assert.AreEqual("dreams", result.Component.Name);
            Assert.AreEqual(_now, result.DateAdded);
            Assert.AreEqual(LogLevel.Error, result.LogLevel);
            Assert.Contains(result.ExceptionMessage.ToLowerInvariant(), "didn't do that one thing");
            Assert.Contains(result.MachineName.ToLowerInvariant(), "scrappyjoe");
            Assert.Contains(result.LogMessage.ToLowerInvariant(), "something went wrong!");
        }

        /// <summary>
        /// Return 16 entries
        /// </summary>
        private IQueryable<LogEntry> GenerateLogEntries()
        {
            var project = new Project {Name = "shattered"};
            var otherProject = new Project {Name = "sweet"};
            var dreamComponent = new Component {Name = "dreams", Project = project};
            var treatComponent = new Component {Name = "treats", Project = project};
            var danceComponent = new Component {Name = "dance", Project = otherProject};
            return new List<LogEntry>
            {
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000001"), Component = dreamComponent, DateAdded = _now, LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000002"), Component = treatComponent, DateAdded = _now, LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000003"), Component = danceComponent, DateAdded = _now, LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000004"), Component = dreamComponent, DateAdded = _now.AddDays(5), LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000005"), Component = dreamComponent, DateAdded = _now.AddDays(8), LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000006"), Component = dreamComponent, DateAdded = _now.AddDays(10), LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000007"), Component = dreamComponent, DateAdded = _now, LogLevel = LogLevel.Debug, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000008"), Component = dreamComponent, DateAdded = _now, LogLevel = LogLevel.Fatal, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000009"), Component = dreamComponent, DateAdded = _now, LogLevel = LogLevel.Error, MachineName = "ScrappyJake", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000010"), Component = dreamComponent, DateAdded = _now, LogLevel = LogLevel.Error, MachineName = "LazyLou", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000011"), Component = dreamComponent, DateAdded = _now, LogLevel = LogLevel.Error, MachineName = "AbrahamTheUgly", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000012"), Component = dreamComponent, DateAdded = _now, LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that other thing", LogMessage = "Something went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000013"), Component = dreamComponent, DateAdded = _now, LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Performed hate crime", LogMessage = "Something Went wrong!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000014"), Component = dreamComponent, DateAdded = _now, LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "Something went right!"},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000015"), Component = dreamComponent, DateAdded = _now, LogLevel = LogLevel.Error, MachineName = "ScrappyJoe", 
                    ExceptionMessage = "Didn't do that one thing", LogMessage = "You're doing it wrong bro..."},
                new LogEntry {ID =  Guid.Parse("00000000-0000-0000-0000-000000000016"), Component = danceComponent, DateAdded = _now.AddDays(11), LogLevel = LogLevel.Info, MachineName = "BoPeep", 
                    ExceptionMessage = "Ain't a thing", LogMessage = "It's all good"},
            }.AsQueryable();
        }
    }
}
