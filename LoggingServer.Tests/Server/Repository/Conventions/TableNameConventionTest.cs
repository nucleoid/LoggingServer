using System;
using FluentNHibernate;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository.Conventions;
using MbUnit.Framework;

namespace LoggingServer.Tests.Server.Repository.Conventions
{
    [TestFixture]
    public class TableNameConventionTest
    {
        [Test]
        public void Apply_Pluralizes_And_Sets_Table_Name()
        {
            //Arrange
            var convention = new TableNameConvention();
            var tester = new ClassInstanceTester();

            //Act
            convention.Apply(tester);

            //Assert
            Assert.AreEqual("LogEntries", tester.NameOfTable);
        }

        private class ClassInstanceTester : IClassInstance
        {
            public string NameOfTable { get; set; }

            public bool IsSet(Member property)
            {
                throw new NotImplementedException();
            }

            public Type EntityType
            {
                get { return typeof(LogEntry); }
            }

            public string StringIdentifierForModel
            {
                get { throw new NotImplementedException(); }
            }

            bool ILazyLoadInspector.LazyLoad
            {
                get { throw new NotImplementedException(); }
            }

            bool IReadOnlyInspector.ReadOnly
            {
                get { throw new NotImplementedException(); }
            }

            public string TableName
            {
                get { throw new NotImplementedException(); }
            }

            IOptimisticLockInstance IClassInstance.OptimisticLock
            {
                get { throw new NotImplementedException(); }
            }

            ISchemaActionInstance IClassInstance.SchemaAction
            {
                get { throw new NotImplementedException(); }
            }

            public ICacheInstance Cache
            {
                get { throw new NotImplementedException(); }
            }

            public void Table(string tableName)
            {
                NameOfTable = tableName;
            }

            void IClassInstance.DynamicInsert()
            {
                throw new NotImplementedException();
            }

            void IClassInstance.DynamicUpdate()
            {
                throw new NotImplementedException();
            }

            void IClassInstance.BatchSize(int size)
            {
                throw new NotImplementedException();
            }

            void IClassInstance.LazyLoad()
            {
                throw new NotImplementedException();
            }

            void IClassInstance.ReadOnly()
            {
                throw new NotImplementedException();
            }

            void IClassInstance.Schema(string schema)
            {
                throw new NotImplementedException();
            }

            void IClassInstance.Where(string @where)
            {
                throw new NotImplementedException();
            }

            void IClassInstance.Subselect(string subselectSql)
            {
                throw new NotImplementedException();
            }

            void IClassInstance.Proxy<T>()
            {
                throw new NotImplementedException();
            }

            void IClassInstance.Proxy(Type type)
            {
                throw new NotImplementedException();
            }

            void IClassInstance.Proxy(string type)
            {
                throw new NotImplementedException();
            }

            public void ApplyFilter(string name, string condition)
            {
                throw new NotImplementedException();
            }

            public void ApplyFilter(string name)
            {
                throw new NotImplementedException();
            }

            public void ApplyFilter<TFilter>(string condition) where TFilter : FilterDefinition, new()
            {
                throw new NotImplementedException();
            }

            public void ApplyFilter<TFilter>() where TFilter : FilterDefinition, new()
            {
                throw new NotImplementedException();
            }

            public IClassInstance Not
            {
                get { throw new NotImplementedException(); }
            }

            OptimisticLock IClassInspector.OptimisticLock
            {
                get { throw new NotImplementedException(); }
            }

            SchemaAction IClassInspector.SchemaAction
            {
                get { throw new NotImplementedException(); }
            }

            public string Schema
            {
                get { throw new NotImplementedException(); }
            }

            bool IClassInspector.DynamicUpdate
            {
                get { throw new NotImplementedException(); }
            }

            bool IClassInspector.DynamicInsert
            {
                get { throw new NotImplementedException(); }
            }

            public int BatchSize
            {
                get { throw new NotImplementedException(); }
            }

            public bool Abstract
            {
                get { throw new NotImplementedException(); }
            }

            public string Check
            {
                get { throw new NotImplementedException(); }
            }

            public object DiscriminatorValue
            {
                get { throw new NotImplementedException(); }
            }

            public string Name
            {
                get { throw new NotImplementedException(); }
            }

            public string Persister
            {
                get { throw new NotImplementedException(); }
            }

            public Polymorphism Polymorphism
            {
                get { throw new NotImplementedException(); }
            }

            public string Proxy
            {
                get { throw new NotImplementedException(); }
            }

            public string Where
            {
                get { throw new NotImplementedException(); }
            }

            public string Subselect
            {
                get { throw new NotImplementedException(); }
            }

            public bool SelectBeforeUpdate
            {
                get { throw new NotImplementedException(); }
            }

            public IIdentityInspectorBase Id
            {
                get { throw new NotImplementedException(); }
            }

            ICacheInspector IClassInspector.Cache
            {
                get { return Cache; }
            }

            public IDiscriminatorInspector Discriminator
            {
                get { throw new NotImplementedException(); }
            }

            public IVersionInspector Version
            {
                get { throw new NotImplementedException(); }
            }

            public IDefaultableEnumerable<IAnyInspector> Anys
            {
                get { throw new NotImplementedException(); }
            }

            public IDefaultableEnumerable<ICollectionInspector> Collections
            {
                get { throw new NotImplementedException(); }
            }

            public IDefaultableEnumerable<IComponentBaseInspector> Components
            {
                get { throw new NotImplementedException(); }
            }

            public IDefaultableEnumerable<IJoinInspector> Joins
            {
                get { throw new NotImplementedException(); }
            }

            public IDefaultableEnumerable<IOneToOneInspector> OneToOnes
            {
                get { throw new NotImplementedException(); }
            }

            public IDefaultableEnumerable<IPropertyInspector> Properties
            {
                get { throw new NotImplementedException(); }
            }

            public IDefaultableEnumerable<IManyToOneInspector> References
            {
                get { throw new NotImplementedException(); }
            }

            public IDefaultableEnumerable<ISubclassInspectorBase> Subclasses
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}
