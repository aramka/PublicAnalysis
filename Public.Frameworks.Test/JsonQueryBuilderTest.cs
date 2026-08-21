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

        public static List<(int i, List<IJsonQueryExpression>, string)>  GetSequencesAndOutcomes()
        {
            Mock<IJsonQueryPathExpression> pathExpressionMoq = new Mock<IJsonQueryPathExpression>();
            pathExpressionMoq.Setup(a => a.AsQueryExpressionString()).Returns("Path");
            Mock<IJsonQueryFilterExpression> filterExpressionMoq = new Mock<IJsonQueryFilterExpression>();
            filterExpressionMoq.Setup(a => a.AsQueryExpressionString()).Returns("Filter");
            Mock<IJsonQueryLogicalExpression> logicalExpressionMoq = new Mock<IJsonQueryLogicalExpression>();
            logicalExpressionMoq.Setup(a => a.AsQueryExpressionString()).Returns("AndOr");

            var pathSequence = new List<IJsonQueryExpression> { pathExpressionMoq.Object };
            List<IJsonQueryExpression>  pathPathSequence = [.. pathSequence, pathExpressionMoq.Object ];

            var filterSequence = new List<IJsonQueryExpression> { filterExpressionMoq.Object };
            var filterLogicalSequence = new List<IJsonQueryExpression> { filterExpressionMoq.Object, logicalExpressionMoq.Object };
            IJsonQueryExpression[] pathPathFilterLogicalFilterLogicalPathPathSequence = [.. pathPathSequence, .. filterLogicalSequence, .. filterSequence, .. pathPathSequence];
            IJsonQueryExpression[] filterLogicalFilterLogicalPathPathFilterLogicalFilterLogicalSequence = [.. filterLogicalSequence, .. filterSequence, .. pathPathSequence, .. filterLogicalSequence, .. filterLogicalSequence];

            var sequencesAndOutcomes = Enumerable.Empty<int>().Select((_) => new { Sequence = new List<IJsonQueryExpression>(), Expected = string.Empty }).ToList();

            sequencesAndOutcomes.Add(new { Sequence = pathPathSequence[0..1], Expected = "$[Path]" });//single path
            sequencesAndOutcomes.Add(new { Sequence = pathPathSequence, Expected = "$[Path][Path]" });
            List<IJsonQueryExpression> pathFilterSequence = [ ..pathPathSequence[0..1], ..filterLogicalSequence[0..1]];
            sequencesAndOutcomes.Add(new { Sequence = pathFilterSequence, Expected = "$[Path][? Filter]" });//path filter 

            sequencesAndOutcomes.Add(new { Sequence = filterSequence, Expected = "$[? Filter]" });//single filter
            sequencesAndOutcomes.Add(new { Sequence = filterLogicalSequence, Expected = "$[? Filter]" });//filter with logical operator at end
            List<IJsonQueryExpression> filterPathSequence = [.. filterSequence, .. pathSequence];
            sequencesAndOutcomes.Add(new { Sequence = filterPathSequence, Expected = "$[? Filter][Path]" });//filter path

            sequencesAndOutcomes.Add(new { Sequence = pathPathFilterLogicalFilterLogicalPathPathSequence.ToList(), Expected = "$[Path][Path][? Filter AndOr Filter][Path][Path]" });
            sequencesAndOutcomes.Add(new { Sequence = filterLogicalFilterLogicalPathPathFilterLogicalFilterLogicalSequence.ToList(), Expected = "$[? Filter AndOr Filter][Path][Path][? Filter AndOr Filter]" });

            return sequencesAndOutcomes.Select((a,i) => (i+1, a.Sequence, a.Expected)).ToList();
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
