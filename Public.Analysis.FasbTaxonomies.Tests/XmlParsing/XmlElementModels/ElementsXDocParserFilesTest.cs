using AwesomeAssertions;
using Public.Analysis.FasbTaxonomies.Tests.Parsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.Tests.XmlParsing.XmlElementModels
{
    [TestClass]
    public class ElementsXDocParserFilesTest
    {
        [TestMethod]
        [DataRow(data: @"..\..\..\..\..\fasb_taxonomies\us-gaap-2026\elts\us-gaap-2026.xsd")]
        [DataRow(data: @"..\..\..\..\..\fasb_taxonomies\us-gaap-2026\elts\srt-2026.xsd")]
        public void ReadAllElementsFromFile(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                XDocument xDoc = XDocument.Load(reader);

                var elts = new ElementsXDocParser(xDoc, new XElementParsingUtility());

                var allElements = elts.GetElements().ToList();


                Assert.IsTrue(allElements.Any());

                foreach (XsElement e in allElements)
                {
                    bool a = e.Abstract;
                    string balance = e.Balance;
                    e.Id.Should().NotBeNullOrWhiteSpace();
                    e.ElementId.Should().NotBeNull();
                    e.Name.Should().NotBeNullOrWhiteSpace();
                    bool nillable = e.Nillable;
                    string pType = e.PeriodType;
                    e.Type.Should().NotBeNull();
                }
            }
        }
    }
}
