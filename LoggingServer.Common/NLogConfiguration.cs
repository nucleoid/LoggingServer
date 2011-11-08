using System.Reflection;
using LoggingServer.Common.Targets;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace LoggingServer.Common
{
    public static class NLogConfiguration
    {
        /// <summary>
        /// Configures NLog to use a logging server with an async wrapper.  
        /// If the logging server fails it uses a file logger until the logging server comes back up.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="loggingServerEndPoint"></param>
        /// <param name="targetAssembly"></param>
        /// <param name="minLogLevel"></param>
        /// <returns></returns>
        public static LoggingConfiguration ConfigureServerLogger(LoggingConfiguration config, string environment, string loggingServerEndPoint, Assembly targetAssembly, LogLevel minLogLevel)
        {
            if(config == null)
                config = new LoggingConfiguration();
            var serverTarget = new LoggingServerTarget {EndpointAddress = loggingServerEndPoint, EnvironmentKey = environment, UseBinaryEncoding = true, };
            serverTarget.AddAssemblyParameters(targetAssembly);

            var fileTarget = new FileTarget { FileName = "${basedir}/${shortdate}.log" };

            var fallbackWrapper = new FallbackGroupTarget(serverTarget, fileTarget) { ReturnToFirstOnSuccess = true };

            var asyncWrapper = new AsyncTargetWrapper
            {
                WrappedTarget = fallbackWrapper,
                QueueLimit = 100000,
                OverflowAction = AsyncTargetWrapperOverflowAction.Grow,
                BatchSize = 50,
                TimeToSleepBetweenBatches = 500
            };

            var fileRule = new LoggingRule("*", minLogLevel, asyncWrapper);
            config.LoggingRules.Add(fileRule);
            return config;
        }
    }
}