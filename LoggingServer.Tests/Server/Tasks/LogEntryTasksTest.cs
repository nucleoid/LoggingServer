using System;
using System.Collections.Generic;
using System.Linq;
using LoggingServer.Common.Extensions;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using LoggingServer.Server.Tasks;
using MbUnit.Framework;
using Rhino.Mocks;

namespace LoggingServer.Tests.Server.Tasks
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
            _logEntryRepository.Expect(x => x.All()).Return(DomainTestHelper.GenerateLogEntries(_now));

            //Act
            var results = _tasks.Paged(1, 10, null).ToList();

            //Assert
            Assert.AreEqual(10, results.Count);
            Assert.AreEqual(_now.AddDays(11), results[0].DateAdded);
            Assert.AreEqual(_now.AddDays(10), results[1].DateAdded);
            Assert.AreEqual(_now.AddDays(8), results[2].DateAdded);
            Assert.AreEqual(_now.AddDays(5), results[3].DateAdded);
            Assert.ForAll(results.Skip(4), x => x.DateAdded == _now);
        }

        [Test]
        public void Paged_With_Page_Number_1()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(DomainTestHelper.GenerateLogEntries(_now));

            //Act
            var results = _tasks.Paged(1, 5, null).ToList();

            //Assert
            Assert.AreEqual(5, results.Count);
            Assert.AreEqual(_now.AddDays(11), results[0].DateAdded);
            Assert.AreEqual(_now.AddDays(10), results[1].DateAdded);
            Assert.AreEqual(_now.AddDays(8), results[2].DateAdded);
            Assert.AreEqual(_now.AddDays(5), results[3].DateAdded);
            Assert.AreEqual(_now, results[4].DateAdded);
        }

        [Test]
        public void Paged_With_Page_Number_2()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(DomainTestHelper.GenerateLogEntries(_now));

            //Act
            var results = _tasks.Paged(2, 5, null).ToList();

            //Assert
            Assert.AreEqual(5, results.Count);
            Assert.ForAll(results.Skip(5), x => x.DateAdded == _now);
        }

        [Test]
        public void Paged_With_Page_Number_5()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(DomainTestHelper.GenerateLogEntries(_now));

            //Act
            var results = _tasks.Paged(5, 5, null).ToList();

            //Assert
            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void Paged_With_Page_Number_Negative()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(DomainTestHelper.GenerateLogEntries(_now));

            //Act
            var results = _tasks.Paged(-1, 5, null).ToList();

            //Assert
            Assert.AreEqual(5, results.Count);
            Assert.AreEqual(_now.AddDays(11), results[0].DateAdded);
            Assert.AreEqual(_now.AddDays(10), results[1].DateAdded);
            Assert.AreEqual(_now.AddDays(8), results[2].DateAdded);
            Assert.AreEqual(_now.AddDays(5), results[3].DateAdded);
            Assert.AreEqual(_now, results[4].DateAdded);
        }

        [Test]
        public void Paged_With_Page_Number_Null()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(DomainTestHelper.GenerateLogEntries(_now));

            //Act
            var results = _tasks.Paged(null, 5, null).ToList();

            //Assert
            Assert.AreEqual(5, results.Count);
            Assert.AreEqual(_now.AddDays(11), results[0].DateAdded);
            Assert.AreEqual(_now.AddDays(10), results[1].DateAdded);
            Assert.AreEqual(_now.AddDays(8), results[2].DateAdded);
            Assert.AreEqual(_now.AddDays(5), results[3].DateAdded);
            Assert.AreEqual(_now, results[4].DateAdded);
        }

        [Test]
        [Row(1)]
        [Row(5)]
        public void Paged_With_Varying_Page_Size(int pageSize)
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(DomainTestHelper.GenerateLogEntries(_now));

            //Act
            var results = _tasks.Paged(1, pageSize, null).ToList();

            //Assert
            Assert.AreEqual(pageSize, results.Count);
        }

        [Test]
        public void Paged_With_Negative_Page_Size_Returns_Default()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(DomainTestHelper.GenerateLogEntries(_now));

            //Act
            var results = _tasks.Paged(1, -1, null).ToList();

            //Assert
            Assert.AreEqual(16, results.Count); //default is 500, but we only have 16 entries
        }

        [Test]
        public void Paged_With_Page_Size_Null_Returns_Default()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(DomainTestHelper.GenerateLogEntries(_now));

            //Act
            var results = _tasks.Paged(1, null, null).ToList();

            //Assert
            Assert.AreEqual(16, results.Count); //default is 500, but we only have 16 entries
        }

        [Test]
        public void Paged_With_Page_Size_Too_Large_Returns_All()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(DomainTestHelper.GenerateLogEntries(_now));

            //Act
            var results = _tasks.Paged(1, 30, null).ToList();

            //Assert
            Assert.AreEqual(16, results.Count);
        }

        [Test]
        public void Paged_With_Filter_Filters()
        {
            //Arrange
            const string name = "sWeEt";
            _logEntryRepository.Expect(x => x.All()).Return(DomainTestHelper.GenerateLogEntries(_now));

            //Act
            var results = _tasks.Paged(1, 5, new SearchFilter { ProjectName = name }).ToList();

            //Assert
            Assert.AreEqual(2, results.Count);
            Assert.ForAll(results, x => x.Component.Project.Name == name.ToLowerInvariant());
            _logEntryRepository.VerifyAllExpectations();
        }

        [Test]
        public void AddFilterToQuery_With_ProjectName_Filter_Filters_Using_Case_Insensitive()
        {
            //Arrange
            const string name = "sWeEt";
            var entries = DomainTestHelper.GenerateLogEntries(_now);

            //Act
            var results = _tasks.AddFilterToQuery(new SearchFilter { ProjectName = name }, entries).ToList();

            //Assert
            Assert.AreEqual(2, results.Count);
            Assert.ForAll(results, x => x.Component.Project.Name == name.ToLowerInvariant());
        }

        [Test]
        public void AddFilterToQuery_With_ComponentName_Filter_Filters_Using_Case_Insensitive()
        {
            //Arrange
            const string name = "TREATS";
            var entries = DomainTestHelper.GenerateLogEntries(_now);

            //Act
            var results = _tasks.AddFilterToQuery(new SearchFilter { ComponentName = name }, entries).ToList();

            //Assert
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(name.ToLowerInvariant(), results.SingleOrDefault().Component.Name);
        }

        [Test]
        public void AddFilterToQuery_With_StartDate_Filter_Uses_Date_Inclusive()
        {
            //Arrange
            var date = _now.AddDays(8);
            var entries = DomainTestHelper.GenerateLogEntries(_now);

            //Act
            var results = _tasks.AddFilterToQuery(new SearchFilter { StartDate = date }, entries).ToList();

            //Assert
            Assert.AreEqual(3, results.Count);
            Assert.ForAll(results, x => x.DateAdded >= _now.AddDays(8));
        }

        [Test]
        public void AddFilterToQuery_With_EndDate_Filter_Uses_Date_Inclusive()
        {
            //Arrange
            var date = _now.AddDays(5);
            var entries = DomainTestHelper.GenerateLogEntries(_now);

            //Act
            var results = _tasks.AddFilterToQuery(new SearchFilter { EndDate = date }, entries).ToList();

            //Assert
            Assert.AreEqual(13, results.Count);
            Assert.ForAll(results, x => x.DateAdded <= date);
        }

        [Test]
        public void AddFilterToQuery_With_Start_And_End_DateRange_Filter()
        {
            //Arrange
            var startDate = _now.AddDays(2);
            var endDate = _now.AddDays(9);
            var entries = DomainTestHelper.GenerateLogEntries(_now);

            //Act
            var results = _tasks.AddFilterToQuery(new SearchFilter { StartDate = startDate, EndDate = endDate }, entries).ToList();

            //Assert
            Assert.AreEqual(2, results.Count);
            Assert.ForAll(results, x => x.DateAdded >= startDate && x.DateAdded <= endDate);
        }

        [Test]
        public void AddFilterToQuery_With_LogLevel_Filter()
        {
            //Arrange
            const LogLevel logLevel = LogLevel.Debug | LogLevel.Error;
            var entries = DomainTestHelper.GenerateLogEntries(_now);

            //Act
            var results = _tasks.AddFilterToQuery(new SearchFilter { LogLevel = logLevel }, entries).ToList();

            //Assert
            Assert.AreEqual(14, results.Count);
            Assert.ForAll(results, x => x.LogLevel.TestFor(LogLevel.Debug) || x.LogLevel.TestFor(LogLevel.Error));
        }

        [Test]
        public void AddFilterToQuery_With_MachineNamePartial_Filter_Filters_Using_Case_Insensitive()
        {
            //Arrange
            const string namePartial = "scrap";
            var entries = DomainTestHelper.GenerateLogEntries(_now);

            //Act
            var results = _tasks.AddFilterToQuery(new SearchFilter { MachineNamePartial = namePartial }, entries).ToList();

            //Assert
            Assert.AreEqual(13, results.Count);
            Assert.ForAll(results, x => x.MachineName.ToLowerInvariant().Contains(namePartial));
        }

        [Test]
        public void AddFilterToQuery_With_ExceptionPartial_Filter_Filters_Using_Case_Insensitive()
        {
            //Arrange
            const string exceptionPartial = "didn't do that";
            var entries = DomainTestHelper.GenerateLogEntries(_now);

            //Act
            var results = _tasks.AddFilterToQuery(new SearchFilter { ExceptionPartial = exceptionPartial }, entries).ToList();

            //Assert
            Assert.AreEqual(14, results.Count);
            Assert.ForAll(results, x => x.ExceptionMessage.ToLowerInvariant().Contains(exceptionPartial));
        }

        [Test]
        public void AddFilterToQuery_With_MessagePartial_Filter_Filters_Using_Case_Insensitive()
        {
            //Arrange
            const string messagePartial = "something went";
            var entries = DomainTestHelper.GenerateLogEntries(_now);

            //Act
            var results = _tasks.AddFilterToQuery(new SearchFilter { MessagePartial = messagePartial }, entries).ToList();

            //Assert
            Assert.AreEqual(14, results.Count);
            Assert.ForAll(results, x => x.LogMessage.ToLowerInvariant().Contains(messagePartial));
        }

        [Test]
        public void AddFilterToQuery_With_Full_Filter_Filters_Everything()
        {
            //Arrange
            var entries = DomainTestHelper.GenerateLogEntries(_now);

            //Act
            var results = _tasks.AddFilterToQuery(new SearchFilter
            {
                ProjectName = "Shattered",
                ComponentName = "Dreams",
                StartDate = _now,
                EndDate = _now,
                LogLevel = LogLevel.Error,
                ExceptionPartial = "didn't do that one thing",
                MachineNamePartial = "scrappyJoe",
                MessagePartial = "something went wrong!"
            }, entries).ToList();

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

        [Test]
        public void Count_Without_Filter()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(DomainTestHelper.GenerateLogEntries(_now));

            //Act
            var count = _tasks.Count(null);

            //Assert
            Assert.AreEqual(16, count);
        }

        [Test]
        public void Count_With_Filter()
        {
            //Arrange
            _logEntryRepository.Expect(x => x.All()).Return(DomainTestHelper.GenerateLogEntries(_now));

            //Act
            var count = _tasks.Count(new SearchFilter
            {
                ProjectName = "Shattered",
                ComponentName = "Dreams",
                StartDate = _now,
                EndDate = _now,
                LogLevel = LogLevel.Error,
                ExceptionPartial = "didn't do that one thing",
                MachineNamePartial = "scrappyJoe",
                MessagePartial = "something went wrong!"
            });

            //Assert
            Assert.AreEqual(1, count);
        }        
    }
}
