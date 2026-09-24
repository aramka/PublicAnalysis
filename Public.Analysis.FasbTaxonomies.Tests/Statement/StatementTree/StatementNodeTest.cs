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
            StatementNode node = new StatementNode(xsElementMoq.Object, "childAddedTwice", 0);
            Mock<IStatementNode> childNode = new Mock<IStatementNode>();
            childNode.Setup(a => a.ElementId).Returns(new ElementId("child"));

            node.AddChild(childNode.Object);

            childNode = new Mock<IStatementNode>();
            childNode.Setup(a => a.ElementId).Returns(new ElementId("child"));

            Assert.Throws<InvalidOperationException>(() => { node.AddChild(childNode.Object); });
            
        }
    }
}
