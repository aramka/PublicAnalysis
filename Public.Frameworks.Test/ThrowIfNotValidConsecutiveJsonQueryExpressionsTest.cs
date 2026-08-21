using Public.Frameworks.JsonQuery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.Tests
{
    [TestClass]
    public class ThrowIfNotValidConsecutiveJsonQueryExpressionsTest
    {
        Dictionary<(Type?, Type?), bool> cases = new Dictionary<(Type?, Type?), bool>();
        public ThrowIfNotValidConsecutiveJsonQueryExpressionsTest()
        {

            cases.Add((null, null), false);
            cases.Add((null, typeof(JsonQueryFilter)), true);
            cases.Add((null, typeof(JsonQueryLogicalAnd)), false);
            cases.Add((null, typeof(JsonQueryLogicalOr)), false);
            cases.Add((null, typeof(JsonQueryPath)), true);
            cases.Add((typeof(JsonQueryFilter), null), false);
            cases.Add((typeof(JsonQueryFilter), typeof(JsonQueryFilter)), false);
            cases.Add((typeof(JsonQueryFilter), typeof(JsonQueryLogicalAnd)), true);
            cases.Add((typeof(JsonQueryFilter), typeof(JsonQueryLogicalOr)), true);
            cases.Add((typeof(JsonQueryFilter), typeof(JsonQueryPath)), true);
            cases.Add((typeof(JsonQueryLogicalAnd), null), false);
            cases.Add((typeof(JsonQueryLogicalAnd), typeof(JsonQueryFilter)), true);
            cases.Add((typeof(JsonQueryLogicalAnd), typeof(JsonQueryLogicalAnd)), false);
            cases.Add((typeof(JsonQueryLogicalAnd), typeof(JsonQueryLogicalOr)), false);
            cases.Add((typeof(JsonQueryLogicalAnd), typeof(JsonQueryPath)), false);
            cases.Add((typeof(JsonQueryLogicalOr), null), false);
            cases.Add((typeof(JsonQueryLogicalOr), typeof(JsonQueryFilter)), true);
            cases.Add((typeof(JsonQueryLogicalOr), typeof(JsonQueryLogicalAnd)), false);
            cases.Add((typeof(JsonQueryLogicalOr), typeof(JsonQueryLogicalOr)), false);
            cases.Add((typeof(JsonQueryLogicalOr), typeof(JsonQueryPath)), false);
            cases.Add((typeof(JsonQueryPath), null), false);
            cases.Add((typeof(JsonQueryPath), typeof(JsonQueryFilter)), true);
            cases.Add((typeof(JsonQueryPath), typeof(JsonQueryLogicalAnd)), false);
            cases.Add((typeof(JsonQueryPath), typeof(JsonQueryLogicalOr)), false);
            cases.Add((typeof(JsonQueryPath), typeof(JsonQueryPath)), true);
        }

        [TestMethod]
        public void TestThrowIfNotValid()
        {
            var getInstanceOfJsonQueryExpression = new Func<Type?,string, IJsonQueryExpression?>((Type? type, string caseName) =>
            {
                return type switch
                {
                    null => null,
                    Type t when t == typeof(JsonQueryFilter) => new JsonQueryFilter(caseName, JsonQueryFilterOperators.Eq, caseName),
                    Type t when t == typeof(JsonQueryLogicalAnd) => new JsonQueryLogicalAnd(),
                    Type t when t == typeof(JsonQueryLogicalOr) => new JsonQueryLogicalOr(),
                    Type t when t == typeof(JsonQueryPath) => new JsonQueryPath(caseName),
                    _ => throw new NotImplementedException($"Type {type} is not implemented.")
                };
            });

            var cases = this.cases
                .Select((kvp) => new { First = getInstanceOfJsonQueryExpression(kvp.Key.Item1, $"Case_{kvp.Key.Item1}_{kvp.Key.Item2}"), Second = getInstanceOfJsonQueryExpression(kvp.Key.Item2, $"Case_{kvp.Key.Item2}_{kvp.Key.Item1}"), Expected = kvp.Value });
        
            var underTest = new ThrowIfNotValidConsecutiveJsonQueryExpressions();
            foreach (var testCase in cases)
            {
                if (!testCase.Expected)
                {
                    Assert.Throws<ArgumentException>(() => underTest.ThrowIfNotValid(testCase.First, testCase.Second));
                }
                else
                {
                    underTest.ThrowIfNotValid(testCase.First, testCase.Second);
                }
            }
        }
    }
}
