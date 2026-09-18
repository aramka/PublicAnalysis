using AwesomeAssertions;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels.CnReferenceElementModels;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.Tests.XmlParsing.DeserializableElementsModels
{
    [TestClass]
    public class CnReferenceElementsModelsTest
    {
        [DataRow(@"..\..\..\..\..\fasb_taxonomies\us-gaap-2026\elts", DisplayName = "elts")]
        [TestMethod]
        public void AllCnRefFilesParse(string labelsFilesDirectory)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(ReferenceLinkBase));
            HashSet<string> hasPresentationLink = new HashSet<string>();

            string cnRefFilesSearch = "*-cn-ref-*";

            var labelsFiles = Directory.GetFiles(labelsFilesDirectory, cnRefFilesSearch).ToList();

            foreach (string labelsFile in labelsFiles)
            {
                using (StreamReader reader = new StreamReader(labelsFile))
                {
                    ReferenceLinkBase result = (ReferenceLinkBase)serializer.Deserialize(reader)!;
                    result.referenceLink.Should().NotBeNull();

                    var locs = result.referenceLink.Locs;
                    var refs = result.referenceLink.References;
                    var arcs = result.referenceLink.ReferenceArcs;

                    locs.Should().NotBeEmpty();
                    refs.Should().NotBeEmpty();
                    arcs.Should().NotBeEmpty();

                    var refCns = refs.SelectMany(r => r.CnParts).ToList();
                    refCns.Should().NotBeEmpty();
                }
            }
        }
    }
}
