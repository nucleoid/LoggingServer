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
