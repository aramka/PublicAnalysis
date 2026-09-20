using AwesomeAssertions;
using Public.Analysis.FasbTaxonomies.StatementTree;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Tests.StatementNodesBuilder
{
    [TestClass]
    public class LabelsBuilderTest
    {
        private readonly LabelLink labelLink;
        private readonly Dictionary<ElementIdRecord, string> expectedDict;

        public LabelsBuilderTest()
        {
            List<Loc> elementLocs = new List<Loc>();
            List<Label> labelLocs = new List<Label>();
            List<Arc> links = new List<Arc>();
            this.expectedDict = new Dictionary<ElementIdRecord, string>();
            for (int i = 1; i <= 3; i++)
            {
                Loc elementLocator = new Loc();
                elementLocator.Href = $"location#element Id Expected In Arc Role {i}";
                elementLocator.XLinkLabel = $"loc_{i}";

                elementLocs.Add(elementLocator);

                Label label = new Label();
                label.XLinkLabel = $"label id expected in arc role {i}";
                label.Value = $"the actual label text {i}";
                labelLocs.Add(label);

                Arc linkLabelAndElement = new Arc();
                linkLabelAndElement.From = elementLocator.XLinkLabel;
                linkLabelAndElement.To = label.XLinkLabel;

                links.Add(linkLabelAndElement);

                expectedDict[elementLocator.ElementId] = label.Value;
            }

            this.labelLink = new LabelLink
            {
                Arcs = links.ToArray(),
                Labels = labelLocs.ToArray(),
                Locs = elementLocs.ToArray()
            };
        }
        [TestMethod]
        public void BuildLabels()
        {
            LabelsBuilder labelsBuilder = new LabelsBuilder();
            var actualLabelsDict =  labelsBuilder.BuildLabels(labelLink);

            actualLabelsDict.Should().BeEquivalentTo(expectedDict);
        }

        [TestMethod]
        public void ElementNotFound()
        {

        }
    }
}
