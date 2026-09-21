using AwesomeAssertions;
using Moq;
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
                Loc loc = new Loc
                {
                    Href = $"elements#{prefix}{i}",
                    XLinkLabel = $"loc_{prefix}{i}"
                };
                var elementMoq = new Mock<IXsElement>();
                elementMoq.Setup(e => e.Id).Returns(loc.LocationAndAnchor.Anchor);
                elementMoq.Setup(e => e.Name).Returns(loc.LocationAndAnchor.Anchor);
                elementMoq.Setup(e => e.Abstract).Returns(i % 2 == 0);
                elementMoq.Setup(e => e.Balance).Returns($"{loc.LocationAndAnchor.Anchor}_balance");
                elementMoq.Setup(e => e.Nillable).Returns(i % 2 == 1);
                elementMoq.Setup(e => e.PeriodType).Returns($"{loc.LocationAndAnchor.Anchor}_periodType");
                elementMoq.Setup(e => e.ElementId).Returns(loc.ElementId);
                string label = $"{prefix}_{i}";

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
    }
}
