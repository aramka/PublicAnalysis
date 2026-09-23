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
        public void StatementNodeParentSetOnce()
        {
            Mock<IXsElement> xsElementMoq = new Mock<IXsElement>();
            StatementNode node = new StatementNode(xsElementMoq.Object, "parentSetOnlyOnce", 0);
            node.ParentElementId = new ElementId("firstElementId");
            Assert.Throws<InvalidOperationException>(() => { node.ParentElementId = new ElementId("secondElementId"); });
            
        }
    }
}
