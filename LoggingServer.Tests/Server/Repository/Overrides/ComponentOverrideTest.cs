
using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate;
using FluentNHibernate.Automapping;
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
            Assert.AreEqual("assigned", mapping.Id(x => x.ID).GeneratedBy.GetGeneratorMapping().Class);
        }

        [Test, Ignore("finish tests")]
        public void Override_Configures_HasMany()
        {
            //Arrange
            var componentOverride = new ComponentOverride();
            var mapping = new AutoMapping<Component>(new List<Member>());

            //Act
            componentOverride.Override(mapping);

            //Assert
            var hasMany = mapping.HasMany(x => x.LogEntries);
            Assert.AreEqual("EntryAssemblyGuid", hasMany.KeyColumns.FirstOrDefault().Name);
            Assert.AreEqual("all-delete-orphan", (hasMany.Cascade.GetField("setter") as Action<string>).ToString());
//            Assert.IsTrue(hasMany.);
        }
    }
}
