using Microsoft.VisualStudio.TestTools.UnitTesting;
using CallApis;
using System.Collections.Generic;

namespace CallApisTest
{
    internal struct Helper 
    {
        public int Prop1 { get; set; }
        public int Prop2 { get; set; }
        public int Prop3 { get; set; }
    }
    [TestClass]
    public class CallApisTestClass
    {
        CA callApis;
        [TestInitialize]
        public void Initialize()
        {
           callApis = new CA();
        }

        [TestMethod]
        public void DeserializeObjectFromJson_Test()
        {
            // Arrange
            var expected = new Helper { Prop1 = 1, Prop2 = 2, Prop3 = 3 };

            // Act
            var result = callApis.DeserializeObjectFromJson<Helper>("{Prop1: 1, Prop2: 2, Prop3:3}");

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetMinimum_Test()
        {
            // Arrange
            var expected = 5;
            var list = new List<int> { 32, 46, 5, 45 };

            // Act
            var result = callApis.GetMinimum(list);

            // Assert
            Assert.AreEqual(result, expected);
        }
    }
}
