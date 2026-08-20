using Public.Frameworks.JsonQuery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.Tests
{
    [TestClass]
    public class JsonQueryPathTest
    {
        [TestMethod]
        public void TestJsonQueryPath()
        {
            // Arrange
            var path = new JsonQueryPath("test");
            // Act
            var result = path.AsQueryExpressionString();
            // Assert
            Assert.AreEqual($"'test'", result);
        }

        [TestMethod]
        public void TestNullPath()
        {
            Assert.Throws<ArgumentNullException>(() => new JsonQueryPath(null));
        }
    }
}
