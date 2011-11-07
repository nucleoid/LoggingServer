
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository.Overrides;
using MbUnit.Framework;

namespace LoggingServer.Tests.Server.Repository.Overrides
{
    [TestFixture]
    public class ProjectOverrideTest
    {
        [Test]
        public void Override_Configures_Uniqueness()
        {
            //Arrange
            var componentOverride = new ProjectOverride();
            var mapping = new AutoMapping<Project>(new List<Member>());

            //Act
            componentOverride.Override(mapping);

            //Assert
            var propertyParts = (mapping.GetField("providers") as MappingProviderStore).Properties.Select(x => (x as PropertyPart));
            var store = (propertyParts.SingleOrDefault(x => (x.GetField("member") as Member).Name == "Name").GetField("columnAttributes") as AttributeStore<ColumnMapping>).GetField("store") as AttributeStore;
            Assert.AreEqual(true, store["Unique"]);
        }

        [Test]
        public void Override_Configures_Cache()
        {
            //Arrange
            var componentOverride = new ProjectOverride();
            var mapping = new AutoMapping<Project>(new List<Member>());

            //Act
            componentOverride.Override(mapping);

            //Assert
            var attributeStore = ((mapping.Cache.GetField("attributes") as AttributeStore<CacheMapping>).GetField("store") as AttributeStore);
            Assert.AreEqual("read-write", attributeStore["Usage"]);
        }
    }
}
