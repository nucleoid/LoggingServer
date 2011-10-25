using System;
using LoggingServer.Server.Domain;

namespace LoggingServer.Interface.Models
{
    public class LogEntryModel
    {
        public Guid ID { get; set; }
        public Guid ApplicationID { get; set; }
        public string ClientID { get; set; }
        public string BaseDirectory { get; set; }
        public string CallSite { get; set; }
        public int Counter { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionString { get; set; }
        public string ExceptionMethod { get; set; }
        public string ExceptionStackTrace { get; set; }
        public Guid LogID { get; set; }
        public string LogIdentity { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Logger { get; set; }
        public DateTime LongDate { get; set; }
        public string MachineName { get; set; }
        public string LogMessage { get; set; }
        public string ProcessID { get; set; }
        public string ProcessInfo { get; set; }
        public string ProcessName { get; set; }
        public TimeSpan ProcessTime { get; set; }
        public string StackTrace { get; set; }
        public string ThreadID { get; set; }
        public string ThreadName { get; set; }
        public string WindowsIdentity { get; set; }
        public string EntryAssemblyCompany { get; set; }
        public string EntryAssemblyDescription { get; set; }
        public Guid EntryAssemblyGuid { get; set; }
        public string EntryAssemblyProduct { get; set; }
        public string EntryAssemblyTitle { get; set; }
        public string EntryAssemblyVersion { get; set; }
        public string EnvironmentKey { get; set; }
        public DateTime DateAdded { get; set; }
        public bool NotificationsQueued { get; set; }
        public int Version { get; set; }

        public Guid ComponentID { get; set; }
        public string ComponentName { get; set; }

        public string ComponentProjectName { get; set; }
    }
}