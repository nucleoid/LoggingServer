using AutoMapper;
using LoggingServer.Interface.Automapper;
using MbUnit.Framework;

namespace LoggingServer.Tests.Interface.Automapper
{
    [TestFixture]
    public class AutomapperConfigTest
    {
        [Test]
        public void TestMappings()
        {
            //Act
            AutomapperConfig.Setup();

            //Assert
            Mapper.AssertConfigurationIsValid();
        }
    }
}
