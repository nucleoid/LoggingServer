using System.Linq;
using System.Reflection;
using NLog.Common;
using NLog.Layouts;
using NLog.Targets;

namespace LoggingServer.Common.Targets
{
    /// <summary>
    /// Either NLogConfiguration.ConfigureServerLogger can be used for programmatic NLog configuration
    /// or an app config can be configured.  You will need to reference the LoggingServer.Common assembly.
    /// 
    /// Example app config snippet:
    /// 
    ///&lt;configSections&gt;
    ///    &lt;section name="nlog" type="NLog.Config.ConfigSectionHandler, NLog" /&gt;
    ///&lt;/configSections&gt;
    ///&lt;nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"&gt;
    ///    &lt;extensions&gt;
    ///        &lt;add assembly="LoggingServer.Common"/&gt;
    ///    &lt;/extensions&gt;
    ///    &lt;targets async="true"&gt;
    ///        &lt;target name="logserver" type="LoggingServer" endpointAddress="http://localhost:60925/LoggingServer.svc" assemblyName="LoggingServer.Interface"/&gt;
    ///    &lt;/targets&gt;
    ///    &lt;rules&gt;
    ///        &lt;logger name="*" minLevel="Trace" appendTo="logserver"/&gt;
    ///    &lt;/rules&gt;
    ///&lt;/nlog&gt;
    /// </summary>
    [Target("LoggingServer")]
    public class LoggingServerTarget : LogReceiverWebServiceTarget
    {
        public LoggingServerTarget()
        {
            EnvironmentKey = string.Empty;
        }

        public string AssemblyName { get; set; }
        public string EnvironmentKey { get; set; }
        public bool BaseWriteCalled { get; set; }

        protected override void InitializeTarget()
        {
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
            Parameters.Add(new MethodCallParameter("EnvironmentKey", EnvironmentKey));
            if (!string.IsNullOrEmpty(AssemblyName) && !Parameters.Any(x => x.Name == "EntryAssemblyCompany"))
            {
                var assembly = Assembly.Load(AssemblyName);
                AddAssemblyParameters(assembly);
            }
            base.InitializeTarget();
        }

        public void AddAssemblyParameters(Assembly assembly)
        {
            Parameters.Add(new MethodCallParameter("EntryAssemblyCompany", AssemblyInfoUtil.Company(assembly)));
            Parameters.Add(new MethodCallParameter("EntryAssemblyDescription", AssemblyInfoUtil.Description(assembly)));
            Parameters.Add(new MethodCallParameter("EntryAssemblyGuid", AssemblyInfoUtil.Guid(assembly)));
            Parameters.Add(new MethodCallParameter("EntryAssemblyProduct", AssemblyInfoUtil.Product(assembly)));
            Parameters.Add(new MethodCallParameter("EntryAssemblyTitle", AssemblyInfoUtil.Title(assembly)));
            Parameters.Add(new MethodCallParameter("EntryAssemblyVersion", AssemblyInfoUtil.Version(assembly)));
        }

        /// <summary>
        /// Fixing a bug in LogReceiverWebServiceTarget.Write(AsyncLogEventInfo[] logEvents) when logEvents has no elements
        /// </summary>
        /// <param name="logEvents"></param>
        protected override void Write(AsyncLogEventInfo[] logEvents)
        {
            if (logEvents != null && logEvents.Length > 0)
            {
                BaseWriteCalled = true;
                base.Write(logEvents);
            }
        }
    }
}
