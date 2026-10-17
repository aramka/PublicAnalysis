using AwesomeAssertions;
using Moq;
using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Statement
{

    [TestClass]
    public class StatementTreeTrimmerTests
    {
        [TestMethod]
        public void RemoveBranch()
        {
            //remove the node and all its successors
        }

        [TestMethod]
        public void RemoveNodeMoveChildrenUp_1()
        {
            Mock<IXsElement> xsElementMoq = new Mock<IXsElement>();
            xsElementMoq.Setup(a => a.ElementId).Returns(new ElementId("to_remove"));
            IStatementNode toRemove = new StatementNode(xsElementMoq.Object, "to_remove");

            Dictionary<ElementId, IStatementNode> tree = new IStatementNode[] { toRemove }.ToDictionary(n => n.Id);

            StatementTreeTrimmer treeTrimmer = new StatementTreeTrimmer();
            var actual = treeTrimmer.RemoveNodeMoveChildrenUp(toRemove.Id, tree);

            actual.Should().BeEmpty();
        }

        [TestMethod]
        public void RemoveNodeMoveChildrenUp_2()
        {
            Mock<IXsElement> xsElementMoq = new Mock<IXsElement>();
            xsElementMoq.Setup(a => a.ElementId).Returns(new ElementId("to_remove"));
            IStatementNode toRemove = new StatementNode(xsElementMoq.Object, "to_remove");

            string childLabel = "child_of_toRemove";
            var childrenOfToRemove = BuildNodes(2, childLabel,
                (childNode, i) =>
                {
                    childNode.AddParent(toRemove.Id);
                    toRemove.AddChild(childNode.Id, childNode.Id.ToString(), i);
                }
                );
            Dictionary<ElementId, IStatementNode> tree = childrenOfToRemove.Concat([toRemove]).ToDictionary(n => n.Id);

            StatementTreeTrimmer treeTrimmer = new StatementTreeTrimmer();
            var actual = treeTrimmer.RemoveNodeMoveChildrenUp(toRemove.Id, tree);

            var expected = BuildNodes(2, childLabel, (_, _) => { }).ToDictionary(n=>n.Id);

            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void RemoveNodeMoveChildrenUp_3()
        {
            //remove the node and move its successors up to the parent

            Mock<IXsElement> xsElementMoq = new Mock<IXsElement>();
            xsElementMoq.Setup(a => a.ElementId).Returns(new ElementId("to_remove"));
            IStatementNode toRemove = new StatementNode(xsElementMoq.Object, "to_remove");

            string childLabel = "child_of_toRemove";
            var childrenOfToRemove = BuildNodes(2, childLabel,
                (childNode, i) =>
                {
                    childNode.AddParent(toRemove.Id);
                    toRemove.AddChild(childNode.Id, childNode.Id.ToString(), i);
                }
                );
            string parentLabel = "parent_of_ToRemove";
            var parentsOfToRemoveMoqs = BuildNodes(2, parentLabel, (parent, i) =>
            {
                parent.AddChild(toRemove.Id, toRemove.Id.ToString(), 1);
                toRemove.AddParent(parent.Id);
            });
            Dictionary<ElementId, IStatementNode> tree = parentsOfToRemoveMoqs.Concat(childrenOfToRemove).Concat([toRemove]).ToDictionary(n => n.Id);


            StatementTreeTrimmer treeTrimmer = new StatementTreeTrimmer();
            var actual = treeTrimmer.RemoveNodeMoveChildrenUp(toRemove.Id, tree);

            var expectedChildNodes = BuildNodes(2, childLabel, (_, _) => { });
            var expectedParentNodes = BuildNodes(2, parentLabel, (parent, i) => {

                int j = 0;
                foreach(var expectedChild in expectedChildNodes)
                {
                    parent.AddChild(expectedChild.Id, expectedChild.Id.ToString(), ++j);
                    expectedChild.AddParent(parent.Id);
                }
            
            });
            var expected = expectedParentNodes.Concat(expectedChildNodes).ToDictionary(n => n.Id);

            actual.Should().BeEquivalentTo(expected);
        }
        List<IStatementNode> BuildNodes(int count, string idPrefix, Action<IStatementNode, int> sideEffect)
        {
            return Enumerable.Range(1, count)
                .Select((i) =>
                {
                    Mock<IXsElement> xsElementMoq = new Mock<IXsElement>();
                    xsElementMoq.Setup(a => a.ElementId).Returns(new ElementId($"{idPrefix}_{i}"));

                    IStatementNode node = new StatementNode(xsElementMoq.Object, xsElementMoq.Object.ElementId.ToString());

                    sideEffect(node,i);

                    return node;
                }).ToList();
        }
    }
}
