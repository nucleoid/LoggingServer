using System.Linq;
using System.Reflection;
using LoggingServer.Common;
using LoggingServer.Common.Targets;
using MbUnit.Framework;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace LoggingServer.Tests.Common
{
    [TestFixture]
    public class NLogConfigurationTest
    {
        private LoggingConfiguration _config;
        private const string Environment = "blah";
        private readonly Assembly _assembly = Assembly.GetExecutingAssembly();

        [SetUp]
        public void Setup()
        {
            //Act
            _config = NLogConfiguration.ConfigureServerLogger(null, Environment, "http://blah.com", _assembly, LogLevel.Trace);
        }

        [Test]
        public void ConfigureServerLogger_Uses_AsnycWrapper()
        {
            //Assert
            Assert.IsInstanceOfType<AsyncTargetWrapper>(_config.LoggingRules[0].Targets[0]);
            Assert.IsInstanceOfType<LoggingServerTarget>(((_config.LoggingRules[0].Targets[0] as AsyncTargetWrapper).WrappedTarget as FallbackGroupTarget).Targets[0]);
        }

        [Test]
        public void ConfigureServerLogger_Adds_LoggingServer_Parameters()
        {
            //Assert
            var target = ((_config.LoggingRules[0].Targets[0] as AsyncTargetWrapper).WrappedTarget as FallbackGroupTarget).Targets[0] as LoggingServerTarget;
            Assert.AreEqual("blah", target.EnvironmentKey);
            Assert.AreEqual("http://blah.com", target.EndpointAddress);
            Assert.AreEqual(Assembly.GetExecutingAssembly().FullName, target.AssemblyName);
            Assert.AreEqual(NLogConfiguration.LogFileExtension, target.FallbackFileExtion);
        }

        [Test]
        public void ConfigureServerLogger_Adds_AsyncTarget_Logging_Rule()
        {
            //Assert
            var rule = _config.LoggingRules[0];
            Assert.IsTrue(rule.Targets.Any(y => y.GetType() == typeof(AsyncTargetWrapper)));
            Assert.IsTrue(rule.Levels.Contains(LogLevel.Info));
        }

        [Test]
        public void ConfigureServerLogger_Uses_FallbackGroup()
        {
            //Assert
            Assert.IsInstanceOfType<FallbackGroupTarget>((_config.LoggingRules[0].Targets[0] as AsyncTargetWrapper).WrappedTarget);
        }

        [Test]
        public void ConfigureServerLogger_FallbackGroup_Has_FileTarget_As_Fallback()
        {
            //Assert
            Assert.IsInstanceOfType<FileTarget>(((_config.LoggingRules[0].Targets[0] as AsyncTargetWrapper).WrappedTarget as FallbackGroupTarget).Targets[1]);
        }

        [Test]
        public void ConfigureServerLogger_Uses_Daily_Rotating_Logs_For_FileTarget()
        {
            //Arrange
            var fileTarget = ((_config.LoggingRules[0].Targets[0] as AsyncTargetWrapper).WrappedTarget as FallbackGroupTarget).Targets[1] as FileTarget;

            //Assert
            Assert.AreEqual("'${basedir}\\${shortdate}.log'", fileTarget.FileName.ToString());
        }

        [Test]
        public void ConfigureServerLogger_Uses_LoggingServer_Layout_For_FileTarget()
        {
            //Arrange
            var fileTarget = ((_config.LoggingRules[0].Targets[0] as AsyncTargetWrapper).WrappedTarget as FallbackGroupTarget).Targets[1] as FileTarget;
            var loggingServerLayout = new LoggingServerTarget { EnvironmentKey = Environment, AssemblyName = _assembly.FullName };

            //Assert
            Assert.AreEqual(string.Format("'{0}'", loggingServerLayout.LayoutForFile()), fileTarget.Layout.ToString());
        }
    }
}
