using System;
using System.IO;
using System.Linq;
using LoggingServer.Common;
using LoggingServer.Common.Targets;
using MbUnit.Framework;
using NLog;
using NLog.Common;
using NLog.LayoutRenderers;
using NLog.Layouts;
using Rhino.Mocks;

namespace LoggingServer.Tests.Common.Targets
{
    [TestFixture]
    public class ExistingFileLogWriterTest
    {
        private IWritableTarget _target;
        private LoggingServerTarget _targetForLayout;
        private ExistingFileLogWriter _writer;
        private string _filename;

        [SetUp]
        public void Setup()
        {
            _targetForLayout = new LoggingServerTarget { AssemblyName = GetType().Assembly.FullName };
            _target = MockRepository.GenerateMock<IWritableTarget>();
            var currentDir = Directory.GetCurrentDirectory();
            _writer = new ExistingFileLogWriter(currentDir, "spleen", _targetForLayout.LayoutForFile(), _target);
        }

        [TearDown]
        public void Teardown()
        {
            if (File.Exists(_filename))
                File.Delete(_filename);
            var backupName = string.Format("{0}.{1}", _filename, ExistingFileLogWriter.BackupFileExtension);
            if(File.Exists(backupName))
                File.Delete(backupName);
        }

        [Test]
        public void WriteAndRemoveLogs_Writes_Existing_Logs_To_Target()
        {
            //Arrange
            _filename = CreateLogFile(_targetForLayout.LayoutForFile());
            var events = Arg<AsyncLogEventInfo[]>.Matches(y => y.Length == 2 && y.All(z =>
                z.LogEvent.Exception.Message.Contains("Message: blah!\r\nMethod: System.String CreateLogFile(System.String)\r\nStackTrace:    at LoggingServer.Tests.Common.Targets.ExistingFileLogWriterTest.CreateLogFile(String layout) in C:\\gosquids\\LoggingServer\\LoggingServer.Tests\\Common\\Targets\\ExistingFileLogWriterTest.cs:line ") &&
                z.LogEvent.Exception.Message.Contains("\r\nType: System.Exception") && z.LogEvent.Level == LogLevel.Fatal && 
                z.LogEvent.LoggerName == GetType().Name && z.LogEvent.Message == "blah!"));
            _target.Expect(x => x.WriteLogs(events));

            //Act
            _writer.WriteAndRemoveLogs();

            //Assert
            _target.VerifyAllExpectations();
        }

        [Test]
        public void WriteAndRemoveLogs_Removes_Existing_Logs_On_Successful_Write()
        {
            //Arrange
            _filename = CreateLogFile(_targetForLayout.LayoutForFile());

            //Act
            var logs = _writer.WriteAndRemoveLogs();
            logs.FirstOrDefault().Continuation.Invoke(null);

            //Assert
            Assert.IsFalse(File.Exists(_filename));
            var backupName = string.Format("{0}.{1}", _filename, ExistingFileLogWriter.BackupFileExtension);
            Assert.IsFalse(File.Exists(backupName));
        }

        [Test]
        public void WriteAndRemoveLogs_Renames_Existing_Logs_To_Original_On_Failed_Write()
        {
            //Arrange
            _filename = CreateLogFile(_targetForLayout.LayoutForFile());

            //Act
            var logs = _writer.WriteAndRemoveLogs();
            logs.FirstOrDefault().Continuation.Invoke(new Exception());

            //Assert
            Assert.IsTrue(File.Exists(_filename));
            var backupName = string.Format("{0}.{1}", _filename, ExistingFileLogWriter.BackupFileExtension);
            Assert.IsFalse(File.Exists(backupName));
        }

        [Test]
        public void WriteAndRemoveLogs_With_Existing_Original_Appends_Original_Log_On_Failed_Write()
        {
            //Arrange
            _filename = CreateLogFile(_targetForLayout.LayoutForFile());

            //Act
            var logs = _writer.WriteAndRemoveLogs();
            CreateLogFile(_targetForLayout.LayoutForFile());
            logs.FirstOrDefault().Continuation.Invoke(new Exception());

            //Assert
            Assert.IsTrue(File.Exists(_filename));
            Assert.AreEqual(4, File.ReadAllLines(_filename).Length);
            var backupName = string.Format("{0}.{1}", _filename, ExistingFileLogWriter.BackupFileExtension);
            Assert.IsFalse(File.Exists(backupName));
        }

        [Test]
        public void WriteAndRemoveLogs_Without_Logs()
        {
            //Arrange
            _target.Expect(x => x.WriteLogs(Arg<AsyncLogEventInfo[]>.Is.Anything)).Repeat.Never();

            //Act
            _writer.WriteAndRemoveLogs();

            //Assert
            _target.VerifyAllExpectations();
        }

        private string CreateLogFile(string layout)
        {
            const string fileName = "templog.spleen";
            using (var stream = File.CreateText(fileName))
            {
                try
                {
                    throw new Exception("blah!"); //populating stacktrace and other fields that only show up after throwing
                }catch(Exception except)
                {
                    var formatted = Layout.FromString(layout).Render(new LogEventInfo(LogLevel.Fatal, GetType().Name, null, "blah!", null, except))
                        .Replace(Environment.NewLine, string.Empty);
                    stream.WriteLine(formatted);
                    stream.WriteLine(formatted);
                }
            }
            return fileName;
        }
    }
}
