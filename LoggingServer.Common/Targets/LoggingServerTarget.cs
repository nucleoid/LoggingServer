using System;
using NLog.Layouts;
using NLog.Targets;

namespace LoggingServer.Common.Targets
{
    [Target("LoggingServerTarget")]
    public class LoggingServerTarget : LogReceiverWebServiceTarget
    {
        public LoggingServerTarget()
        {
            UseBinaryEncoding = false;
            EnvironmentKey = string.Empty;
        }

        public string EnvironmentKey { get; set; }

        private Guid _applicationID = Guid.Empty;
        public string ApplicationID
        {
            get { return _applicationID.ToString(); }
            set { _applicationID = new Guid(value); }
        }

        protected override void InitializeTarget()
        {
            Parameters.Clear();

            Parameters.Add(new MethodCallParameter("BaseDirectory", Layout.FromString("${basedir}")));
            Parameters.Add(new MethodCallParameter("CallSite", Layout.FromString("${callsite:fileName=true}")));
            Parameters.Add(new MethodCallParameter("Counter", Layout.FromString("${counter}")));
            Parameters.Add(new MethodCallParameter("ExceptionMessage", Layout.FromString("${exception:format=Message}")));
            Parameters.Add(new MethodCallParameter("ExceptionType", Layout.FromString("${exception:format=Type}")));
            Parameters.Add(new MethodCallParameter("ExceptionString", Layout.FromString("${exception:format=ToString}")));
            Parameters.Add(new MethodCallParameter("ExceptionMethod", Layout.FromString("${exception:format=Method}")));
            Parameters.Add(new MethodCallParameter("ExceptionStackTrace", Layout.FromString("${exception:format=StackTrace}")));
            Parameters.Add(new MethodCallParameter("LogID", Layout.FromString("${guid}")));
            Parameters.Add(new MethodCallParameter("LogIdentity", Layout.FromString("${identity}")));
            Parameters.Add(new MethodCallParameter("LogLevel", Layout.FromString("${level}")));
            Parameters.Add(new MethodCallParameter("Logger", Layout.FromString("${logger}")));
            Parameters.Add(new MethodCallParameter("LongDate", Layout.FromString("${longdate}")));
            Parameters.Add(new MethodCallParameter("MachineName", Layout.FromString("${machinename}")));
            Parameters.Add(new MethodCallParameter("LogMessage", Layout.FromString("${message}")));
            Parameters.Add(new MethodCallParameter("ProcessID", Layout.FromString("${processid}")));
            Parameters.Add(new MethodCallParameter("ProcessInfo", Layout.FromString("${processinfo}")));
            Parameters.Add(new MethodCallParameter("ProcessName", Layout.FromString("${processname}")));
            Parameters.Add(new MethodCallParameter("ProcessTime", Layout.FromString("${processtime}")));
            Parameters.Add(new MethodCallParameter("StackTrace", Layout.FromString("${stacktrace:format=DetailedFlat}")));
            Parameters.Add(new MethodCallParameter("ThreadID", Layout.FromString("${threadid}")));
            Parameters.Add(new MethodCallParameter("ThreadName", Layout.FromString("${threadname}")));
            Parameters.Add(new MethodCallParameter("WindowsIdentity", Layout.FromString("${windows-identity}")));
            Parameters.Add(new MethodCallParameter("EntryAssemblyCompany", Layout.FromString("${entryassemblycompany}")));
            Parameters.Add(new MethodCallParameter("EntryAssemblyDescription", Layout.FromString("${entryassemblydescription}")));
            Parameters.Add(new MethodCallParameter("EntryAssemblyGuid", Layout.FromString("${entryassemblyguid}")));
            Parameters.Add(new MethodCallParameter("EntryAssemblyProduct", Layout.FromString("${entryassemblyproduct}")));
            Parameters.Add(new MethodCallParameter("EntryAssemblyTitle", Layout.FromString("${entryassemblytitle}")));
            Parameters.Add(new MethodCallParameter("EntryAssemblyVersion", Layout.FromString("${entryassemblyversion}")));
            Parameters.Add(new MethodCallParameter("EnvironmentKey", EnvironmentKey));
            Parameters.Add(new MethodCallParameter("ApplicationID", ApplicationID));

            base.InitializeTarget();
        }
    }
}
