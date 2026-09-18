using AwesomeAssertions;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.Tests.XmlParsing.DeserializableElementsModels
{
    [TestClass]
    public class LabelsElementModelsTest
    {
        [TestMethod]
        public void ToDo()
        {
            Assert.Fail("Maybe you need to parse srt labels file as well? Maybe you need to parse other srt files?")
        }
        [Ignore]
        [TestMethod]
        public void LabelsXsdFileParses()
        {

            XmlSerializer serializer = new XmlSerializer(typeof(linkbase));
            string labelFilePath = @"path to us-gaap-lab-2026.xml";
            HashSet<string> hasPresentationLink = new HashSet<string>();
            using (StreamReader reader = new StreamReader(labelFilePath))
            {
                linkbase result = (linkbase)serializer.Deserialize(reader)!;

                if (result.labelLink is not null)
                {
                    var locs = result.labelLink.Items.Where(item => item is linkbaseLabelLinkLoc).ToList();
                    var labels = result.labelLink.Items.Where(item => item is linkbaseLabelLinkLabel).ToList();
                    var arcs = result.labelLink.Items.Where(item => item is linkbaseLabelLinkLabelArc).ToList();

                    locs.Should().NotBeEmpty();
                    labels.Should().NotBeEmpty();
                    arcs.Should().NotBeEmpty();
                }
            }

        }

    }
}

