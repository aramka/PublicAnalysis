using AwesomeAssertions;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels.PresentationElementModels
{
    [TestClass]
    public class PresentationElementModelsTest
    {
        [DataRow(@"..\..\..\..\..\fasb_taxonomies\us-gaap-2026\stm", DisplayName = "Statements")]
        [DataRow(@"..\..\..\..\..\fasb_taxonomies\us-gaap-2026\elts", DisplayName = "elts")]
        [TestMethod]
        public void AllPresentationFilesParse(string presentationFilesDirectory)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(StatementLinkBase));
            
            
            string presentationXmlFileSearch = "*-pre-*";

            var presentationFiles = Directory.GetFiles(presentationFilesDirectory, presentationXmlFileSearch).ToList();
            int i = 0;
            HashSet<string> hasPresentationLink = new HashSet<string>();
            foreach (string filePath in presentationFiles)
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    StatementLinkBase result = (StatementLinkBase)serializer.Deserialize(reader)!;

                    result.presentationLink.Should().NotBeNull();

                    var hRefs = result.presentationLink.Locs.Where(l => string.IsNullOrWhiteSpace(l.LocationAndAnchor.Location) || string.IsNullOrWhiteSpace(l.LocationAndAnchor.Anchor));

                    hRefs.Should().BeEmpty();

                    hasPresentationLink.Add(filePath);
                }
                ++i;
            }

            var noPresentationLink = presentationFiles.Where(f => !hasPresentationLink.Contains(f)).ToList();

            noPresentationLink.Should().BeEmpty();

        }
    }
}
