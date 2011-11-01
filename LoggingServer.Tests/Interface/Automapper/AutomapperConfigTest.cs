using System;
using AutoMapper;
using LoggingServer.Interface.Automapper;
using LoggingServer.Interface.Models;
using LoggingServer.Server.Autofac;
using LoggingServer.Server.Automapper;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using MbUnit.Framework;
using Rhino.Mocks;

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

        [Test]
        public void FilterResolver_Resolves_SearchFilter()
        {
            //Arrange
            var searchFilter = new SearchFilter {ID = Guid.NewGuid(), ComponentName = "filterThis!"};
            var repository = MockRepository.GenerateMock<IReadableRepository<SearchFilter>>();
            repository.Expect(x => x.Get(Arg<Guid>.Is.Equal(searchFilter.ID))).Return(searchFilter);
            var filterResolver = new FilterResolver(repository);
            DependencyContainer.Reset();
            DependencyContainer.RegisterInstance(filterResolver);
            DependencyContainer.BuildContainer();
            var model = new SubscriptionModel {FilterId = searchFilter.ID};
            AutomapperConfig.Setup();

            //Act
            var result = Mapper.Map<SubscriptionModel, Subscription>(model);

            //Assert
            Assert.AreEqual(searchFilter.ID, result.Filter.ID);
            Assert.AreEqual(searchFilter.ComponentName, result.Filter.ComponentName);
        }
    }
}
