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
        [DynamicData(nameof(GetSequencesAndOutcomes))]
        public void AsJsonPathQueryString(int i, List<IJsonQueryExpression> sequence, string expected)
        {
            var queryBuilder = new JsonQueryBuilder(this.throwIfNotValidMoq.Object);
            int j = 0;
            foreach (IJsonQueryExpression expression in sequence) { 

                queryBuilder = queryBuilder.AddExpression(expression);
                ++j;
            }
            var actual = queryBuilder.AsJsonPathQueryString();

            Assert.AreEqual(expected, actual, message: $"Failed for sequence: {string.Join(", ", sequence.Select(s => s.AsQueryExpressionString()))}");
        }

        public static IEnumerable<(int i, List<IJsonQueryExpression>, string)>  GetSequencesAndOutcomes()
        {
            Mock<IJsonQueryPathExpression> pathExpressionMoq = new Mock<IJsonQueryPathExpression>();
            pathExpressionMoq.Setup(a => a.AsQueryExpressionString()).Returns("Path");
            var pathExpression = pathExpressionMoq.Object;

            Mock<IJsonQueryFilterExpression> filterExpressionMoq = new Mock<IJsonQueryFilterExpression>();
            filterExpressionMoq.Setup(a => a.AsQueryExpressionString()).Returns("Filter");
            var filterExpression = filterExpressionMoq.Object;

            Mock<IJsonQueryLogicalExpression> logicalExpressionMoq = new Mock<IJsonQueryLogicalExpression>();
            logicalExpressionMoq.Setup(a => a.AsQueryExpressionString()).Returns("AndOr");
            var logicalAndOrExpression = logicalExpressionMoq.Object;

            //var pathSequence = new List<IJsonQueryExpression> { pathExpressionMoq.Object };
            //List<IJsonQueryExpression>  pathExpression, pathExpression = [.. pathSequence, pathExpressionMoq.Object ];

            //var filterSequence = new List<IJsonQueryExpression> { filterExpressionMoq.Object };
            //var filterLogicalSequence = new List<IJsonQueryExpression> { filterExpressionMoq.Object, logicalExpressionMoq.Object };


            yield return (1, [pathExpression],"$[Path]");//single path
            yield return (2, [pathExpression, pathExpression], "$[Path][Path]");
            yield return (3, [pathExpression, filterExpression], "$[Path][? Filter]");//path filter 
            yield return (4, [filterExpression], "$[? Filter]" );//single filter
            yield return (5, [filterExpression, logicalAndOrExpression], "$[? Filter]" );//filter with logical operator at end
            yield return (6, [filterExpression, pathExpression], "$[? Filter][Path]" );//filter path
            yield return (7, [pathExpression, pathExpression, filterExpression, logicalAndOrExpression, filterExpression, pathExpression, pathExpression], "$[Path][Path][? Filter AndOr Filter][Path][Path]" );
            yield return (8, [filterExpression, logicalAndOrExpression, filterExpression, pathExpression, pathExpression, filterExpression, logicalAndOrExpression, filterExpression, logicalAndOrExpression], "$[? Filter AndOr Filter][Path][Path][? Filter AndOr Filter]" );

        }

        [TestMethod]
        public void AsJsonPathQueryString_Unknown_IJsonQueryExpression()
        {
            var queryBuilder = this.underTest;
            var expressions = Enumerable.Range(1, 5).Select((i) => { var expressionMoq = new Mock<IJsonQueryExpression>(); expressionMoq.Setup(a => a.AsQueryExpressionString()).Returns($"Expression_{i}"); return expressionMoq.Object; });

            foreach(IJsonQueryExpression expression in expressions) { queryBuilder = queryBuilder.AddExpression(expression); }

            Assert.Throws<InvalidOperationException>(() => this.underTest.AsJsonPathQueryString());

        }
        [TestMethod]
        public void AddExpression_CallsValidator()
        {
            var first = new Mock<IJsonQueryFilterExpression>();
            var second = new Mock<IJsonQueryFilterExpression>();
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
