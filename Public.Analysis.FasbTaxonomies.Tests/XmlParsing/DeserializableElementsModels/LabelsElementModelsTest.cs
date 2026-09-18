using AwesomeAssertions;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels.LabelsElementModels
{
    [TestClass]
    public class AllLabelsFilesParseTest
    {
        [TestMethod]
        public void ToDo()
        {
            Assert.Fail("Maybe you need to parse srt labels file as well? Maybe you need to parse other srt files?");
        }
        [DataRow(@"..\..\..\..\..\fasb_taxonomies\us-gaap-2026\elts", DisplayName = "elts")]
        [TestMethod]
        public void AllLabelsFilesParse(string labelsFilesDirectory)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(LabelLinkBase));
            HashSet<string> hasPresentationLink = new HashSet<string>();
            
            string labelsFileSearch = "*-lab-*";

            var labelsFiles = Directory.GetFiles(labelsFilesDirectory, labelsFileSearch).ToList();

            foreach (string labelsFile in labelsFiles)
            {
                using (StreamReader reader = new StreamReader(labelsFile))
                {
                    LabelLinkBase result = (LabelLinkBase)serializer.Deserialize(reader)!;
                    result.LabelLink.Should().NotBeNull();

                    var locs = result.LabelLink.Locs;
                    var labels = result.LabelLink.Labels;
                    var arcs = result.LabelLink.Arcs;

                    locs.Should().NotBeEmpty();
                    labels.Should().NotBeEmpty();
                    arcs.Should().NotBeEmpty();
                }
            }
        }
    }
}

