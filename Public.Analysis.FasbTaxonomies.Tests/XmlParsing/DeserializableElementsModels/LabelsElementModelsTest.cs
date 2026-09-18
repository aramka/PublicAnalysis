using AwesomeAssertions;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels.LabelsElementModels
{
    [TestClass]
    public class LabelsElementModelsTest
    {
        [TestMethod]
        public void ToDo()
        {
            Assert.Fail("Maybe you need to parse srt labels file as well? Maybe you need to parse other srt files?");
        }
        [DataRow(@"..\..\..\..\..\fasb_taxonomies\us-gaap-2026\elts", DisplayName = "elts")]
        [TestMethod]
        public void LabelsXsdFileParses(string labelsFilesDirectory)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(linkbase));
            HashSet<string> hasPresentationLink = new HashSet<string>();
            
            string labelsFileSearch = "*-lab-*";

            var labelsFiles = Directory.GetFiles(labelsFilesDirectory, labelsFileSearch).ToList();

            foreach (string labelsFile in labelsFiles)
            {
                using (StreamReader reader = new StreamReader(labelsFile))
                {
                    linkbase result = (linkbase)serializer.Deserialize(reader)!;
                    result.labelLink.Should().NotBeNull();

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

