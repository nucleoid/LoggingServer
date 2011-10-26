using System;
using System.Web.Mvc;
using LoggingServer.Interface.Extensions;
using MbUnit.Framework;

namespace LoggingServer.Tests.Interface.Extensions
{
    [TestFixture]
    public class HtmlHelperExtensionsTest
    {
        [Test, ExpectedArgumentException]
        public void CheckBoxWithValue_Without_Name()
        {
            //Arrange
            HtmlHelper helper = MvcHelper.GetHtmlHelper();

            //Act
            helper.CheckBoxWithValue(null, false, null, null);
        }


        [Test]
        public void CheckBoxWithValue_With_Name()
        {
            //Arrange
            HtmlHelper helper = MvcHelper.GetHtmlHelper();

            //Act
            var checkBox = helper.CheckBoxWithValue("print", 3);

            //Assert
            Assert.AreEqual("<input id=\"print\" name=\"print\" type=\"checkbox\" value=\"3\" />", checkBox.ToString());
        }

        [Test]
        public void CheckBoxWithValue_With_Name_And_Checked()
        {
            //Arrange
            HtmlHelper helper = MvcHelper.GetHtmlHelper();

            //Act
            var checkBox = helper.CheckBoxWithValue("print", true, 3);

            Assert.AreEqual("<input checked=\"checked\" id=\"print\" name=\"print\" type=\"checkbox\" value=\"3\" />", checkBox.ToString());
        }

        [Test]
        public void CheckBoxWithValue_With_Name_And_Checked_And_HtmlAttributes()
        {
            //Arrange
            HtmlHelper helper = MvcHelper.GetHtmlHelper();

            //Act
            var checkBox = helper.CheckBoxWithValue("print", true, 3, new { awesomize = "yes", @class = "linkage" });

            //Assert
            Assert.AreEqual("<input awesomize=\"yes\" checked=\"checked\" class=\"linkage\" id=\"print\" name=\"print\" type=\"checkbox\" value=\"3\" />",
                checkBox.ToString());
        }

        [Test]
        public void ActionLinkArea_Routes_To_Area()
        {
            //Arrange
            HtmlHelper helper = MvcHelper.GetHtmlHelper();

            //Act
            var linkage = helper.ActionLinkArea<TestController>(c => c.Tester(), "test link", "SecretArea");

            //Assert
            Assert.AreEqual("<a href=\"/SecretArea/Test/Tester\">test link</a>", linkage.ToString());
        }

        [Test]
        public void ActionLinkArea_HtmlAttributes_Routes_To_Area()
        {
            //Arrange
            HtmlHelper helper = MvcHelper.GetHtmlHelper();

            //Act
            var linkage = helper.ActionLinkArea<TestController>(c => c.Tester(), "test link", "SecretArea", new { coolness = "11" });

            //Assert
            Assert.AreEqual("<a coolness=\"11\" href=\"/SecretArea/Test/Tester\">test link</a>", linkage.ToString());
        }

        [Test, ExpectedArgumentException("TProperty must be a flags enum")]
        public void CheckBoxesForFlagsEnum_Without_Enum_Property()
        {
            //Arrange
            var vdd = new ViewDataDictionary<TestModel>();
            HtmlHelper<TestModel> helper = MvcHelper.GetHtmlHelper(vdd);

            //Act
            helper.CheckBoxesForFlagsEnum(x => x.NotEnum, TestFlagsEnum.Blah);
        }

        [Test, ExpectedArgumentException("TProperty must be a flags enum")]
        public void CheckBoxesForFlagsEnum_Without_Flags_Enum_Property()
        {
            //Arrange
            var vdd = new ViewDataDictionary<TestModel>();
            HtmlHelper<TestModel> helper = MvcHelper.GetHtmlHelper(vdd);

            //Act
            helper.CheckBoxesForFlagsEnum(x => x.Test, TestEnum.Generic);
        }

        [Test]
        public void CheckBoxesForFlagsEnum_Outputs_Checkboxes_And_Hidden_Field()
        {
            //Arrange
            var vdd = new ViewDataDictionary<TestModel>();
            HtmlHelper<TestModel> helper = MvcHelper.GetHtmlHelper(vdd);

            //Act
            var checkBoxes = helper.CheckBoxesForFlagsEnum(x => x.TestFlags, TestFlagsEnum.OtherBlah);

            //Assert
            Assert.AreEqual("<label for=\"Not_TestFlags\">Blah</label> <input flaggedenum=\"true\" id=\"Not_TestFlags\" name=\"Not.TestFlags\" type=\"checkbox\" value=\"Blah\" />\r\n<label for=\"Not_TestFlags\">OtherBlah</label> <input checked=\"checked\" flaggedenum=\"true\" id=\"Not_TestFlags\" name=\"Not.TestFlags\" type=\"checkbox\" value=\"OtherBlah\" />\r\n<input id=\"TestFlags\" name=\"TestFlags\" type=\"hidden\" value=\"OtherBlah\" />",
                checkBoxes.ToString());
        }

