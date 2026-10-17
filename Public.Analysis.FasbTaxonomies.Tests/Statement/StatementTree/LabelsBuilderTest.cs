using AwesomeAssertions;
using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
using Public.Analysis.FasbTaxonomies.Statement.StatementTree.Labels;
using Public.Analysis.FasbTaxonomies.XmlParsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Tests.Statement.StatementTree
{
    [TestClass]
    public class LabelsBuilderTest
    {
        private readonly LabelLink labelLink;
        private readonly Dictionary<ElementId, IReadOnlyDictionary<LabelRole,string>> expectedDict;

        public LabelsBuilderTest()
        {
            List<Loc> elementLocs = new List<Loc>();
            List<Label> labelLocs = new List<Label>();
            List<Arc> links = new List<Arc>();
            this.expectedDict = new Dictionary<ElementId, IReadOnlyDictionary<LabelRole,string>>();
            for (int i = 1; i <= 3; i++)
            {
                Loc elementLocator = new Loc();
                elementLocator.Href = $"location#element Id Expected In Arc Role {i}";
                elementLocator.XLinkLabel = $"loc_{i}";

                elementLocs.Add(elementLocator);

                var labelsXlinkLabel = $"label_{i}";
                var labels = LabelRole.LabelRoles
                    .Select((r, j) => new Label { Id = $"label_{r}_{i}_{j}", Role = $"http://www.xbrl.org/2003/role/{r}", Value = $"label text for {r} {i} {j}", XLinkLabel = labelsXlinkLabel });
                
                labelLocs.AddRange(labels);

                Arc linkLabelAndElement = new Arc();
                linkLabelAndElement.From = elementLocator.XLinkLabel;
                linkLabelAndElement.To = labelsXlinkLabel; //links the same element to multiple labels who share the same labelsXlinkLabel

                links.Add(linkLabelAndElement);

                expectedDict[elementLocator.ElementId] = labels.ToDictionary(l => new LabelRole( l.LabelRole), l => l.Value);
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
            var labelsByXLinkLabel = this.labelLink.Labels.GroupBy(l => l.XLinkLabel);
            var missing = labelsByXLinkLabel.ElementAt(1);
            this.labelLink.Labels = labelsByXLinkLabel.Take(1).SelectMany(g=>g).Concat(labelsByXLinkLabel.Skip(2).SelectMany(g=>g)).ToArray();

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
            actual.Message.Should().Be($"Label from {missing.Key} was not found in {nameof(LabelLink.Labels)}.");
        }
    }
}
