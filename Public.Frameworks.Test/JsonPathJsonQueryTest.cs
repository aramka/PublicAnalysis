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

        [TestMethod]
        public void Query_JsonObjectPath()
        {
            // Arrange
            var jsonQuery = new JsonPathJsonQuery();
            var jsonNode = JsonNode.Parse("{\"AAPL\":{\"facts\":{\"us-gaap\":{\"AccountsPayable\":{\"units\":{\"USD\":100,\"EUR\":90}}}}}}");
            var path = new string[] { "AAPL", "facts", "us-gaap", "AccountsPayable", "units" };
            // Act
            var result = jsonQuery.Query(jsonNode!, path);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            var unitsNode = result.First() as JsonObject;
            Assert.IsNotNull(unitsNode);
            Assert.AreEqual(100, unitsNode["USD"]!.GetValue<int>());
            Assert.AreEqual(90, unitsNode["EUR"]!.GetValue<int>());
        }
        [TestMethod]
        public void Query_ArrayPath()
        {
            // Arrange
            var jsonQuery = new JsonPathJsonQuery();
            var jsonNode = JsonNode.Parse("{\"AAPL\":{\"facts\":{\"us-gaap\":{\"AccountsPayable\":[{\"units\":{\"USD\":100}},{\"units\":{\"USD\":200}}]}}}}");
            var path = new string[] { "AAPL", "facts", "us-gaap", "AccountsPayable" };
            // Act
            var result = jsonQuery.Query(jsonNode!, path);
            // Assert
            Assert.IsNotNull(result);
            Assert.HasCount(1, result);
            var accountsPayableArray = result.First() as JsonArray;
            Assert.AreEqual(2, accountsPayableArray?.Count());
            var firstItem = accountsPayableArray![0] as JsonObject;
            var secondItem = accountsPayableArray[1] as JsonObject;
            Assert.IsNotNull(firstItem);
            Assert.IsNotNull(secondItem);
            Assert.AreEqual(100, firstItem["units"]!["USD"]!.GetValue<int>());
            Assert.AreEqual(200, secondItem["units"]!["USD"]!.GetValue<int>());
        }

        [TestMethod]
        public void Query_ArrayPath_WithFilter() { 
            Assert.Fail("must implement. add new filter parameter to the Query method in IJsonQuery and JsonPathJsonQuery. Then implement the filter logic in JsonPathJsonQuery.Query method. Finally, implement this test to verify the filter functionality.");
        }
        [TestMethod]
        public void Query_WithProjection() { 
            Assert.Fail("must implement. add new projection parameter to the Query method in IJsonQuery and JsonPathJsonQuery. Then implement the projection logic in JsonPathJsonQuery.Query method. Finally, implement this test to verify the projection functionality.");
        }
    }
    
}
