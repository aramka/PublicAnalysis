using Moq;
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
        public void Query_Null_Node_Throws()
        {
            Mock<IJsonQueryBuilderFactory> queryBuilderFactoryMoq = new Mock<IJsonQueryBuilderFactory>();
            JsonPathJsonQuery underTest = new JsonPathJsonQuery(queryBuilderFactoryMoq.Object);


            Assert.Throws<ArgumentNullException>(() => underTest.Query(null, Enumerable.Empty<IJsonQueryExpression>()));
        }
    }
}
