using AwesomeAssertions;
using Moq;
using OpenTelemetry.Context;
using Public.Analysis.FasbTaxonomies.StatementTree;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels.PresentationElementModels;
using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Tests.StatementNodesBuilder
{
    [TestClass]
    public class NodesBuilderTest
    {
        /*
         * record ElementId(IXsElement element) - done
         * LabelsBuilder:ILabelsBuilder
         *  IReadOnlyDictionary<ElementId,string> BuildLabels(labels)
         *  
         * NodeBuilder:INodeBuilder
         *  NodeBuilder(ILabelsBuilder)
         *  IEnumerable<IStatementNode> BuildNodes(IEnumerable<IUSGaapElements> elements, IEnumerable<Loc> locators, LabelLinkBase labels);
         *  
         * DeprecateConcepts:IDeprecateConcepts
         *  DeprecateConcepts(IBuildDeprecatedConcepts deprecatedConceptsBuilder)
         *  IEnumerable<IDeprecatedConcept> DeprecateConcepts(List<IStatementNode> nodes);
         * StatementTreeBuilder:IStatementTreeBuilder
         *  IReadOnlyDictionary<ElementId,IStatementNode> BuildStatementTree(IEnumerable<IStatementNode> nodes);
         */
        public List<(Loc loc, Mock<IXsElement> elementMoq, string label)> BuildElementTestData(int parentCount, string prefix)
        {
            List<(Loc loc, Mock<IXsElement> elementMoq, string label)> l = new List<(Loc loc, Mock<IXsElement> elementMoq, string label)>();
            for (int i = 1; i <= parentCount; i++)
            {
                string label = $"{prefix}_{i}";

                Loc loc = new Loc
                {
                    Href = $"elements#{label}",
                    XLinkLabel = $"loc_{label}"
                };
                var elementMoq = new Mock<IXsElement>();
                elementMoq.Setup(e => e.Id).Returns(loc.LocationAndAnchor.Anchor);
                elementMoq.Setup(e => e.Name).Returns(loc.LocationAndAnchor.Anchor);
                elementMoq.Setup(e => e.Abstract).Returns(i % 2 == 0);
                elementMoq.Setup(e => e.Balance).Returns($"{loc.LocationAndAnchor.Anchor}_balance");
                elementMoq.Setup(e => e.Nillable).Returns(i % 2 == 1);
                elementMoq.Setup(e => e.PeriodType).Returns($"{loc.LocationAndAnchor.Anchor}_periodType");
                elementMoq.Setup(e => e.ElementId).Returns(loc.ElementId);

                l.Add((loc, elementMoq, label));
            }
            return l;
        }

        [TestMethod]
        public void Build()
        {
            // Dictionary<ElementId, IXsElement> elementsByElementId = new Dictionary<ElementId, IXsElement>();

            // Dictionary<ElementId, string> labelsByElementId = new Dictionary<ElementId, string>();

            List<Loc> locs = new List<Loc>();

            int parentCount = 3;
            int childCount = 1;

            var parents = BuildElementTestData(parentCount, "parent").Select(p => new { p.elementMoq, p.label, p.loc });

            var parentChildren = parents.Select((p,i) => new { Parent = p, Children = BuildElementTestData(childCount, $"{p.label}_child").Select(c => new { c.elementMoq, c.label, c.loc }) });

            var parentChildArcs = parentChildren.Select((pc, i) => new { pc.Parent, pc.Children, Arcs = pc.Children.Select((c, j) => new { Parent=pc.Parent, Child = c, From = pc.Parent.loc.XLinkLabel, To = c.loc.XLinkLabel, Order = j }) });

           

            PresentationLink presentationLink = new PresentationLink();
            presentationLink.Arcs = parentChildArcs.SelectMany(pca=>pca.Arcs.Select(a=>new Arc { From = a.From, To = a.To, Order = a.Order })).ToArray();
            presentationLink.Locs = parentChildArcs.SelectMany(pca => pca.Children.Select(c => c.loc).Concat([pca.Parent.loc])).ToArray();

            var elementAndLabels = parentChildArcs.SelectMany(pca => pca.Children.Select(c => new { Element = c.elementMoq.Object, Label = c.label })
            .Concat([new { Element = pca.Parent.elementMoq.Object, Label = pca.Parent.label }])
            );
            var elementsByElementId = elementAndLabels.ToDictionary(e => e.Element.ElementId, e => e.Element);
            var labelsByElementId = elementAndLabels.ToDictionary(e => e.Element.ElementId, e => e.Label);

            NodesBuilder nodeBuilder = new NodesBuilder();
            Dictionary<ElementId, IStatementNode> nodes = nodeBuilder.BuildNodes(presentationLink, elementsByElementId, labelsByElementId);

            var expectation = parentChildArcs.SelectMany((pca, i) => {
                List<StatementNode> nodes = new List<StatementNode>();
                StatementNode parentNode = new StatementNode(pca.Parent.elementMoq.Object, pca.Parent.label, 0);
                nodes.Add(parentNode);
                foreach(var arc in pca.Arcs)
                {
                    StatementNode childNode = new StatementNode(arc.Child.elementMoq.Object, arc.Child.label, arc.Order);
                    childNode.ParentElementId = parentNode.ElementId;
                    parentNode.AddChild(childNode);
                    nodes.Add(childNode);
                }
                return nodes;
            }).ToDictionary(n=>n.XsElement.ElementId);

            nodes.Should().BeEquivalentTo(expectation);
        }

        public (PresentationLink presentationLink, IReadOnlyDictionary<ElementId, IXsElement> elementsByElementId, IReadOnlyDictionary<ElementId,string> labelsByElementId, Dictionary<ElementId, IStatementNode> expectedNodes) BuildNestedNodesTestData(int depth)
        {
            List<Arc> arcs = new List<Arc>();
            List<Loc> locs = new List<Loc>();
            Dictionary<ElementId, IXsElement> elementsByElementId = new Dictionary<ElementId, IXsElement>();
            Dictionary<ElementId, string> labelsByElementId = new Dictionary<ElementId, string>();
            Dictionary<ElementId, IStatementNode> expectedNodes = new Dictionary<ElementId, IStatementNode>();

            (Loc parentLoc, Mock<IXsElement> parentElementMoq, string parentLabel) = BuildElementTestData(1, "parent").Single();

            locs.Add(parentLoc);
            elementsByElementId[parentElementMoq.Object.ElementId] = parentElementMoq.Object;
            labelsByElementId[parentElementMoq.Object.ElementId] = parentLabel;
            StatementNode parentNode = new StatementNode(parentElementMoq.Object, parentLabel, 0);
            expectedNodes[parentElementMoq.Object.ElementId] = parentNode;

            var parent = new { loc=parentLoc, parentElementMoq, parentLabel, parentNode }; // BuildElementTestData(1, "parent").Single();

            for(int i = 1; i <= depth; i++)
            {
                (Loc childLoc, Mock<IXsElement> childElementMoq, string childLabel) = BuildElementTestData(1, $"child_{i}").Single();
                
                var parentChildArc = new Arc();
                parentChildArc.From = parent.loc.XLinkLabel;
                parentChildArc.To = childLoc.XLinkLabel;
                parentChildArc.Order = 1;

                StatementNode childNode = new StatementNode(childElementMoq.Object, childLabel, 1);
                childNode.ParentElementId = parent.parentNode.ElementId;
                parent.parentNode.AddChild(childNode);

                arcs.Add(parentChildArc);
                locs.Add(childLoc);
                
                elementsByElementId[childElementMoq.Object.ElementId] = childElementMoq.Object;
                labelsByElementId[childElementMoq.Object.ElementId] = childLabel;
                expectedNodes[childElementMoq.Object.ElementId] = childNode;

                parent = new { loc = childLoc, parentElementMoq = childElementMoq, parentLabel = childLabel, parentNode = childNode };

            }

            PresentationLink presentationLink = new PresentationLink
            {
                Arcs = arcs.ToArray(),
                Locs = locs.ToArray()

            };

            //(Loc child1Loc, Mock<IXsElement> child1ElementMoq, string child1Label) = BuildElementTestData(1, "child_1").Single();
            //(Loc child2Loc, Mock<IXsElement> child2ElementMoq, string child2Label) = BuildElementTestData(1, "child_2").Single();

            //var arc1 = new Arc();
            //arc1.From = parentLoc.XLinkLabel;
            //arc1.To = child1Loc.XLinkLabel;
            //arc1.Order = 1;

            //var arc2 = new Arc();
            //arc2.From = child1Loc.XLinkLabel;
            //arc2.To = child2Loc.XLinkLabel;
            //arc2.Order = 1;

            //PresentationLink presentationLink = new PresentationLink();
            //presentationLink.Arcs = [arc1, arc2];
            //presentationLink.Locs = [parentLoc, child1Loc, child2Loc];

            //IReadOnlyDictionary<ElementId, IXsElement> elementsByElementId = new Dictionary<ElementId, IXsElement>
            //{
            //    [parentElementMoq.Object.ElementId] = parentElementMoq.Object,
            //    [child1ElementMoq.Object.ElementId] = child1ElementMoq.Object,
            //    [child2ElementMoq.Object.ElementId] = child2ElementMoq.Object
            //};
            //IReadOnlyDictionary<ElementId, string> labelsByElementId = new Dictionary<ElementId, string>
            //{
            //    [parentElementMoq.Object.ElementId] = parentLabel,
            //    [child1ElementMoq.Object.ElementId] = child1Label,
            //    [child2ElementMoq.Object.ElementId] = child2Label
            //};

            //StatementNode parentNode = new StatementNode(parentElementMoq.Object, parentLabel, 0);

            //StatementNode child1Node = new StatementNode(child1ElementMoq.Object, child1Label, 1);
            //parentNode.AddChild(child1Node);
            //child1Node.ParentElementId = parentNode.ElementId;

            //StatementNode child2Node = new StatementNode(child2ElementMoq.Object, child2Label, 1);
            //child1Node.AddChild(child2Node);
            //child2Node.ParentElementId = child1Node.ElementId;


            //var expectedNodes = new Dictionary<ElementId, IStatementNode>
            //{
            //    [parentNode.ElementId] = parentNode,
            //    [child1Node.ElementId] = child1Node,
            //    [child2Node.ElementId] = child2Node
            //};

            return (presentationLink, elementsByElementId, labelsByElementId, expectedNodes);
        }

        [TestMethod(DisplayName ="Build: Parent->Child->Child")]
        public void Build_Depth3()
        {
            (PresentationLink presentationLink, IReadOnlyDictionary<ElementId, IXsElement> elementsByElementId, IReadOnlyDictionary<ElementId, string> labelsByElementId, Dictionary<ElementId, IStatementNode> expectedNodes) = BuildNestedNodesTestData(3);

            NodesBuilder nodesBuilder = new NodesBuilder();
            Dictionary<ElementId, IStatementNode> actualNodes = nodesBuilder.BuildNodes(presentationLink, elementsByElementId, labelsByElementId);

            actualNodes.Should().BeEquivalentTo(expectedNodes);
        }
    }
}
