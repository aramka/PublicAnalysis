using Public.Analysis.FasbTaxonomies.XmlParsing;
using Public.Analysis.FasbTaxonomies.Tests.Parsing;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq
{
    [TestClass]
    public class XElementParsingHelperTest
    {
        [TestMethod]
        public void AttributeMissingTest()
        {

            var locElement = FasbXmlElements.LocElement1;
            locElement.Attribute(FasbXmlElements.NameSpacesByPrefix[LocalNamesAndPrefixes.XLinkPrefix] + LocalNamesAndPrefixes.LabelAttribute)!.Remove();

            XElementParsingUtility helper = new XElementParsingUtility();

            Assert.Throws<InvalidOperationException>(() => helper.GetAttributeValue<string>(locElement, LocalNamesAndPrefixes.LabelAttribute,  LocalNamesAndPrefixes.XLinkPrefix, FasbXmlElements.NameSpacesByPrefix));
        }

    }
}
