using System.Collections.Generic;
using System.Linq;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository.Overrides;
using MbUnit.Framework;

namespace LoggingServer.Tests.Server.Repository.Overrides
{
    [TestFixture]
    public class SubscriptionOverrideTest
    {
        [Test]
        public void Override_Configures_Property_Ignore()
        {
            //Arrange
            var componentOverride = new SubscriptionOverride();
            var mapping = new AutoMapping<Subscription>(new List<Member>());

            //Act
            componentOverride.Override(mapping);

            //Assert
            var propertyParts = (mapping.GetField("providers") as MappingProviderStore).Properties.Select(x => (x as PropertyPart));
            var part = propertyParts.SingleOrDefault(x => (x.GetField("member") as Member).Name == "Emails");
            Assert.IsNull(part);
        }
    }
}
