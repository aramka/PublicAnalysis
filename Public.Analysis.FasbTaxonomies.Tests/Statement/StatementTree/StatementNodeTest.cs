using AwesomeAssertions;
using Moq;
using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Tests.Statement.StatementTree
{
    [TestClass]
    public class StatementNodeTest
    {
        [TestMethod]
        public void ChildAddedTwice()
        {
            Mock<IXsElement> xsElementMoq = new Mock<IXsElement>();
            StatementNode parentNode = new StatementNode(xsElementMoq.Object, "childAddedTwice");

            parentNode.AddChild(new ElementId("child"), "child",1);


            Assert.Throws<InvalidOperationException>(() => parentNode.AddChild(new ElementId("child"), "child", 1));
            
        }
        [TestMethod]
        public void ParentAddedTwice()
        {
            Mock<IXsElement> xsElementMoq = new Mock<IXsElement>();
            StatementNode childNode = new StatementNode(xsElementMoq.Object, "parentAddedTwice");

            childNode.AddParent(new ElementId("parent"));

            childNode.ParentsElementIds.Should().BeEquivalentTo([new ElementId("parent")]);

        }

    }
}
