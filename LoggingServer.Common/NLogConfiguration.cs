using System.Reflection;
using LoggingServer.Common.Targets;
using NLog;
using NLog.Config;
using NLog.Targets.Wrappers;

namespace LoggingServer.Common
{
    public static class NLogConfiguration
    {
        public static LoggingConfiguration ConfigureServerLogger(LoggingConfiguration config, string environment, string loggingServerEndPoint, Assembly targetAssembly, LogLevel minLogLevel)
        {
            if(config == null)
                config = new LoggingConfiguration();
            var serverTarget = new LoggingServerTarget {EndpointAddress = loggingServerEndPoint, EnvironmentKey = environment};

            serverTarget.AddAssemblyParameters(targetAssembly);

            var wrapper = new AsyncTargetWrapper
            {
                WrappedTarget = serverTarget,
                QueueLimit = 5000,
                OverflowAction = AsyncTargetWrapperOverflowAction.Discard
            };

            var fileRule = new LoggingRule("*", minLogLevel, wrapper);
            config.LoggingRules.Add(fileRule);
            return config;
        }
    }
}