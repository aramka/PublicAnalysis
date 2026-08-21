using Moq;
using Public.Frameworks.JsonQuery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.Tests
{
    [TestClass]
    public class ThrowIfNotValidConsecutiveJsonQueryExpressionsTest
    {
  
        [TestMethod]
        [DynamicData(nameof(GetTestCases))]
        public void TestThrowIfNotValid(IJsonQueryExpression? first, IJsonQueryExpression? second, bool expected)
        {
            var getInstanceOfJsonQueryExpression = new Func<Type?, string, IJsonQueryExpression?>((Type? type, string caseName) =>
            {
                return type switch
                {
                    null => null,
                    Type t when t == typeof(JsonQueryFilter) => new Mock<IJsonQueryFilterExpression>().Object,
                    Type t when t == typeof(JsonQueryLogicalAnd) => new Mock<IJsonQueryLogicalExpression>().Object,
                    Type t when t == typeof(JsonQueryLogicalOr) => new Mock<IJsonQueryLogicalExpression>().Object,
                    Type t when t == typeof(JsonQueryPath) => new Mock<IJsonQueryPathExpression>().Object,
                    _ => throw new NotImplementedException($"Type {type} is not implemented.")
                };
            });

            var underTest = new ThrowIfNotValidConsecutiveJsonQueryExpressions();
            if (!expected)
            {
                Assert.Throws<ArgumentException>(() => underTest.ThrowIfNotValid(first, second));
            }
            else
            {
                underTest.ThrowIfNotValid(first, second);
            }
        }

        private static IEnumerable<(IJsonQueryExpression? first, IJsonQueryExpression? second, bool expected)> GetTestCases()
        {

            yield return (null, null, false);
            yield return (null,new Mock<IJsonQueryFilterExpression>().Object, true);
            yield return (null, new Mock<IJsonQueryLogicalExpression>().Object, false);
            yield return (null, new Mock<IJsonQueryLogicalExpression>().Object, false);
            yield return (null, new Mock<IJsonQueryPathExpression>().Object, true);
            yield return (new Mock<IJsonQueryFilterExpression>().Object, null, false);
            yield return (new Mock<IJsonQueryFilterExpression>().Object, new Mock<IJsonQueryFilterExpression>().Object, false);
            yield return (new Mock<IJsonQueryFilterExpression>().Object, new Mock<IJsonQueryLogicalExpression>().Object, true);
            yield return (new Mock<IJsonQueryFilterExpression>().Object, new Mock<IJsonQueryLogicalExpression>().Object, true);
            yield return (new Mock<IJsonQueryFilterExpression>().Object, new Mock<IJsonQueryPathExpression>().Object, true);
            yield return (new Mock<IJsonQueryLogicalExpression>().Object, null, false);
            yield return (new Mock<IJsonQueryLogicalExpression>().Object, new Mock<IJsonQueryFilterExpression>().Object, true);
            yield return (new Mock<IJsonQueryLogicalExpression>().Object, new Mock<IJsonQueryLogicalExpression>().Object, false);
            yield return (new Mock<IJsonQueryLogicalExpression>().Object, new Mock<IJsonQueryLogicalExpression>().Object, false);
            yield return (new Mock<IJsonQueryLogicalExpression>().Object, new Mock<IJsonQueryPathExpression>().Object, false);
            yield return (new Mock<IJsonQueryLogicalExpression>().Object, null, false);
            yield return (new Mock<IJsonQueryLogicalExpression>().Object, new Mock<IJsonQueryFilterExpression>().Object, true);
            yield return (new Mock<IJsonQueryLogicalExpression>().Object, new Mock<IJsonQueryLogicalExpression>().Object, false);
            yield return (new Mock<IJsonQueryLogicalExpression>().Object, new Mock<IJsonQueryLogicalExpression>().Object, false);
            yield return (new Mock<IJsonQueryLogicalExpression>().Object, new Mock<IJsonQueryPathExpression>().Object, false);
            yield return (new Mock<IJsonQueryPathExpression>().Object, null, false);
            yield return (new Mock<IJsonQueryPathExpression>().Object, new Mock<IJsonQueryFilterExpression>().Object, true);
            yield return (new Mock<IJsonQueryPathExpression>().Object, new Mock<IJsonQueryLogicalExpression>().Object, false);
            yield return (new Mock<IJsonQueryPathExpression>().Object, new Mock<IJsonQueryLogicalExpression>().Object, false);
            yield return (new Mock<IJsonQueryPathExpression>().Object, new Mock<IJsonQueryPathExpression>().Object, true);
        }
    }
}
