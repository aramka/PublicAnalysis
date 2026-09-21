using AwesomeAssertions;
using Moq;
using OpenTelemetry.Context;
using OpenTelemetry.Trace;
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
        public (Loc loc, Mock<IXsElement> elementMoq, string label) BuildElementTestData(int parentCount, string label)
        {
            Loc loc = new Loc
            {
                Href = $"elements#{label}",
                XLinkLabel = $"loc_{label}"
            };
            var elementMoq = new Mock<IXsElement>();
            elementMoq.Setup(e => e.Id).Returns(loc.LocationAndAnchor.Anchor);
            elementMoq.Setup(e => e.Name).Returns(loc.LocationAndAnchor.Anchor);
            elementMoq.Setup(e => e.Abstract).Returns(DateTime.Now.Millisecond % 2 == 0);
            elementMoq.Setup(e => e.Balance).Returns($"{loc.LocationAndAnchor.Anchor}_balance");
            elementMoq.Setup(e => e.Nillable).Returns(DateTime.Now.Millisecond % 2 == 1);
            elementMoq.Setup(e => e.PeriodType).Returns($"{loc.LocationAndAnchor.Anchor}_periodType");
            elementMoq.Setup(e => e.ElementId).Returns(loc.ElementId);

            return (loc, elementMoq, label);
        }

        public (PresentationLink presentationLink, IReadOnlyDictionary<ElementId, IXsElement> elementsByElementId, IReadOnlyDictionary<ElementId,string> labelsByElementId, Dictionary<ElementId, IStatementNode> expectedNodes) 
            BuildNestedNodesTestData(int depth, Dictionary<int,int> nodesByDepth)
        {
            List<Arc> arcs = new List<Arc>();
            List<Loc> locs = new List<Loc>();
            Dictionary<ElementId, IXsElement> elementsByElementId = new Dictionary<ElementId, IXsElement>();
            Dictionary<ElementId, string> labelsByElementId = new Dictionary<ElementId, string>();
            Dictionary<ElementId, IStatementNode> expectedNodes = new Dictionary<ElementId, IStatementNode>();

            int nodesAtDepth = nodesByDepth[0];

            var parents = Enumerable.Range(0, nodesAtDepth)
                .Select((i) => {

                    (Loc parentLoc, Mock<IXsElement> parentElementMoq, string parentLabel) = BuildElementTestData(1, $"node_0_{i+1}");

                    locs.Add(parentLoc);
                    elementsByElementId[parentElementMoq.Object.ElementId] = parentElementMoq.Object;
                    labelsByElementId[parentElementMoq.Object.ElementId] = parentLabel;
                    StatementNode parentNode = new StatementNode(parentElementMoq.Object, parentLabel, 0);
                    expectedNodes[parentElementMoq.Object.ElementId] = parentNode;

                    return new { loc = parentLoc, parentElementMoq, parentLabel, parentNode };

                }).ToList();
         

            for(int i = 1; i < depth; i++)
            {
                nodesAtDepth = nodesByDepth[i];
                var nextParents = parents.Take(0).ToList();
                foreach (var parent in parents)
                {
                    for (int j = 0; j < nodesAtDepth; j++) {
                        (Loc childLoc, Mock<IXsElement> childElementMoq, string childLabel) = BuildElementTestData(1, $"{parent.parentLabel}_node1_{i}_{j}");

                        var parentChildArc = new Arc
                        {
                            From = parent.loc.XLinkLabel,
                            To = childLoc.XLinkLabel,
                            Order = 1
                        };

                        StatementNode childNode = new StatementNode(childElementMoq.Object, childLabel, 1)
                        {
                            ParentElementId = parent.parentNode.ElementId
                        };
                        parent.parentNode.AddChild(childNode);

                        arcs.Add(parentChildArc);
                        locs.Add(childLoc);

                        elementsByElementId[childElementMoq.Object.ElementId] = childElementMoq.Object;
                        labelsByElementId[childElementMoq.Object.ElementId] = childLabel;
                        expectedNodes[childElementMoq.Object.ElementId] = childNode;

                        nextParents.Add(new { loc = childLoc, parentElementMoq = childElementMoq, parentLabel = childLabel, parentNode = childNode });
                    }
                }

                parents = nextParents;
            }

            PresentationLink presentationLink = new PresentationLink
            {
                Arcs = arcs.ToArray(),
                Locs = locs.ToArray()

            };

            return (presentationLink, elementsByElementId, labelsByElementId, expectedNodes);
        }

        [TestMethod(DisplayName ="Build: Parent->Child->Child")]
        public void Build_Depth3()
        {
            Dictionary<int, int> nodesByDepth = new Dictionary<int, int> { [0] = 2, [1] = 1, [2] = 3 };
            (PresentationLink presentationLink, IReadOnlyDictionary<ElementId, IXsElement> elementsByElementId, IReadOnlyDictionary<ElementId, string> labelsByElementId, Dictionary<ElementId, IStatementNode> expectedNodes) = 
                BuildNestedNodesTestData(3, nodesByDepth);

            NodesBuilder nodesBuilder = new NodesBuilder();
            Dictionary<ElementId, IStatementNode> actualNodes = nodesBuilder.BuildNodes(presentationLink, elementsByElementId, labelsByElementId);

            actualNodes.Should().BeEquivalentTo(expectedNodes);
        }
    }
}
