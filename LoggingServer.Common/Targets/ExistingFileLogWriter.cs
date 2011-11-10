
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NLog;
using NLog.Common;

namespace LoggingServer.Common.Targets
{
    public class ExistingFileLogWriter
    {
        public static string BackupFileExtension = "bak";

        private readonly string _logDir;
        private readonly string _fileLogExtension;
        private readonly string _logLayout;
        private readonly IWritableTarget _target;
        private readonly object _lockObject = new object();

        public ExistingFileLogWriter(string logDir, string fileLogExtension, string logLayout, IWritableTarget target)
        {
            _logDir = logDir;
            _fileLogExtension = fileLogExtension;
            _logLayout = logLayout;
            _target = target;
        }

        public IList<AsyncLogEventInfo> WriteAndRemoveLogs()
        {
            var files = Directory.GetFiles(_logDir, string.Format("*.{0}", _fileLogExtension));
            var allLogs = new List<AsyncLogEventInfo>();
            foreach (var file in files)
            {
                var backupname = string.Format("{0}.{1}", file, BackupFileExtension);
                File.Move(file, backupname);
                using (var reader = File.OpenText(backupname))
                {
                    var logs = new List<AsyncLogEventInfo>();
                    var line = reader.ReadLine();
                    while(!string.IsNullOrEmpty(line))
                    {
                        LogLevel level = ParseLogLevel(line);
                        string loggerName = ParseLoggerName(line);
                        string message = ParseLogMessage(line);
                        FileLogException exception = ParseException(line);
                        logs.Add(new AsyncLogEventInfo(LogEventInfo.Create(level, loggerName, message, exception), GenerateContinuation(file, backupname)));
                        line = reader.ReadLine();
                    }
                    if(logs.Count > 0)
                    {
                        _target.WriteLogs(logs.ToArray());
                        allLogs.AddRange(logs);
                    }
                }
            }
            return allLogs;
        }

        private LogLevel ParseLogLevel(string line)
        {
            var logLevel = LogLevel.Off;
            var logLevelPosition = FindElementPosition("${level}");
            var lineSplit = line.Split('|');
            if (logLevelPosition != -1 && lineSplit.Length >= logLevelPosition)
                logLevel = LogLevel.FromString(lineSplit[logLevelPosition]);
            return logLevel;
        }

        private string ParseLoggerName(string line)
        {
            var loggerNamePosition = FindElementPosition("${logger}");
            var lineSplit = line.Split('|');
            return parseLineElement(lineSplit, loggerNamePosition);
        }

        private string ParseLogMessage(string line)
        {
            var loggerMessagePosition = FindElementPosition("${message}");
            var lineSplit = line.Split('|');
            return parseLineElement(lineSplit, loggerMessagePosition);
        }

        private FileLogException ParseException(string line)
        {
            var lineSplit = line.Split('|');
            var exceptionMessage = parseLineElement(lineSplit, FindElementPosition("${exception:format=Message}"));

            if(!string.IsNullOrEmpty(exceptionMessage))
            {
                var exceptionMethod = parseLineElement(lineSplit, FindElementPosition("${exception:format=Method}"));
                var exceptionStackTrace = parseLineElement(lineSplit, FindElementPosition("${exception:format=StackTrace}"));
                var exceptionType = parseLineElement(lineSplit, FindElementPosition("${exception:format=Type}"));

                var builder = new StringBuilder().AppendFormat("Message: {0}{1}Method: {2}{1}StackTrace: {3}{1}Type: {4}", 
                    exceptionMessage, Environment.NewLine, exceptionMethod, exceptionStackTrace, exceptionType);
                return new FileLogException(builder.ToString());
            }
            return null;
        }

        private string parseLineElement(string[] lineSplit, int position)
        {
            if (position != -1 && lineSplit.Length > position)
                return lineSplit[position];
            return string.Empty;
        }

        private int FindElementPosition(string element)
        {
            return new List<string>(_logLayout.Split('|')).FindIndex(x => x == element);
        }

        private AsyncContinuation GenerateContinuation(string filename, string backupFilename)
        {
            return ex =>
                       {
                           lock (_lockObject)
                           {
                               if (File.Exists(backupFilename)) //No processing needed
                               {
                                   if (ex == null) //Target write succeeded
                                   {
                                       File.Delete(backupFilename);
                                   }
                                   else if (!File.Exists(filename)) //Target write failed
                                   {
                                       //Rename log file to original for later processing
                                       File.Move(backupFilename, filename);
                                   } else
                                   {
                                       //Another log entry has been written while processing this one, just append to it
                                       File.AppendAllLines(filename, File.ReadAllLines(backupFilename));
                                       File.Delete(backupFilename);
                                   }
                               }
                           }
                       };
        }
    }
}
