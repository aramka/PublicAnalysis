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
            Mock<IJsonQueryBuilder> queryBuilderMoq = new Mock<IJsonQueryBuilder>();
            JsonPathJsonQuery underTest = new JsonPathJsonQuery(queryBuilderMoq.Object);


            Assert.Throws<ArgumentNullException>(() => underTest.Query(null, Enumerable.Empty<IJsonQueryExpression>()));
        }
    }
}
