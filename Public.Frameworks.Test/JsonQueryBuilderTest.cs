using AwesomeAssertions;
using Moq;
using Public.Frameworks.JsonQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Public.Frameworks.Tests
{
    [TestClass]
    public class JsonQueryBuilderTest
    {


        [TestMethod]
        [DynamicData(nameof(GetSequencesAndOutcomes))]
        public void AsJsonPathQueryString(int i, List<IJsonQueryExpression> sequence, string expected)
        {
            var queryBuilder = new JsonQueryBuilder();
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
        [DynamicData(nameof(InvalidSequences))]
        public void AddExpression_InvalidSequences(IEnumerable<IJsonQueryExpression?> invalidSequence)
        {
            var queryBuilder = new JsonQueryBuilder();
            ArgumentException actual = null;
            try
            {
                foreach (var expression in invalidSequence)
                {
                    queryBuilder.AddExpression(expression);
                }

            }
            catch (ArgumentException ex)
            {
                actual = ex;
            }

            Assert.IsNotNull(actual);

        }

        public static IEnumerable<IEnumerable<IJsonQueryExpression?>> InvalidSequences()
        {
            yield return new IJsonQueryExpression?[] { null };
            yield return new IJsonQueryExpression?[] { new Mock<IJsonQueryFilterExpression>().Object, null };
            yield return new IJsonQueryExpression?[] { new Mock<IJsonQueryFilterExpression>().Object, new Mock<IJsonQueryFilterExpression>().Object };
            yield return new IJsonQueryExpression?[] { new Mock<IJsonQueryLogicalExpression>().Object };
            yield return new IJsonQueryExpression?[] { new Mock<IJsonQueryPathExpression>().Object, null };
            yield return new IJsonQueryExpression?[] { new Mock<IJsonQueryPathExpression>().Object, new Mock<IJsonQueryLogicalExpression>().Object };
            yield return new IJsonQueryExpression?[] { new Mock<IJsonQueryExpression>().Object };
        }
        
    }
}