        [Test]
        public void CheckBoxesForFlagsEnum_Outputs_Checkboxes_And_Hidden_Field_With_Null_Value()
        {
            //Arrange
            var vdd = new ViewDataDictionary<TestModel>();
            HtmlHelper<TestModel> helper = MvcHelper.GetHtmlHelper(vdd);

            //Act
            var checkBoxes = helper.CheckBoxesForFlagsEnum(x => x.NullableTestFlags, null);

            //Assert
            Assert.AreEqual("<label for=\"Not_NullableTestFlags\">Blah</label> <input flaggedenum=\"true\" id=\"Not_NullableTestFlags\" name=\"Not.NullableTestFlags\" type=\"checkbox\" value=\"Blah\" />\r\n<label for=\"Not_NullableTestFlags\">OtherBlah</label> <input flaggedenum=\"true\" id=\"Not_NullableTestFlags\" name=\"Not.NullableTestFlags\" type=\"checkbox\" value=\"OtherBlah\" />\r\n<input id=\"NullableTestFlags\" name=\"NullableTestFlags\" type=\"hidden\" value=\"\" />",
                checkBoxes.ToString());
        }

        [Test]
        public void CheckBoxesForFlagsEnum_CheckBoxHtmlAttributes_Outputs_Checkboxes_And_Hidden_Field()
        {
            //Arrange
            var vdd = new ViewDataDictionary<TestModel>();
            HtmlHelper<TestModel> helper = MvcHelper.GetHtmlHelper(vdd);

            //Act
            var checkBoxes = helper.CheckBoxesForFlagsEnum(x => x.TestFlags, TestFlagsEnum.OtherBlah, new { awesomize = "yes" });

            //Assert
            Assert.AreEqual("<label for=\"Not_TestFlags\">Blah</label> <input awesomize=\"yes\" flaggedenum=\"true\" id=\"Not_TestFlags\" name=\"Not.TestFlags\" type=\"checkbox\" value=\"Blah\" />\r\n<label for=\"Not_TestFlags\">OtherBlah</label> <input awesomize=\"yes\" checked=\"checked\" flaggedenum=\"true\" id=\"Not_TestFlags\" name=\"Not.TestFlags\" type=\"checkbox\" value=\"OtherBlah\" />\r\n<input id=\"TestFlags\" name=\"TestFlags\" type=\"hidden\" value=\"OtherBlah\" />",
                checkBoxes.ToString());
        }

        [Test]
        public void CheckBoxesForFlagsEnum_With_Multiple_Values_Checked()
        {
            //Arrange
            var vdd = new ViewDataDictionary<TestModel>();
            HtmlHelper<TestModel> helper = MvcHelper.GetHtmlHelper(vdd);

            //Act
            var checkBoxes = helper.CheckBoxesForFlagsEnum(x => x.TestFlags, TestFlagsEnum.Blah | TestFlagsEnum.OtherBlah);

            //Assert
            Assert.AreEqual("<label for=\"Not_TestFlags\">Blah</label> <input checked=\"checked\" flaggedenum=\"true\" id=\"Not_TestFlags\" name=\"Not.TestFlags\" type=\"checkbox\" value=\"Blah\" />\r\n<label for=\"Not_TestFlags\">OtherBlah</label> <input checked=\"checked\" flaggedenum=\"true\" id=\"Not_TestFlags\" name=\"Not.TestFlags\" type=\"checkbox\" value=\"OtherBlah\" />\r\n<input id=\"TestFlags\" name=\"TestFlags\" type=\"hidden\" value=\"Blah, OtherBlah\" />",
                checkBoxes.ToString());
        }

        private class TestController : Controller
        {
            public ActionResult Tester()
            {
                return null;
            }
        }

        private class TestModel
        {
            public string NotEnum { get; set; }
            public TestFlagsEnum TestFlags { get; set; }
            public TestFlagsEnum? NullableTestFlags { get; set; }
            public TestEnum Test { get; set; }
        }

        [Flags]
        private enum TestFlagsEnum
        {
            Blah = 1,
            OtherBlah = 2
        }

        private enum TestEnum
        {
            Generic,
            NotGeneric
        }
    }
}
