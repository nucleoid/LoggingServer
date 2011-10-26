using System;
using System.ComponentModel.DataAnnotations;
using LoggingServer.Server.Domain;

namespace LoggingServer.Interface.Models
{
    public class LogEntryModel
    {
        [Display(Name = "ID")]
        public Guid ID { get; set; }
        public string Project { get; set; }
        public string Component { get; set; }
        public LogLevel LogLevel { get; set; }
        public string LogMessage { get; set; }
        public string MachineName { get; set; }
        public DateTime LongDate { get; set; }
        public DateTime DateAdded { get; set; }

        [ScaffoldColumn(false)]
        public string StackTrace { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }

        [ScaffoldColumn(false)]
        public string ExceptionString { get; set; }
        public string ExceptionMethod { get; set; }

        [ScaffoldColumn(false)]
        public string ExceptionStackTrace { get; set; }

        [Display(Name = "Log ID")]
        public Guid LogID { get; set; }
        public string LogIdentity { get; set; }
        public string Logger { get; set; }

        [Display(Name = "Process ID")]
        public string ProcessID { get; set; }
        public string ProcessInfo { get; set; }
        public string ProcessName { get; set; }
        public TimeSpan ProcessTime { get; set; }

        [Display(Name = "Thread ID")]
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
        public bool NotificationsQueued { get; set; }
        public int Version { get; set; }

        [Display(Name = "Component ID")]
        public Guid ComponentID { get; set; }

        [Display(Name = "Client ID")]
        public string BaseDirectory { get; set; }
        public string CallSite { get; set; }
        public int Counter { get; set; }
    }
}