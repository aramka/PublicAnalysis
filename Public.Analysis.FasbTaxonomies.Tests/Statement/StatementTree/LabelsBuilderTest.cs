using AwesomeAssertions;
using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
using Public.Analysis.FasbTaxonomies.XmlParsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Tests.Statement.StatementTree
{
    [TestClass]
    public class LabelsBuilderTest
    {
        private readonly LabelLink labelLink;
        private readonly Dictionary<ElementId, string> expectedDict;

        public LabelsBuilderTest()
        {
            List<Loc> elementLocs = new List<Loc>();
            List<Label> labelLocs = new List<Label>();
            List<Arc> links = new List<Arc>();
            this.expectedDict = new Dictionary<ElementId, string>();
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
            var missing = this.labelLink.Locs[1];
            this.labelLink.Locs = this.labelLink.Locs.Take(1).Concat(this.labelLink.Locs.Skip(2)).ToArray();

            LabelsBuilder builder = new LabelsBuilder();
            InvalidOperationException? actual = null;
            try
            {
                builder.BuildLabels(this.labelLink);

            }catch(InvalidOperationException e)
            {
                actual = e;
            }

            actual.Should().NotBeNull();
            actual.Message.Should().Be($"Arc from {missing.XLinkLabel} was not found in {nameof(LabelLink.Locs)}.");
        }
        [TestMethod]
        public void LabelNotFound()
        {
            var missing = this.labelLink.Labels[1];
            this.labelLink.Labels = this.labelLink.Labels.Take(1).Concat(this.labelLink.Labels.Skip(2)).ToArray();

            LabelsBuilder builder = new LabelsBuilder();
            InvalidOperationException? actual = null;
            try
            {
                builder.BuildLabels(this.labelLink);
            }
            catch (InvalidOperationException e)
            {
                actual = e;
            }

            actual.Should().NotBeNull();
            actual.Message.Should().Be($"Label from {missing.XLinkLabel} was not found in {nameof(LabelLink.Labels)}.");
        }
        [TestMethod]
        public void StandardAndTotalLabelUseTotalLabel()
        {
            var standardLabel= this.labelLink.Labels[0];
            standardLabel.Role = @"http://www.xbrl.org/2003/role/label";
            
            Label totalLabel = new Label();
            totalLabel.XLinkLabel = standardLabel.XLinkLabel;
            totalLabel.Role = @"http://www.xbrl.org/2003/role/totalLabel";
            totalLabel.Value = $"{standardLabel.Value} total";

            labelLink.Labels = [totalLabel, .. labelLink.Labels];

            LabelsBuilder labelsBuilder = new LabelsBuilder();
            var actualLabelsDict = labelsBuilder.BuildLabels(labelLink);

            var elementXLinkLabel = this.labelLink.Arcs.Single(a => a.To == standardLabel.XLinkLabel).From;
            var elementLoc = this.labelLink.Locs.Single(a => a.XLinkLabel == elementXLinkLabel);
            var elementId = elementLoc.ElementId;

            actualLabelsDict[elementId].Should().Be(totalLabel.Value);
        }
        [TestMethod]
        [DataRow(data: LocalNamesAndPrefixes.XLinkRoleStandardLabelSuffix)]
        [DataRow(data: LocalNamesAndPrefixes.XLinkRoleTotalLabelSuffix)]
        public void MoreThanOneStandardOrTotalLabel(string suffix)
        {
            var standardLabel = this.labelLink.Labels[0];
            standardLabel.Role = $@"http://www.xbrl.org/2003/role/{suffix}";

            Label dupe = new Label();
            dupe.XLinkLabel = standardLabel.XLinkLabel;
            dupe.Role = standardLabel.Role;
            dupe.Value = standardLabel.Value;

            labelLink.Labels = [dupe, .. labelLink.Labels];

            LabelsBuilder labelsBuilder = new LabelsBuilder();
            Assert.Throws<InvalidOperationException>(() => labelsBuilder.BuildLabels(labelLink));


        }

    }
}
