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
            StatementNode parentNode = new StatementNode(xsElementMoq.Object, "childAddedTwice", 0);

            Mock<IStatementNode> childNode = new Mock<IStatementNode>();
            childNode.Setup(a => a.ElementId).Returns(new ElementId("child"));

            parentNode.AddChild(childNode.Object.ElementId);

            childNode = new Mock<IStatementNode>();
            childNode.Setup(a => a.ElementId).Returns(new ElementId("child"));

            Assert.Throws<InvalidOperationException>(() => { parentNode.AddChild(childNode.Object.ElementId); });
            
        }
    }
}
