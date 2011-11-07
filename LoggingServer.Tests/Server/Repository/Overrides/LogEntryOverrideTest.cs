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
    public class LogEntryOverrideTest
    {
        [Test]
        public void Override_Configures_HasMany()
        {
            //Arrange
            var logEntryOverride = new LogEntryOverride();
            var mapping = new AutoMapping<LogEntry>(new List<Member>());

            //Act
            logEntryOverride.Override(mapping);

            //Assert
            var referenceMapping = (mapping.GetField("providers") as MappingProviderStore).References.SingleOrDefault().GetManyToOneMapping();
            Assert.AreEqual("EntryAssemblyGuid", referenceMapping.Columns.SingleOrDefault().Name);
            Assert.AreEqual("ignore", referenceMapping.NotFound);
            var attributeStore = ((referenceMapping.GetField("attributes") as AttributeStore<ManyToOneMapping>).GetField("store") as AttributeStore);
            Assert.AreEqual(Laziness.Proxy.ToString(), attributeStore["Lazy"]);
            Assert.AreEqual(false, attributeStore["Insert"]);
            Assert.AreEqual(false, attributeStore["Update"]);
        }

        [Test]
        public void Override_Configures_Column_Lengths()
        {
            //Arrange
            var logEntryOverride = new LogEntryOverride();
            var mapping = new AutoMapping<LogEntry>(new List<Member>());

            //Act
            logEntryOverride.Override(mapping);

            //Assert
            var propertyParts = (mapping.GetField("providers") as MappingProviderStore).Properties.Select(x => (x as PropertyPart));
            Assert.AreEqual(10000, GetStoreForMember(propertyParts, "ExceptionStackTrace")["Length"]);
            Assert.AreEqual(10000, GetStoreForMember(propertyParts, "ExceptionString")["Length"]);
            Assert.AreEqual(10000, GetStoreForMember(propertyParts, "StackTrace")["Length"]);
            Assert.AreEqual(10000, GetStoreForMember(propertyParts, "ExceptionMessage")["Length"]);
            Assert.AreEqual(10000, GetStoreForMember(propertyParts, "ExceptionMethod")["Length"]);
            Assert.AreEqual(10000, GetStoreForMember(propertyParts, "ExceptionType")["Length"]);
            Assert.AreEqual(10000, GetStoreForMember(propertyParts, "BaseDirectory")["Length"]);
            Assert.AreEqual(10000, GetStoreForMember(propertyParts, "CallSite")["Length"]);
        }

        private AttributeStore GetStoreForMember(IEnumerable<PropertyPart> parts, string propertyName)
        {
            return (parts.SingleOrDefault(x => (x.GetField("member") as Member).Name == propertyName).GetField("columnAttributes") as AttributeStore<ColumnMapping>).GetField("store") as AttributeStore;
        }
    }
}
