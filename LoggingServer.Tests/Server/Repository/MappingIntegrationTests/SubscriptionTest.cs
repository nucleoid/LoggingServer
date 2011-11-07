using LoggingServer.Server.Autofac;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using MbUnit.Framework;

namespace LoggingServer.Tests.Server.Repository.MappingIntegrationTests
{
    [TestFixture]
    public class SubscriptionTest : BaseMappingTest<Subscription>
    {
        [Test, Rollback]
        public void Properties_Are_Mapped()
        {
            //Arrange
            var filter = new SearchFilter();
            var filterRepo = DependencyContainer.Resolve<IWritableRepository<SearchFilter>>();
            filterRepo.Save(filter);
            var subscription = new Subscription
            {
                Filter = filter,
                EmailList = "me@me.com,mstatz@gmail.com",
                IsDailyOverview = true
            };

            //Act
            Repository.Save(subscription);
            var postSubscription = Repository.Get(subscription.ID);

            //Assert
            Assert.AreEqual(subscription.ID, postSubscription.ID);
            Assert.AreEqual(filter.ID, postSubscription.Filter.ID);
            Assert.AreEqual("me@me.com,mstatz@gmail.com", postSubscription.EmailList);
            Assert.IsTrue(postSubscription.IsDailyOverview);
        }
    }
}
