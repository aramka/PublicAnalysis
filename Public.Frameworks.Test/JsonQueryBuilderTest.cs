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
        private readonly Mock<IThrowIfNotValidConsecutiveJsonQueryExpressions> throwIfNotValidMoq;
        private readonly JsonQueryBuilder underTest;

        public JsonQueryBuilderTest() {

            this.throwIfNotValidMoq = new Mock<IThrowIfNotValidConsecutiveJsonQueryExpressions>();
            this.underTest = new JsonQueryBuilder(throwIfNotValidMoq.Object);
        }

        [TestMethod]
        public void AsJsonPathQueryString()
        {
            var queryBuilder = this.underTest;
            var expressions = Enumerable.Range(1, 5).Select((i) => { var expressionMoq = new Mock<IJsonQueryExpression>(); expressionMoq.Setup(a => a.AsQueryExpressionString()).Returns($"Expression_{i}"); return expressionMoq.Object; });

            foreach(IJsonQueryExpression expression in expressions) { queryBuilder = queryBuilder.AddExpression(expression); }

            var actual = this.underTest.AsJsonPathQueryString();

            var expected = string.Join("", expressions.Select(e => e.AsQueryExpressionString()));
            expected = $"${expected}";
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void AddExpression_CallsValidator()
        {
            var first = new Mock<IJsonQueryExpression>();
            var second = new Mock<IJsonQueryExpression>();
            ArgumentException expectedException = new ArgumentException();
            this.throwIfNotValidMoq.Setup(a => a.ThrowIfNotValid(first.Object, second.Object)).Throws(expectedException);

            this.underTest.AddExpression(first.Object);
            ArgumentException actual = null;
            try
            {
                underTest.AddExpression(second.Object);
            }
            catch (ArgumentException ex) {

                actual = ex;
            }

            actual.Should().Be(expectedException);

            this.throwIfNotValidMoq.Verify(a => a.ThrowIfNotValid(It.IsAny<IJsonQueryExpression>(), It.IsAny<IJsonQueryExpression>()), Times.Exactly(2));

        }
        
    }
}
