using Public.Frameworks.JsonQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;

namespace Public.Frameworks.Tests
{
    [TestClass]
    public class JsonPathJsonQueryTest
    {
        [TestMethod]
        public void Query_SingleValuePath()
        {
            // Arrange
            var jsonQuery = new JsonPathJsonQuery();
            var jsonNode = JsonNode.Parse("{\"AAPL\":{\"facts\":{\"us-gaap\":{\"AccountsPayable\":{\"units\":{\"USD\":100}}}}}}");
            var path = new string[] { "AAPL", "facts", "us-gaap", "AccountsPayable", "units", "USD" };
            // Act
            var result = jsonQuery.Query(jsonNode!, path);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(100, result.First().GetValue<int>());
        }
    }
}
