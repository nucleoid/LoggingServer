using System.Collections.Generic;
using System.Linq;
using LoggingServer.Interface.Extensions;
using MbUnit.Framework;

namespace LoggingServer.Tests.Interface.Extensions
{
    [TestFixture]
    public class IQueryableExtensionsTest
    {
        [Test]
        public void ToSelectList_With_No_Selected()
        {
            //Arrange
            var queryable = GenerateQueryable();
            const string dataTextField = "Title";
            const string dataValueField = "Content";

            //Act
            var selectList = queryable.ToSelectList(dataValueField, dataTextField, null);

            //Assert
            Assert.AreEqual(4, selectList.Count());
            Assert.ForAll(selectList, x => !x.Selected);
            Assert.AreEqual("without", selectList.SingleOrDefault(x => x.Text == "Without").Value);
            Assert.AreEqual("feeling", selectList.SingleOrDefault(x => x.Text == "Feeling").Value);
            Assert.AreEqual("azure", selectList.SingleOrDefault(x => x.Text == "Azure").Value);
            Assert.AreEqual("ray", selectList.SingleOrDefault(x => x.Text == "Ray").Value);
        }

        [Test]
        public void ToSelectList_With_Selected()
        {
            //Arrange
            var queryable = GenerateQueryable();
            const string dataTextField = "Title";
            const string dataValueField = "Content";

            //Act
            var selectList = queryable.ToSelectList(dataValueField, dataTextField, "feeling");

            //Assert
            Assert.AreEqual(4, selectList.Count());
            Assert.IsFalse(selectList.SingleOrDefault(x => x.Text == "Without").Selected);
            Assert.IsTrue(selectList.SingleOrDefault(x => x.Text == "Feeling").Selected);
            Assert.IsFalse(selectList.SingleOrDefault(x => x.Text == "Azure").Selected);
            Assert.IsFalse(selectList.SingleOrDefault(x => x.Text == "Ray").Selected);
        }

        private IQueryable<StubClass> GenerateQueryable()
        {
            return new List<StubClass>
                {
                    new StubClass {Title = "Without", Content = "without"},
                    new StubClass {Title = "Feeling", Content = "feeling"},
                    new StubClass {Title = "Azure", Content = "azure"},
                    new StubClass {Title = "Ray", Content = "ray"}
                }.AsQueryable();
        }

        private class StubClass
        {
            public string Title { get; set; }
            public string Content { get; set; }
        }
    }
}
