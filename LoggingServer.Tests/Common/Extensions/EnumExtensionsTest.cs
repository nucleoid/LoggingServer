using System;
using System.Collections.Generic;
using System.Linq;
using LoggingServer.Common.Extensions;
using MbUnit.Framework;

namespace LoggingServer.Tests.Common.Extensions
{
    [TestFixture]
    public class EnumExtensionsTest
    {
        [Test]
        [ExpectedException(typeof(InvalidOperationException), "Test value was not a member of the bitmask")]
        public void TestFor_Without_Enum_Flag_Support()
        {
            //Act
            TestEnumeration.TestValueThree.TestFor(BitField.Four);
        }

        [Test, ExpectedException(typeof(InvalidOperationException), "Tests only valid for single member of enumeration")]
        public void TestFor_On_Bitmask_Fails()
        {
            //Arrange
            const BitField actual = BitField.One | BitField.Two;
            const BitField mask = BitField.Two | BitField.Four;

            //Act
            actual.TestFor(mask);
        }

        [Test, ExpectedException(typeof(InvalidOperationException), "Enumeration is not a bitmask")]
        public void TestFor_Without_Test_Flag_Support()
        {
            //Arrange
            const TestEnumeration flags = TestEnumeration.TestValueOne | TestEnumeration.Testvaluefive;

            //Act
            flags.TestFor(TestEnumeration.Testvaluefive);
        }

        [Test]
        public void TestFor_Expected_Flags()
        {
            //Arrange
            const BitField flags = BitField.One | BitField.Two | BitField.Four;
            var expected = new List<BitField>(new[] { BitField.One, BitField.Two, BitField.Four });

            //Act
            foreach(var bitField in EnumExtensions.AsEnumerable<BitField>())
            {
                //Assert
                if (expected.Contains(bitField))
                    Assert.IsTrue(flags.TestFor(bitField));
                else
                    Assert.IsFalse(flags.TestFor(bitField));
            };
        }

        [Test, ExpectedException(typeof(NotSupportedException), "The type must be of Enum.")]
        public void AsEnumerable_Without_Enum()
        {
            //Act
            EnumExtensions.AsEnumerable<Int32>();
        }

        [Test]
        public void AsEnumerable_Returns_Enum_List()
        {
            //Act
            var enums = EnumExtensions.AsEnumerable<TestEnumeration>();

            //Assert
            Assert.AreEqual(7, enums.Count());
            Assert.Contains(enums, TestEnumeration.TestValueOne);
            Assert.Contains(enums, TestEnumeration.Testvaluetwo);
            Assert.Contains(enums, TestEnumeration.TestValueThree);
            Assert.Contains(enums, TestEnumeration.TesTvalueFour);
            Assert.Contains(enums, TestEnumeration.Testvaluefive);
            Assert.Contains(enums, TestEnumeration.Testvaluesix);
            Assert.Contains(enums, TestEnumeration.TestValueseveN);
        }

        [Test]
        public void ToSelectList_Generates_SelectList()
        {
            //Act
            var selectList = BitField.One.ToSelectList(false);

            //Assert
            Assert.AreEqual(6, selectList.Count());
            Assert.AreEqual("One", selectList.SingleOrDefault(x => x.Text == "One").Value);
            Assert.AreEqual("Two", selectList.SingleOrDefault(x => x.Text == "Two").Value);
            Assert.AreEqual("Four", selectList.SingleOrDefault(x => x.Text == "Four").Value);
            Assert.AreEqual("Eight", selectList.SingleOrDefault(x => x.Text == "Eight").Value);
            Assert.AreEqual("Sixteen", selectList.SingleOrDefault(x => x.Text == "Sixteen").Value);
            Assert.AreEqual("ThrityTwo", selectList.SingleOrDefault(x => x.Text == "ThrityTwo").Value);
        }

        [Test]
        public void ToSelectList_Without_Selected()
        {
            //Act
            var selectList = BitField.One.ToSelectList(false);

            //Assert
            Assert.IsFalse(selectList.SingleOrDefault(x => x.Text == "One").Selected);
            Assert.IsFalse(selectList.SingleOrDefault(x => x.Text == "Two").Selected);
            Assert.IsFalse(selectList.SingleOrDefault(x => x.Text == "Four").Selected);
            Assert.IsFalse(selectList.SingleOrDefault(x => x.Text == "Eight").Selected);
            Assert.IsFalse(selectList.SingleOrDefault(x => x.Text == "Sixteen").Selected);
            Assert.IsFalse(selectList.SingleOrDefault(x => x.Text == "ThrityTwo").Selected);
        }

        [Test]
        public void ToSelectList_With_Selected()
        {
            //Act
            var selectList = BitField.One.ToSelectList(true);

            //Assert
            Assert.IsTrue(selectList.SingleOrDefault(x => x.Text == "One").Selected);
            Assert.IsFalse(selectList.SingleOrDefault(x => x.Text == "Two").Selected);
            Assert.IsFalse(selectList.SingleOrDefault(x => x.Text == "Four").Selected);
            Assert.IsFalse(selectList.SingleOrDefault(x => x.Text == "Eight").Selected);
            Assert.IsFalse(selectList.SingleOrDefault(x => x.Text == "Sixteen").Selected);
            Assert.IsFalse(selectList.SingleOrDefault(x => x.Text == "ThrityTwo").Selected);
        }

        [Test]
        public void BitFieldAsEnumerable_With_One_Value_Returned()
        {
            var flags = BitField.One;
            IList<BitField> fields = EnumExtensions.BitFieldAsEnumerable(flags);

            Assert.IsNotNull(fields);
            Assert.AreEqual(1, fields.Count);
            Assert.AreEqual(BitField.One, fields[0]);
        }

        [Test]
        public void BitFieldAsEnumerable_With_Multiple_Values_Returned()
        {
            var flags = BitField.One | BitField.Two | BitField.Four;
            IList<BitField> fields = EnumExtensions.BitFieldAsEnumerable(flags);

            Assert.IsNotNull(fields);
            Assert.AreEqual(3, fields.Count);
            Assert.IsTrue(fields.Contains(BitField.One));
            Assert.IsTrue(fields.Contains(BitField.Two));
            Assert.IsTrue(fields.Contains(BitField.Four));
        }

        public enum TestEnumeration
        {
            TestValueOne,
            Testvaluetwo,
            TestValueThree,
            TesTvalueFour,
            Testvaluefive,
            Testvaluesix,
            TestValueseveN
        }

        [Flags]
        public enum BitField
        {
            One = 1,
            Two = 2,
            Four = 4,
            Eight = 8,
            Sixteen = 16,
            ThrityTwo = 32
        }
    }
}
