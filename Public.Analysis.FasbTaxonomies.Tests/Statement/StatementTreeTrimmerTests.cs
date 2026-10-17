using AwesomeAssertions;
using Moq;
using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;

namespace Public.Analysis.FasbTaxonomies.Statement
{

    [TestClass]
    public class StatementTreeTrimmerTests
    {

        [TestMethod]
        public void RemoveBranch()
        {
            //remove the node and all its successors

            var tree = BuildTree([2, 2, 1]);
            
            var roots = tree.Where(kvp => !kvp.Value.ParentsElementIds.Any()).Select(kvp => kvp.Value).ToList();

            var secondRoot = roots[1];
            var firstRootChild = roots[0].Children.ElementAt(0);
            secondRoot.AddChild(firstRootChild.Id, $"{firstRootChild.Id.ToString()}_put_in_second_root", firstRootChild.Order);
            tree[firstRootChild.Id].AddParent(secondRoot.Id);

            HashSet<ElementId> removedBranchNodes = new ();
            var queue = new Queue<IStatementNode>([roots[0]]);
            while (queue.Any())
            {
                var node = queue.Dequeue();
                removedBranchNodes.Add(node.Id);
                foreach(var child in node.Children)
                {
                    queue.Enqueue(tree[child.Id]);
                }
            }

            StatementTreeTrimmer underTest = new StatementTreeTrimmer();
            IReadOnlyDictionary<ElementId, IStatementNode> actual = underTest.RemoveBranch(roots[0].Id, tree);

            var expected = BuildTree([2, 2, 1])
                .Select(kvp => kvp.Value)
                .Where(n => !removedBranchNodes.Contains(n.Id))
                .ToDictionary(n => n.Id);

            actual.Should().BeEquivalentTo(expected);
            
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

        public Dictionary<ElementId, IStatementNode> BuildTree(int[] nodesAtDepth)
        {
            if(nodesAtDepth is null || nodesAtDepth.Length == 0)
            {
                return new Dictionary<ElementId, IStatementNode>();
            }
            var roots = BuildNodes(nodesAtDepth[0], "root", (_, _) => { });

            if (nodesAtDepth.Length == 1)
            {
                return roots.ToDictionary(n => n.Id);
            }
            var allNodes = new List<IStatementNode>();
            allNodes.AddRange(roots);
            var parents = roots;
            for(int i = 1; i < nodesAtDepth.Length; i++)
            {
                var nextParents = new List<IStatementNode>();

                foreach(var parent in parents)
                {
                    var children = BuildNodes(nodesAtDepth[i], $"{parent.Id}_child", (child, j) =>
                    {
                        child.AddParent(parent.Id);
                        parent.AddChild(child.Id, child.Id.ToString(), j);
                    });
                    nextParents.AddRange(children);
                    allNodes.AddRange(children);
                }
                parents = nextParents;
            }
            return allNodes.ToDictionary(n => n.Id);
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
