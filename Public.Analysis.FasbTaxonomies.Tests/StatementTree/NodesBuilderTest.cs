using Moq;
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
         *  
         * LabelsBuilder:ILabelsBuilder
         *  IReadOnlyDictionary<ElementId,string> BuildLabels(labels)
         * NodeBuilder:INodeBuilder
         *  NodeBuilder(ILabelsBuilder)
         *  IEnumerable<IStatementNode> BuildNodes(IEnumerable<IUSGaapElements> elements, IEnumerable<Loc> locators, LabelLinkBase labels);
         * DeprecateConcepts:IDeprecateConcepts
         *  DeprecateConcepts(IBuildDeprecatedConcepts deprecatedConceptsBuilder)
         *  IEnumerable<IDeprecatedConcept> DeprecateConcepts(List<IStatementNode> nodes);
         * StatementTreeBuilder:IStatementTreeBuilder
         *  IReadOnlyDictionary<ElementId,IStatementNode> BuildStatementTree(IEnumerable<IStatementNode> nodes);
         */

        [TestMethod]
        public void Build()
        {
            Loc parentLoc = new Loc
            {
                Href = "elements#parentId",
                XLinkLabel = "loc_parentId"
            };
            Loc childLoc = new Loc
            {
                Href = "elements#childId",
                XLinkLabel = "lock_childId"
            };
            var locators = new Loc[] { parentLoc, childLoc };

            Arc fromTo = new Arc();
            fromTo.From = parentLoc.XLinkLabel;
            fromTo.To = childLoc.XLinkLabel;
            var parentChildArcs = new Arc[] { fromTo };

            var elementsMoqs = locators
                .Select(l => {
                    var eMoq = new Mock<IXsElement>();
                    eMoq.Setup(e => e.Id).Returns(l.LocationAndAnchor.Anchor);
                    eMoq.Setup(e => e.Name).Returns(l.LocationAndAnchor.Anchor);
                    return eMoq;
                }).ToList();

            // var labels = locators.Select
        }
    }
}
