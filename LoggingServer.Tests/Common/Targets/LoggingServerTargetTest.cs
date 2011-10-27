using System;
using System.Linq;
using System.Reflection;
using LoggingServer.Common.Targets;
using MbUnit.Framework;

namespace LoggingServer.Tests.Common.Targets
{
    [TestFixture]
    public class LoggingServerTargetTest
    {
        private LoggingServerTarget _target;

        [SetUp]
        public void Setup()
        {
            _target = new LoggingServerTarget();
        }

        [Test]
        public void Constructor_Sets_Defaults()
        {
            //Assert
            Assert.AreEqual(string.Empty, _target.EnvironmentKey);
        }

        [Test]
        public void AddAssemblyParameters_Adds_Assembly_Parameters()
        {
            //Arrange
            var assembly = GetType().Assembly;

            //Act
            _target.AddAssemblyParameters(assembly);

            //Assert
            Assert.AreEqual("'Mitch Statz'", _target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyCompany").Layout.ToString());
            Assert.AreEqual("'Tests for LoggingServer.Tests'", _target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyDescription").Layout.ToString());
            Assert.AreEqual("'bd01c085-3a2c-432b-8ada-d876da6ef4f1'", _target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyGuid").Layout.ToString());
            Assert.AreEqual("'LoggingServer.Tests Unit Tests'", _target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyProduct").Layout.ToString());
            Assert.AreEqual("'LoggingServer.Tests'", _target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyTitle").Layout.ToString());
            Assert.AreEqual("'1.0.0.0'", _target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyVersion").Layout.ToString());
        }

        [Test]
        public void InitializeTarget_Without_AssemblyName()
        {
            //Act
            InvokeMethod<LoggingServerTarget>(_target, "InitializeTarget", null);

            //Assert
            Assert.AreEqual("'${basedir}'", _target.Parameters.SingleOrDefault(x => x.Name == "BaseDirectory").Layout.ToString());
            Assert.AreEqual("'${callsite:fileName=true}'", _target.Parameters.SingleOrDefault(x => x.Name == "CallSite").Layout.ToString());
            Assert.AreEqual("'${counter}'", _target.Parameters.SingleOrDefault(x => x.Name == "Counter").Layout.ToString());
            Assert.AreEqual("'${exception:format=Message}'", _target.Parameters.SingleOrDefault(x => x.Name == "ExceptionMessage").Layout.ToString());
            Assert.AreEqual("'${exception:format=Type}'", _target.Parameters.SingleOrDefault(x => x.Name == "ExceptionType").Layout.ToString());
            Assert.AreEqual("'${exception:format=ToString}'", _target.Parameters.SingleOrDefault(x => x.Name == "ExceptionString").Layout.ToString());
            Assert.AreEqual("'${exception:format=Method}'", _target.Parameters.SingleOrDefault(x => x.Name == "ExceptionMethod").Layout.ToString());
            Assert.AreEqual("'${exception:format=StackTrace}'", _target.Parameters.SingleOrDefault(x => x.Name == "ExceptionStackTrace").Layout.ToString());
            Assert.AreEqual("'${guid}'", _target.Parameters.SingleOrDefault(x => x.Name == "LogID").Layout.ToString());
            Assert.AreEqual("'${identity}'", _target.Parameters.SingleOrDefault(x => x.Name == "LogIdentity").Layout.ToString());
            Assert.AreEqual("'${level}'", _target.Parameters.SingleOrDefault(x => x.Name == "LogLevel").Layout.ToString());
            Assert.AreEqual("'${logger}'", _target.Parameters.SingleOrDefault(x => x.Name == "Logger").Layout.ToString());
            Assert.AreEqual("'${longdate}'", _target.Parameters.SingleOrDefault(x => x.Name == "LongDate").Layout.ToString());
            Assert.AreEqual("'${machinename}'", _target.Parameters.SingleOrDefault(x => x.Name == "MachineName").Layout.ToString());
            Assert.AreEqual("'${message}'", _target.Parameters.SingleOrDefault(x => x.Name == "LogMessage").Layout.ToString());
            Assert.AreEqual("'${processid}'", _target.Parameters.SingleOrDefault(x => x.Name == "ProcessID").Layout.ToString());
            Assert.AreEqual("'${processinfo}'", _target.Parameters.SingleOrDefault(x => x.Name == "ProcessInfo").Layout.ToString());
            Assert.AreEqual("'${processname}'", _target.Parameters.SingleOrDefault(x => x.Name == "ProcessName").Layout.ToString());
            Assert.AreEqual("'${processtime}'", _target.Parameters.SingleOrDefault(x => x.Name == "ProcessTime").Layout.ToString());
            Assert.AreEqual("'${stacktrace:format=DetailedFlat}'", _target.Parameters.SingleOrDefault(x => x.Name == "StackTrace").Layout.ToString());
            Assert.AreEqual("'${threadid}'", _target.Parameters.SingleOrDefault(x => x.Name == "ThreadID").Layout.ToString());
            Assert.AreEqual("'${threadname}'", _target.Parameters.SingleOrDefault(x => x.Name == "ThreadName").Layout.ToString());
            Assert.AreEqual("'${windows-identity}'", _target.Parameters.SingleOrDefault(x => x.Name == "WindowsIdentity").Layout.ToString());
            Assert.AreEqual(string.Format("'{0}'", string.Empty), _target.Parameters.SingleOrDefault(x => x.Name == "EnvironmentKey").Layout.ToString());

            Assert.IsNull(_target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyCompany"));
            Assert.IsNull(_target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyDescription"));
            Assert.IsNull(_target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyGuid"));
            Assert.IsNull(_target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyProduct"));
            Assert.IsNull(_target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyTitle"));
            Assert.IsNull(_target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyVersion"));
        }

        [Test]
        public void InitializeTarget_With_AssemblyName()
        {
            //Arrange
            _target.AssemblyName = GetType().Assembly.FullName;

            //Act
            InvokeMethod<LoggingServerTarget>(_target, "InitializeTarget", null);

            //Assert
            Assert.AreEqual("'${basedir}'", _target.Parameters.SingleOrDefault(x => x.Name == "BaseDirectory").Layout.ToString());
            Assert.AreEqual("'${callsite:fileName=true}'", _target.Parameters.SingleOrDefault(x => x.Name == "CallSite").Layout.ToString());
            Assert.AreEqual("'${counter}'", _target.Parameters.SingleOrDefault(x => x.Name == "Counter").Layout.ToString());
            Assert.AreEqual("'${exception:format=Message}'", _target.Parameters.SingleOrDefault(x => x.Name == "ExceptionMessage").Layout.ToString());
            Assert.AreEqual("'${exception:format=Type}'", _target.Parameters.SingleOrDefault(x => x.Name == "ExceptionType").Layout.ToString());
            Assert.AreEqual("'${exception:format=ToString}'", _target.Parameters.SingleOrDefault(x => x.Name == "ExceptionString").Layout.ToString());
            Assert.AreEqual("'${exception:format=Method}'", _target.Parameters.SingleOrDefault(x => x.Name == "ExceptionMethod").Layout.ToString());
            Assert.AreEqual("'${exception:format=StackTrace}'", _target.Parameters.SingleOrDefault(x => x.Name == "ExceptionStackTrace").Layout.ToString());
            Assert.AreEqual("'${guid}'", _target.Parameters.SingleOrDefault(x => x.Name == "LogID").Layout.ToString());
            Assert.AreEqual("'${identity}'", _target.Parameters.SingleOrDefault(x => x.Name == "LogIdentity").Layout.ToString());
            Assert.AreEqual("'${level}'", _target.Parameters.SingleOrDefault(x => x.Name == "LogLevel").Layout.ToString());
            Assert.AreEqual("'${logger}'", _target.Parameters.SingleOrDefault(x => x.Name == "Logger").Layout.ToString());
            Assert.AreEqual("'${longdate}'", _target.Parameters.SingleOrDefault(x => x.Name == "LongDate").Layout.ToString());
            Assert.AreEqual("'${machinename}'", _target.Parameters.SingleOrDefault(x => x.Name == "MachineName").Layout.ToString());
            Assert.AreEqual("'${message}'", _target.Parameters.SingleOrDefault(x => x.Name == "LogMessage").Layout.ToString());
            Assert.AreEqual("'${processid}'", _target.Parameters.SingleOrDefault(x => x.Name == "ProcessID").Layout.ToString());
            Assert.AreEqual("'${processinfo}'", _target.Parameters.SingleOrDefault(x => x.Name == "ProcessInfo").Layout.ToString());
            Assert.AreEqual("'${processname}'", _target.Parameters.SingleOrDefault(x => x.Name == "ProcessName").Layout.ToString());
            Assert.AreEqual("'${processtime}'", _target.Parameters.SingleOrDefault(x => x.Name == "ProcessTime").Layout.ToString());
            Assert.AreEqual("'${stacktrace:format=DetailedFlat}'", _target.Parameters.SingleOrDefault(x => x.Name == "StackTrace").Layout.ToString());
            Assert.AreEqual("'${threadid}'", _target.Parameters.SingleOrDefault(x => x.Name == "ThreadID").Layout.ToString());
            Assert.AreEqual("'${threadname}'", _target.Parameters.SingleOrDefault(x => x.Name == "ThreadName").Layout.ToString());
            Assert.AreEqual("'${windows-identity}'", _target.Parameters.SingleOrDefault(x => x.Name == "WindowsIdentity").Layout.ToString());
            Assert.AreEqual(string.Format("'{0}'", string.Empty), _target.Parameters.SingleOrDefault(x => x.Name == "EnvironmentKey").Layout.ToString());

            Assert.AreEqual("'Mitch Statz'", _target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyCompany").Layout.ToString());
            Assert.AreEqual("'Tests for LoggingServer.Tests'", _target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyDescription").Layout.ToString());
            Assert.AreEqual("'bd01c085-3a2c-432b-8ada-d876da6ef4f1'", _target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyGuid").Layout.ToString());
            Assert.AreEqual("'LoggingServer.Tests Unit Tests'", _target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyProduct").Layout.ToString());
            Assert.AreEqual("'LoggingServer.Tests'", _target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyTitle").Layout.ToString());
            Assert.AreEqual("'1.0.0.0'", _target.Parameters.SingleOrDefault(x => x.Name == "EntryAssemblyVersion").Layout.ToString());
        }

        public static T InvokeMethod<T>(object obj, string methodName, object[] parameters)
        {
            Type type = obj.GetType();
            MethodInfo info = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (info == null)
                throw new Exception("Method not found");

            return (T)info.Invoke(obj, parameters);
        }
    }
}
