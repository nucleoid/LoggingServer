
using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository.Overrides;
using MbUnit.Framework;

namespace LoggingServer.Tests.Server.Repository.Overrides
{
    [TestFixture]
    public class ComponentOverrideTest
    {
        [Test]
        public void Override_Sets_Generator()
        {
            //Arrange
            var componentOverride = new ComponentOverride();
            var mapping = new AutoMapping<Component>(new List<Member>());

            //Act
            componentOverride.Override(mapping);

            //Assert
            var providers = (mapping.GetField("providers") as MappingProviderStore);
            Assert.AreEqual("assigned", providers.Id.GetIdentityMapping().Generator.Class);
        }

        [Test]
        public void Override_Configures_HasMany()
        {
            //Arrange
            var componentOverride = new ComponentOverride();
            var mapping = new AutoMapping<Component>(new List<Member>());

            //Act
            componentOverride.Override(mapping);

            //Assert
            var collectionMapping = (mapping.GetField("providers") as MappingProviderStore).Collections.SingleOrDefault().GetCollectionMapping();
            Assert.AreEqual("EntryAssemblyGuid", collectionMapping.Key.Columns.FirstOrDefault().Name);
            Assert.AreEqual("all-delete-orphan", collectionMapping.Cascade);
            Assert.AreEqual(Lazy.True, collectionMapping.Lazy);
            Assert.IsTrue(collectionMapping.Inverse);
        }

        [Test]
        public void Override_Configures_Cache()
        {
            //Arrange
            var componentOverride = new ComponentOverride();
            var mapping = new AutoMapping<Component>(new List<Member>());

            //Act
            componentOverride.Override(mapping);

            //Assert
            var attributeStore = ((mapping.Cache.GetField("attributes") as AttributeStore<CacheMapping>).GetField("store") as AttributeStore);
            Assert.AreEqual("read-write", attributeStore["Usage"]);
        }
    }
}
