using System;
using System.Linq;
using System.Reflection;
using FluentNHibernate;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository.Conventions;
using MbUnit.Framework;

namespace LoggingServer.Tests.Server.Repository.Conventions
{
    [TestFixture]
    public class HasManyConventionTest
    {
        [Test]
        public void Apply_Chains_Conventions()
        {
            //Arrange
            var convention = new HasManyConvention();
            var tester = new OneToManyCollectionInstanceTester();

            //Act
            convention.Apply(tester);

            //Assert
            Assert.AreEqual("LogEntryId", tester.Key.Columns.FirstOrDefault().Name);
            Assert.AreEqual("all-delete-orphan", tester.Cascade);
            Assert.IsTrue(tester.IsInverse);
            Assert.IsTrue(tester.IsLazyLoad);
        }

        private class OneToManyCollectionInstanceTester : IOneToManyCollectionInstance
        {
            private readonly IKeyInstance _key;

            public OneToManyCollectionInstanceTester()
            {
                _key = new KeyInstance(new KeyMapping());
                Cascade = string.Empty;
                IsInverse = false;
            }

            public string Cascade { get; private set; }

            public bool IsInverse { get; private set; }

            public bool IsLazyLoad { get; private set; }

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

            public void Table(string tableName)
            {
                throw new NotImplementedException();
            }

            void ICollectionInstance.Name(string name)
            {
                throw new NotImplementedException();
            }

            void ICollectionInstance.Schema(string schema)
            {
                throw new NotImplementedException();
            }

            void ICollectionInstance.LazyLoad()
            {
                IsLazyLoad = true;
            }

            public void ExtraLazyLoad()
            {
                throw new NotImplementedException();
            }

            void ICollectionInstance.BatchSize(int batchSize)
            {
                throw new NotImplementedException();
            }

            public void ReadOnly()
            {
                throw new NotImplementedException();
            }

            public void AsArray()
            {
                throw new NotImplementedException();
            }

            public void AsBag()
            {
                throw new NotImplementedException();
            }

            public void AsList()
            {
                throw new NotImplementedException();
            }

            public void AsMap()
            {
                throw new NotImplementedException();
            }

            public void AsSet()
            {
                throw new NotImplementedException();
            }

            void ICollectionInstance.Check(string constraint)
            {
                throw new NotImplementedException();
            }

            void ICollectionInstance.CollectionType<T>()
            {
                throw new NotImplementedException();
            }

            void ICollectionInstance.CollectionType(string type)
            {
                throw new NotImplementedException();
            }

            void ICollectionInstance.CollectionType(Type type)
            {
                throw new NotImplementedException();
            }

            void ICollectionInstance.Generic()
            {
                throw new NotImplementedException();
            }

            void ICollectionInstance.Inverse()
            {
                IsInverse = true;
            }

            void ICollectionInstance.Persister<T>()
            {
                throw new NotImplementedException();
            }

            void ICollectionInstance.Where(string whereClause)
            {
                throw new NotImplementedException();
            }

            void ICollectionInstance.OrderBy(string orderBy)
            {
                throw new NotImplementedException();
            }

            void ICollectionInstance.Sort(string sort)
            {
                throw new NotImplementedException();
            }

            public void Subselect(string subselect)
            {
                throw new NotImplementedException();
            }

            public IKeyInstance Key
            {
                get { return _key; }
            }

            public IIndexInstanceBase Index
            {
                get { throw new NotImplementedException(); }
            }

            public IElementInstance Element
            {
                get { throw new NotImplementedException(); }
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

            public IOneToManyInstance Relationship
            {
                get { throw new NotImplementedException(); }
            }

            public IOneToManyCollectionInstance Not
            {
                get { throw new NotImplementedException(); }
            }

            IRelationshipInstance ICollectionInstance.Relationship
            {
                get { return Relationship; }
            }

            ICollectionCascadeInstance ICollectionInstance.Cascade
            {
                get { return new CollectionCascadeInstance(x => Cascade = x); }
            }

            IFetchInstance ICollectionInstance.Fetch
            {
                get { throw new NotImplementedException(); }
            }

            IOptimisticLockInstance ICollectionInstance.OptimisticLock
            {
                get { throw new NotImplementedException(); }
            }

            OptimisticLock ICollectionInspector.OptimisticLock
            {
                get { throw new NotImplementedException(); }
            }

            ICollectionInstance ICollectionInstance.Not
            {
                get { return Not; }
            }

            IAccessInstance ICollectionInstance.Access
            {
                get { throw new NotImplementedException(); }
            }

            public ICacheInstance Cache
            {
                get { throw new NotImplementedException(); }
            }

            IKeyInspector ICollectionInspector.Key
            {
                get { return Key; }
            }

            IIndexInspectorBase ICollectionInspector.Index
            {
                get { return Index; }
            }

            public string Sort
            {
                get { throw new NotImplementedException(); }
            }

            public string TableName
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsMethodAccess
            {
                get { throw new NotImplementedException(); }
            }

            public MemberInfo Member
            {
                get { throw new NotImplementedException(); }
            }

            IOneToManyInspector IOneToManyCollectionInspector.Relationship
            {
                get { return Relationship; }
            }

            IRelationshipInspector ICollectionInspector.Relationship
            {
                get { return Relationship; }
            }

            Cascade ICollectionInspector.Cascade
            {
                get { throw new NotImplementedException(); }
            }

            Fetch ICollectionInspector.Fetch
            {
                get { throw new NotImplementedException(); }
            }

            bool ICollectionInspector.Generic
            {
                get { throw new NotImplementedException(); }
            }

            bool ICollectionInspector.Inverse
            {
                get { throw new NotImplementedException(); }
            }

            Access ICollectionInspector.Access
            {
                get { throw new NotImplementedException(); }
            }

            public int BatchSize
            {
                get { throw new NotImplementedException(); }
            }

            ICacheInspector ICollectionInspector.Cache
            {
                get { return Cache; }
            }

            public string Check
            {
                get { throw new NotImplementedException(); }
            }

            public Type ChildType
            {
                get { throw new NotImplementedException(); }
            }

            public TypeReference CollectionType
            {
                get { throw new NotImplementedException(); }
            }

            public ICompositeElementInspector CompositeElement
            {
                get { throw new NotImplementedException(); }
            }

            IElementInspector ICollectionInspector.Element
            {
                get { return Element; }
            }

            Lazy ICollectionInspector.LazyLoad
            {
                get { throw new NotImplementedException(); }
            }

            public string Name
            {
                get { throw new NotImplementedException(); }
            }

            public TypeReference Persister
            {
                get { throw new NotImplementedException(); }
            }

            public string Schema
            {
                get { throw new NotImplementedException(); }
            }

            public string Where
            {
                get { throw new NotImplementedException(); }
            }

            public string OrderBy
            {
                get { throw new NotImplementedException(); }
            }

            public Collection Collection
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}
