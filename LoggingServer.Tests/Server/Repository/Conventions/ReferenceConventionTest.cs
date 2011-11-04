
using System;
using System.Collections.Generic;
using FluentNHibernate;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository.Conventions;
using MbUnit.Framework;
using Rhino.Mocks;

namespace LoggingServer.Tests.Server.Repository.Conventions
{
    [TestFixture]
    public class ReferenceConventionTest
    {
        [Test]
        public void Apply_Sets_Column_Id_Name()
        {
            //Arrange
            var convention = new ReferenceConvention();
            var cascader = MockRepository.GenerateMock<ICascadeInstance>();
            var tester = new ManyToOneInstanceTester(cascader);

            //Act
            convention.Apply(tester);

            //Assert
            Assert.AreEqual("ComponentId", tester.ColumnName);
        }

        [Test]
        public void Apply_Sets_Cascade_And_LazyLoad()
        {
            //Arrange
            var convention = new ReferenceConvention();
            var cascader = MockRepository.GenerateMock<ICascadeInstance>();
            cascader.Expect(x => x.None());
            var tester = new ManyToOneInstanceTester(cascader);

            //Act
            convention.Apply(tester);

            //Assert
            Assert.IsTrue(tester.IsLazyLoad);
            cascader.VerifyAllExpectations();
        }

        private class ManyToOneInstanceTester : IManyToOneInstance
        {
            private ICascadeInstance _cascadeInstance;

            public string ColumnName { get; private set; }
            public bool IsLazyLoad { get; private set; }

            public ManyToOneInstanceTester(ICascadeInstance cascadeInstance)
            {
                _cascadeInstance = cascadeInstance;
            }

            public void Column(string columnName)
            {
                ColumnName = columnName;
            }

            public void CustomClass<T>()
            {
                throw new NotImplementedException();
            }

            public void CustomClass(Type type)
            {
                throw new NotImplementedException();
            }

            public void Index(string index)
            {
                throw new NotImplementedException();
            }

            void IManyToOneInstance.Insert()
            {
                throw new NotImplementedException();
            }

            void IManyToOneInstance.OptimisticLock()
            {
                throw new NotImplementedException();
            }

            void IManyToOneInstance.LazyLoad()
            {
                IsLazyLoad = true;
            }

            void IManyToOneInstance.LazyLoad(Laziness laziness)
            {
                throw new NotImplementedException();
            }

            void IManyToOneInstance.Nullable()
            {
                throw new NotImplementedException();
            }

            void IManyToOneInstance.PropertyRef(string property)
            {
                throw new NotImplementedException();
            }

            public void ReadOnly()
            {
                throw new NotImplementedException();
            }

            public void Unique()
            {
                throw new NotImplementedException();
            }

            public void UniqueKey(string key)
            {
                throw new NotImplementedException();
            }

            void IManyToOneInstance.Update()
            {
                throw new NotImplementedException();
            }

            void IManyToOneInstance.ForeignKey(string key)
            {
                throw new NotImplementedException();
            }

            void IManyToOneInstance.Formula(string formula)
            {
                throw new NotImplementedException();
            }

            public void OverrideInferredClass(Type type)
            {
                throw new NotImplementedException();
            }

            IAccessInstance IManyToOneInstance.Access
            {
                get { throw new NotImplementedException(); }
            }

            ICascadeInstance IManyToOneInstance.Cascade
            {
                get { return _cascadeInstance; }
            }

            IFetchInstance IManyToOneInstance.Fetch
            {
                get { throw new NotImplementedException(); }
            }

            public IManyToOneInstance Not
            {
                get { throw new NotImplementedException(); }
            }

            INotFoundInstance IManyToOneInstance.NotFound
            {
                get { throw new NotImplementedException(); }
            }

            Access IAccessInspector.Access
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsSet(Member property)
            {
                throw new NotImplementedException();
            }

            public Type EntityType
            {
                get { throw new NotImplementedException(); }
            }

            public string StringIdentifierForModel
            {
                get { throw new NotImplementedException(); }
            }

            public Member Property
            {
                get { return typeof (LogEntry).GetProperty("Component").ToMember(); }
            }

            public string Name
            {
                get { throw new NotImplementedException(); }
            }

            public IEnumerable<IColumnInspector> Columns
            {
                get { throw new NotImplementedException(); }
            }

            Cascade IManyToOneInspector.Cascade
            {
                get { throw new NotImplementedException();}
            }

            public TypeReference Class
            {
                get { throw new NotImplementedException(); }
            }

            public string Formula
            {
                get { throw new NotImplementedException(); }
            }

            Fetch IManyToOneInspector.Fetch
            {
                get { throw new NotImplementedException(); }
            }

            public string ForeignKey
            {
                get { throw new NotImplementedException(); }
            }

            bool IManyToOneInspector.Insert
            {
                get { throw new NotImplementedException(); }
            }

            Laziness IManyToOneInspector.LazyLoad
            {
                get { throw new NotImplementedException(); }
            }

            NotFound IManyToOneInspector.NotFound
            {
                get { throw new NotImplementedException(); }
            }

            public string PropertyRef
            {
                get { throw new NotImplementedException(); }
            }

            bool IManyToOneInspector.Update
            {
                get { throw new NotImplementedException(); }
            }

            bool IManyToOneInspector.Nullable
            {
                get { throw new NotImplementedException(); }
            }

            bool IManyToOneInspector.OptimisticLock
            {
                get { throw new NotImplementedException(); }
            }
        }
    }
}
