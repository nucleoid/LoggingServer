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

        [SetUp]
        public void Setup()
        {
            //Act
            _config = NLogConfiguration.ConfigureServerLogger(null, "blah", "http://blah.com", Assembly.GetExecutingAssembly(), LogLevel.Trace);
        }

        [Test]
        public void ConfigureServerLogger_Uses_AsnycWrapper()
        {
            //Assert
            Assert.IsInstanceOfType<AsyncTargetWrapper>(_config.LoggingRules[0].Targets[0]);
            Assert.IsInstanceOfType<LoggingServerTarget>(((_config.LoggingRules[0].Targets[0] as AsyncTargetWrapper).WrappedTarget as FallbackGroupTarget).Targets[0]);
        }

        [Test]
        public void ConfigureServerLogger_Uses_Environment_And_EndPoint()
        {
            //Assert
            var target = ((_config.LoggingRules[0].Targets[0] as AsyncTargetWrapper).WrappedTarget as FallbackGroupTarget).Targets[0] as LoggingServerTarget;
            Assert.AreEqual("blah", target.EnvironmentKey);
            Assert.AreEqual("http://blah.com", target.EndpointAddress);
        }

        [Test]
        public void ConfigureServerLogger_Adds_Assembly_Parameters()
        {
            //Assert
            var target = ((_config.LoggingRules[0].Targets[0] as AsyncTargetWrapper).WrappedTarget as FallbackGroupTarget).Targets[0] as LoggingServerTarget;
            Assert.IsNotNull(target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyCompany"));
            Assert.IsNotNull(target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyDescription"));
            Assert.IsNotNull(target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyGuid"));
            Assert.IsNotNull(target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyProduct"));
            Assert.IsNotNull(target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyTitle"));
            Assert.IsNotNull(target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyVersion"));
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
            Assert.AreEqual("'${basedir}/${shortdate}.log'", fileTarget.FileName.ToString());
        }
    }
}
