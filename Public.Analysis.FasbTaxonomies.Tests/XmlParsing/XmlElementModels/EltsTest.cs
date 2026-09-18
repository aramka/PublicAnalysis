using AwesomeAssertions;
using Public.Analysis.FasbTaxonomies.Tests.Parsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlElementModels;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.Tests.XmlParsing.XmlElementModels
{
    [TestClass]
    public class EltsTest
    {
        [TestMethod]
        public void ReadAllElementsFromFile()
        {
            var eltsFiles = new string[]
            {
                @"..\..\..\..\..\fasb_taxonomies\us-gaap-2026\elts\us-gaap-2026.xsd",
                @"..\..\..\..\..\fasb_taxonomies\us-gaap-2026\elts\srt-2026.xsd"
            };
            int i = 0;
            foreach (string filePath in eltsFiles)
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    XDocument xDoc = XDocument.Load(reader);

                    var elts = new Elts(xDoc, new XElementParsingUtility());

                    var allElements = elts.GetElements().ToList();

                    Assert.IsTrue(allElements.Any());
                }
                ++i;
            }
        }
    }
}
