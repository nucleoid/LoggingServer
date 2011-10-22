using System.Configuration;
using LoggingServer.Common.Targets;
using NLog.Config;
using NLog.Targets;

namespace LoggingServer.Interface
{
    public static class NLogConfiguration
    {
        public static LoggingConfiguration CreateConfig()
        {
            var config = new LoggingConfiguration();
            ConfigureServerLogger(config);

            return config;
        }

        private static void ConfigureServerLogger(LoggingConfiguration config)
        {
            var serverTarget = new LoggingServerTarget();
            config.AddTarget("server", serverTarget);
            serverTarget.EndpointAddress = "http://mitch-pc/LoggingServer.svc";
            serverTarget.ClientId = "${guid:cached=true}";
            serverTarget.Parameters.Add(new MethodCallParameter("guid", "${guid}"));

            var assembly = typeof (NLogConfiguration).Assembly;
            serverTarget.AddAssemblyParameters(assembly);

            serverTarget.EnvironmentKey = ConfigurationManager.AppSettings["environment"];

            var fileRule = new LoggingRule("*", NLog.LogLevel.Trace, serverTarget);
            config.LoggingRules.Add(fileRule);
        }
    }
}