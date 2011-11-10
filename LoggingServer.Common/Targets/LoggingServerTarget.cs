using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NLog.Common;
using NLog.Layouts;
using NLog.Targets;

namespace LoggingServer.Common.Targets
{
    /// <summary>
    /// Either NLogConfiguration.ConfigureServerLogger can be used for programmatic NLog configuration
    /// or an app config can be configured.  You will need to reference the LoggingServer.Common assembly.
    /// 
    /// If fallbackFileExtension is set, existing log files will be pushed back to the logging server when it comes back up
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
    ///        &lt;target name="logserver" type="LoggingServer" endpointAddress="http://localhost:60925/LoggingServer.svc" assemblyName="LoggingServer.Interface" fallbackFileExtension="log" /&gt;
    ///    &lt;/targets&gt;
    ///    &lt;rules&gt;
    ///        &lt;logger name="*" minLevel="Trace" appendTo="logserver"/&gt;
    ///    &lt;/rules&gt;
    ///&lt;/nlog&gt;
    /// </summary>
    [Target("LoggingServer")]
    public class LoggingServerTarget : LogReceiverWebServiceTarget, IWritableTarget
    {
        public LoggingServerTarget()
        {
            EnvironmentKey = string.Empty;
        }

        public string AssemblyName { get; set; }
        public string EnvironmentKey { get; set; }
        public bool BaseWriteCalled { get; set; }

        /// <summary>
        /// If set, existing log files will be pushed back to the logging server when it comes back up
        /// </summary>
        public string FallbackFileExtension { get; set; }

        protected override void InitializeTarget()
        {
            foreach (var parameter in GetTargetParameters(EnvironmentKey))
                Parameters.Add(new MethodCallParameter(parameter.Key, Layout.FromString(parameter.Value)));
           
            if (!string.IsNullOrEmpty(AssemblyName) && !Parameters.Any(x => x.Name == "EntryAssemblyCompany"))
            {
                var assembly = Assembly.Load(AssemblyName);
                AddAssemblyParameters(assembly);
            }
            base.InitializeTarget();
        }

        public string LayoutForFile()
        {
            var parameters = GetTargetParameters(EnvironmentKey);
            var builder = new StringBuilder(parameters.FirstOrDefault().Value);
            foreach (var parameter in parameters.Skip(1))
                builder.AppendFormat("|{0}", parameter.Value);
            if(!string.IsNullOrEmpty(AssemblyName))
            {
                var assembly = Assembly.Load(AssemblyName);
                foreach (var parameter in GetTargetAssemblyParameters(assembly))
                    builder.AppendFormat("|'{0}'", parameter.Value);
            }
            return builder.ToString();
        }

        public void WriteLogs(AsyncLogEventInfo[] logEvents)
        {
            BaseWriteCalled = true;
            base.Write(logEvents);
        }

        protected override void Write(AsyncLogEventInfo[] logEvents)
        {
            if (logEvents != null && logEvents.Length > 0) //Fixing a bug in LogReceiverWebServiceTarget.Write(AsyncLogEventInfo[] logEvents) when logEvents has no elements
            {
                BaseWriteCalled = true;
                WriteFileLogs();
                base.Write(logEvents);
            }
        }

        private void AddAssemblyParameters(Assembly assembly)
        {
            foreach (var parameter in GetTargetAssemblyParameters(assembly))
                Parameters.Add(new MethodCallParameter(parameter.Key, parameter.Value));
        }

        private void WriteFileLogs()
        {
            if(!string.IsNullOrEmpty(FallbackFileExtension))
            {
                var currentDir = AppDomain.CurrentDomain.BaseDirectory;
                var logWriter = new ExistingFileLogWriter(currentDir, FallbackFileExtension, LayoutForFile(), this);
                logWriter.WriteAndRemoveLogs();
            }
        }

        private static IOrderedEnumerable<KeyValuePair<string, string>> GetTargetParameters(string environmentKey)
        {
            return new Dictionary<string, string>
            {
                {"BaseDirectory","${basedir}"},
                {"CallSite", "${callsite:fileName=true}"},
                {"Counter", "${counter}"},
                {"EnvironmentKey", environmentKey},
                {"ExceptionMessage", "${exception:format=Message}"},
                {"ExceptionType", "${exception:format=Type}"},
                {"ExceptionString", "${exception:format=ToString}"},
                {"ExceptionMethod", "${exception:format=Method}"},
                {"ExceptionStackTrace", "${exception:format=StackTrace}"},
                {"LogID", "${guid}"},
                {"LogIdentity", "${identity}"},
                {"Logger", "${logger}"},
                {"LogLevel", "${level}"},
                {"LogMessage", "${message}"},
                {"LongDate", "${longdate}"},
                {"MachineName", "${machinename}"},
                {"ProcessID", "${processid}"},
                {"ProcessInfo", "${processinfo}"},
                {"ProcessName", "${processname}"},
                {"ProcessTime", "${processtime}"},
                {"StackTrace", "${stacktrace:format=DetailedFlat}"},
                {"ThreadID", "${threadid}"},
                {"ThreadName", "${threadname}"},
                {"WindowsIdentity", "${windows-identity}"}
            }.OrderBy(x => x.Key);
        }

        private static IOrderedEnumerable<KeyValuePair<string, string>> GetTargetAssemblyParameters(Assembly assembly)
        {
            return new Dictionary<string, string>
            {
                {"EntryAssemblyCompany", AssemblyInfoUtil.Company(assembly)},
                {"EntryAssemblyDescription", AssemblyInfoUtil.Description(assembly)},
                {"EntryAssemblyGuid", AssemblyInfoUtil.Guid(assembly)},
                {"EntryAssemblyProduct", AssemblyInfoUtil.Product(assembly)},
                {"EntryAssemblyTitle", AssemblyInfoUtil.Title(assembly)},
                {"EntryAssemblyVersion", AssemblyInfoUtil.Version(assembly)}            
            }.OrderBy(x => x.Key);
        }
    }
}
