using System;

namespace LoggingServer.Server.Domain
{
    public class LogEntry
    {
        public virtual Guid ID { get; set; }
        public virtual Guid ApplicationID { get; set; }
        public virtual string ClientID { get; set; }
        public virtual string BaseDirectory { get; set; }
        public virtual string CallSite { get; set; }
        public virtual int Counter { get; set; }
        public virtual string ExceptionMessage { get; set; }
        public virtual string ExceptionType { get; set; }
        public virtual string ExceptionString { get; set; }
        public virtual string ExceptionMethod { get; set; }
        public virtual string ExceptionStackTrace { get; set; }
        public virtual Guid LogID { get; set; }
        public virtual string LogIdentity { get; set; }
        public virtual LogLevel LogLevel { get; set; }
        public virtual string Logger { get; set; }
        public virtual DateTime LongDate { get; set; }
        public virtual string MachineName { get; set; }
        public virtual string LogMessage { get; set; }
        public virtual string ProcessID { get; set; }
        public virtual string ProcessInfo { get; set; }
        public virtual string ProcessName { get; set; }
        public virtual TimeSpan ProcessTime { get; set; }
        public virtual string StackTrace { get; set; }
        public virtual string ThreadID { get; set; }
        public virtual string ThreadName { get; set; }
        public virtual string WindowsIdentity { get; set; }
        public virtual string EntryAssemblyCompany { get; set; }
        public virtual string EntryAssemblyDescription { get; set; }
        public virtual Guid EntryAssemblyGuid { get; set; }
        public virtual string EntryAssemblyProduct { get; set; }
        public virtual string EntryAssemblyTitle { get; set; }
        public virtual string EntryAssemblyVersion { get; set; }
        public virtual string EnvironmentKey { get; set; }
        public virtual DateTime DateAdded { get; set; }
        public virtual bool NotificationsQueued { get; set; }
        public virtual int Version { get; set; }

        public virtual Component Component { get; set; }

        public virtual bool Equals(LogEntry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.ID.Equals(ID) && other.ApplicationID.Equals(ApplicationID) && Equals(other.ClientID, ClientID) && Equals(other.BaseDirectory, BaseDirectory) && Equals(other.CallSite, CallSite) && other.Counter == Counter && Equals(other.ExceptionMessage, ExceptionMessage) && Equals(other.ExceptionType, ExceptionType) && Equals(other.ExceptionString, ExceptionString) && Equals(other.ExceptionMethod, ExceptionMethod) && Equals(other.ExceptionStackTrace, ExceptionStackTrace) && other.LogID.Equals(LogID) && Equals(other.LogIdentity, LogIdentity) && Equals(other.LogLevel, LogLevel) && Equals(other.Logger, Logger) && other.LongDate.Equals(LongDate) && Equals(other.MachineName, MachineName) && Equals(other.LogMessage, LogMessage) && Equals(other.ProcessID, ProcessID) && Equals(other.ProcessInfo, ProcessInfo) && Equals(other.ProcessName, ProcessName) && other.ProcessTime.Equals(ProcessTime) && Equals(other.StackTrace, StackTrace) && Equals(other.ThreadID, ThreadID) && Equals(other.ThreadName, ThreadName) && Equals(other.WindowsIdentity, WindowsIdentity) && Equals(other.EntryAssemblyCompany, EntryAssemblyCompany) && Equals(other.EntryAssemblyDescription, EntryAssemblyDescription) && other.EntryAssemblyGuid.Equals(EntryAssemblyGuid) && Equals(other.EntryAssemblyProduct, EntryAssemblyProduct) && Equals(other.EntryAssemblyTitle, EntryAssemblyTitle) && Equals(other.EntryAssemblyVersion, EntryAssemblyVersion) && Equals(other.EnvironmentKey, EnvironmentKey) && other.NotificationsQueued.Equals(NotificationsQueued) && other.Version == Version && Equals(other.Component, Component);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (LogEntry)) return false;
            return Equals((LogEntry) obj);
        }
    }
}
