using AwesomeAssertions;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels
{
    [TestClass]
    public class StatementPresentationElementModelsTest
    {
        [Ignore]
        [TestMethod]
        public void AllFasbStatementsParse()
        {

            XmlSerializer serializer = new XmlSerializer(typeof(linkbase));
            string usGaap2026StmDirectory = @"fasb us-gaap-2026\stm directory";
            var statementFiles = Directory.GetFiles(usGaap2026StmDirectory, "*pre-2026.xml").ToList();
            int i = 0;
            HashSet<string> hasPresentationLink = new HashSet<string>();
            foreach (string filePath in statementFiles)
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    linkbase result = (linkbase)serializer.Deserialize(reader)!;
                    
                    if(result.presentationLink is not null)
                    {
                        var hRefs = result.presentationLink.loc.ToHRef().Where(l => string.IsNullOrWhiteSpace(l.Location)  || string.IsNullOrWhiteSpace(l.Anchor));

                        hRefs.Should().BeEmpty();

                        hasPresentationLink.Add(filePath);
                    }
                }
                ++i;
            }

            var noPresentationLink = statementFiles.Where(f => !hasPresentationLink.Contains(f)).ToList();

            noPresentationLink.Should().BeEmpty();

        }
    }
}
