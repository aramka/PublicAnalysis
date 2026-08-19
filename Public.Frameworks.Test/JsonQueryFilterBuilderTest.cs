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
    public class JsonQueryFilterBuilderTest
    {
        public JsonQueryFilterBuilderTest() {
        }
        [TestMethod]
        public void AddPath()
        {
            // Arrange
            var builder = new JsonQueryFilterBuilder();
            // Act
            builder.AddPath("somePath").AddPath("anotherPath");

            Assert.HasCount(2, builder.Paths);
            builder.Paths.Should().BeEquivalentTo(new List<string> { "['somePath']", "['anotherPath']" });
        }
        [TestMethod]
        public void AddFilter()
        {
            var filters = Enumerable.Range(1, 3).Select(i => { 
                var moq = new Mock<IJsonQueryFilterExpression>(); 
                moq.Setup(f => f.AsJsonPathQueryExpression()).Returns($"Filter_{i.ToString()}"); return moq.Object; 
            });

            var builder = new JsonQueryFilterBuilder();
            foreach (var filter in filters)
            {
                builder = builder.AddFilter(filter);
            }

            builder.Filters.Should().BeEquivalentTo(filters.Select(f => f.AsJsonPathQueryExpression()));
        }
        [TestMethod]
        public void AddFilter_NullFilter_ThrowsArgumentNullException()
        {
            var builder = new JsonQueryFilterBuilder();
            Action act = () => builder.AddFilter(null);
            act.Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void AddFilter_EmptyFilter_ThrowsArgumentException()
        {
            var mockFilter = new Mock<IJsonQueryFilterExpression>();
            mockFilter.Setup(f => f.AsJsonPathQueryExpression()).Returns(string.Empty);
            var builder = new JsonQueryFilterBuilder();
            Action act = () => builder.AddFilter(mockFilter.Object);
            act.Should().Throw<ArgumentException>();
        }
        [TestMethod]
        public void AddFilter_WhitespaceFilter_ThrowsArgumentException()
        {
            var mockFilter = new Mock<IJsonQueryFilterExpression>();
            mockFilter.Setup(f => f.AsJsonPathQueryExpression()).Returns("   ");
            var builder = new JsonQueryFilterBuilder();
            Action act = () => builder.AddFilter(mockFilter.Object);
            act.Should().Throw<ArgumentException>();
        }
        [TestMethod]
        public void ToString() {
            // Arrange
            var builder = new JsonQueryFilterBuilder();
            // Act
            builder.AddPath("somePath").AddPath("anotherPath");

            var filters = Enumerable.Range(1, 3).Select(i => {
                var moq = new Mock<IJsonQueryFilterExpression>();
                moq.Setup(f => f.AsJsonPathQueryExpression()).Returns($"Filter_{i.ToString()}"); return moq.Object;
            });
            var actual = builder.AsJsonPathQueryString();

            var expected = string.Join(string.Empty, builder.Paths);
            expected = $"{expected}[? {string.Join(" ", filters.Select(f => f.AsJsonPathQueryExpression()))}]";
            actual.Should().Be(expected);
        }
    }
}
