using AwesomeAssertions;
using Public.Frameworks.JsonQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Moq;

namespace Public.Frameworks.Tests
{
    [TestClass]
    public class JsonQueryBuilderTest
    {
        public JsonQueryBuilderTest() { }

        [TestMethod]
        public void AsJsonPathQueryString()
        {
            var queryBuilder = new JsonQueryBuilder();

            var expressions = Enumerable.Range(1, 5).Select((i) => { var expressionMoq = new Mock<IJsonQueryExpression>(); expressionMoq.Setup(a => a.AsQueryExpressionString()).Returns($"Expression_{i}"); return expressionMoq.Object; });

            foreach(IJsonQueryExpression expression in expressions) { queryBuilder.AddExpression(expression); }

            var actual = queryBuilder.AsJsonPathQueryString();

            var expected = string.Join("", expressions.Select(e => e.AsQueryExpressionString()));
            expected = $"${expected}";
            Assert.AreEqual(expected, actual);
        }
        
    }
}
