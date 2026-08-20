using Public.Frameworks.JsonQuery;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Public.Frameworks.Tests
{
    [TestClass]
    public class JsonQueryFilterTest
    {
        [TestMethod]
        public void TestNullTarget()
        {
            Assert.Throws<ArgumentNullException>(() => new JsonQueryFilter(null, JsonQueryFilterOperators.Eq, 1));
        }
        [TestMethod]
        public void TestNullOperator()
        {
            Assert.Throws<ArgumentNullException>(() => new JsonQueryFilter("someNodeName", null, 1));
        }
        [TestMethod]
        public void TestNullValue()
        {
            Assert.Throws<ArgumentNullException>(() => new JsonQueryFilter("someNodeName", JsonQueryFilterOperators.Eq, null));
        }

        [DynamicData(nameof(GetTestCases))]
        [TestMethod]
        public void TestJsonQueryFilter(string target, JsonQueryFilterOperators @operator, object val,string expectation)
        {
            // Arrange
            var filter = new JsonQueryFilter(target, @operator, val);
            // Act
            var result = filter.AsQueryExpressionString();
            // Assert
            Assert.AreEqual(expectation, result);
        }

        public static List<(string target, JsonQueryFilterOperators @operator, object val, string expectation)> GetTestCases()
        {
            var operators = new[] { JsonQueryFilterOperators.Ge, JsonQueryFilterOperators.Gt, JsonQueryFilterOperators.Le, JsonQueryFilterOperators.Lt, JsonQueryFilterOperators.Eq };
            var target = "someNodeName";
            object[] values = new object[] { 1.0M, 1, "hello" };

            var cases = operators
                .Join<JsonQueryFilterOperators,object,bool,(JsonQueryFilterOperators,object)>
                (values, (op) => true, (val) => true, (op, val) => (op, val))
                .Select((@case)=>(target,@case.Item1, @case.Item2, @case.Item2 switch { object item when double.TryParse(item.ToString(), out double result) => $"@.{target}{@case.Item1}{@case.Item2}", _ => $"@.{target}{@case.Item1}'{@case.Item2}'" } ));

            return cases.ToList();
        }
    }
}
