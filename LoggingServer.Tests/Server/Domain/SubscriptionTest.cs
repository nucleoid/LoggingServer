using System.Collections.Generic;
using LoggingServer.Server.Domain;
using MbUnit.Framework;

namespace LoggingServer.Tests.Server.Domain
{
    [TestFixture]
    public class SubscriptionTest
    {
        [Test]
        public void Constructor_Sets_Defaults()
        {
            //Act
            var subscription = new Subscription();
            
            //Assert
            Assert.AreEqual(string.Empty, subscription.EmailList);
        }

        [Test]
        public void Emails_Uses_EmailList()
        {
            //Arrange
            var subscription = new Subscription {Emails = new List<string> {"blah@blah.com", "hehe@mail.com"}};

            //Act
            var list = subscription.Emails;

            //Assert
            Assert.AreEqual(2, list.Count);
            Assert.Contains(list, "blah@blah.com");
            Assert.Contains(list, "hehe@mail.com");
            Assert.AreEqual("blah@blah.com,hehe@mail.com", subscription.EmailList);
        }
    }
}
