using AwesomeAssertions;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.CodeCoverage;
using Moq;
using OpenTelemetry.Context;
using OpenTelemetry.Trace;
using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels.PresentationElementModels;
using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Tests.Statement.StatementTree
{
    [TestClass]
    public class NodesBuilderTest
    {
        /*
         * record ElementId(IXsElement element) - done
         * LabelsBuilder:ILabelsBuilder - done
         *  IReadOnlyDictionary<ElementId,string> BuildLabels(labels) 
         * NodeBuilder:INodeBuilder - done
         *  NodeBuilder(ILabelsBuilder)
         *  IEnumerable<IStatementNode> BuildNodes(IEnumerable<IUSGaapElements> elements, IEnumerable<Loc> locators, LabelLinkBase labels);
         *  
         * DeprecateConcepts:IDeprecateConcepts
         *  DeprecateConcepts(IBuildDeprecatedConcepts deprecatedConceptsBuilder)
         *  IEnumerable<IDeprecatedConcept> DeprecateConcepts(List<IStatementNode> nodes);
         *  
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
            BuildNestedNodesTestData(int[] nodesAtDepth)
        {
            List<Arc> arcs = new List<Arc>();
            List<Loc> locs = new List<Loc>();
            Dictionary<ElementId, IXsElement> elementsByElementId = new Dictionary<ElementId, IXsElement>();
            Dictionary<ElementId, string> labelsByElementId = new Dictionary<ElementId, string>();
            Dictionary<ElementId, IStatementNode> expectedNodes = new Dictionary<ElementId, IStatementNode>();

            var parents = Enumerable.Range(0, nodesAtDepth[0])
                .Select((i) => {

                    (Loc parentLoc, Mock<IXsElement> parentElementMoq, string parentLabel) = BuildElementTestData(1, $"node_0_{i+1}");

                    locs.Add(parentLoc);
                    elementsByElementId[parentElementMoq.Object.ElementId] = parentElementMoq.Object;
                    labelsByElementId[parentElementMoq.Object.ElementId] = parentLabel;
                    StatementNode parentNode = new StatementNode(parentElementMoq.Object, parentLabel, 0);
                    expectedNodes[parentElementMoq.Object.ElementId] = parentNode;

                    return new { loc = parentLoc, parentElementMoq, parentLabel, parentNode };

                }).ToList();
         

            for(int i = 1; i < nodesAtDepth.Length; i++)
            {
                var nextParents = parents.Take(0).ToList();
                foreach (var parent in parents)
                {
                    for (int j = 0; j < nodesAtDepth[i]; j++) {
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

        [TestMethod()]
        [DynamicData(nameof(GetNodesAtDepth))]
        public void Build_Nodes(int[] nodesByDepth)
        {
            // int[] nodesByDepth = [ 2,  1,  3 ];
            (PresentationLink presentationLink, IReadOnlyDictionary<ElementId, IXsElement> elementsByElementId, IReadOnlyDictionary<ElementId, string> labelsByElementId, Dictionary<ElementId, IStatementNode> expectedNodes) = 
                BuildNestedNodesTestData(nodesByDepth);

            NodesBuilder nodesBuilder = new NodesBuilder();
            Dictionary<ElementId, IStatementNode> actualNodes = nodesBuilder.BuildNodes(presentationLink, elementsByElementId, labelsByElementId);

            actualNodes.Should().BeEquivalentTo(expectedNodes);
        }

        private static IEnumerable<int[]> GetNodesAtDepth()
        {
            yield return [1];
            yield return [2];
            yield return [2, 1];
            yield return [2, 2];
            yield return [2, 1, 1];
            yield return [2, 1, 2];
            yield return [2, 1, 3];
            yield return [2, 2, 1];
            yield return [2, 2, 2];
            yield return [2, 2, 3];
        }
    }
}
