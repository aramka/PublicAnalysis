using AwesomeAssertions;
using Public.Analysis.FasbTaxonomies.Parsing.Xml;
using Public.Analysis.FasbTaxonomies.Tests.Parsing;
using System;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.Parsing.Xrbl
{
    [TestClass]
    public class LocTest
    {
        [TestMethod]
        public void HRefTest()
        {
            var locElement = new Loc(FasbXmlElements.LocElement1, FasbXmlElements.NameSpacesByPrefix);
            HRef hRef = locElement.HRef;
            var expectedHRef = new HRef(FasbXmlElements.locXLinkHRef1);
            hRef.Should().BeEquivalentTo(expectedHRef);
        }

        [TestMethod]
        public void HRefMissingTest()
        {
            var locElement = FasbXmlElements.LocElement1;
            locElement.Attribute(FasbXmlElements.NameSpacesByPrefix[LocalNamesAndPrefixes.XLinkPrefix] + LocalNamesAndPrefixes.HRefAttribute)!.Remove();
            var loc = new Loc(locElement, FasbXmlElements.NameSpacesByPrefix);
            Assert.Throws<InvalidOperationException>(() => loc.HRef);
        }

        [TestMethod]
        public void LabelTest()
        {
            var locElement = new Loc(FasbXmlElements.LocElement1, FasbXmlElements.NameSpacesByPrefix);
            var actualLabel = locElement.Label;
            Assert.AreEqual(FasbXmlElements.locLabel1, actualLabel);
        }

        [TestMethod]
        public void LabelMissingTest()
        {
            var locElement = FasbXmlElements.LocElement1;
            locElement.Attribute(FasbXmlElements.NameSpacesByPrefix[LocalNamesAndPrefixes.XLinkPrefix] + LocalNamesAndPrefixes.LabelAttribute)!.Remove();
            var loc = new Loc(locElement, FasbXmlElements.NameSpacesByPrefix);
            Assert.Throws<InvalidOperationException>(() => loc.Label);
        }

        [TestMethod]
        public void XLinkTypeTest()
        {
            var locElement = new Loc(FasbXmlElements.LocElement1, FasbXmlElements.NameSpacesByPrefix);
            var actualXLinkType = locElement.XLinkType;
            Assert.AreEqual(FasbXmlElements.locXLinkType, actualXLinkType);
        }

        [TestMethod]
        public void XLinkTypeMissingTest()
        {
            //var locElement = FasbXmlElements.LocElement;
            //locElement.Attribute(XName.Get("type", "http://www.w3.org/1999/xlink"))!.Remove();
            //var loc = new Loc(locElement, FasbXmlElements.NameSpacesByPrefix);
            //Assert.Throws<InvalidOperationException>(() => loc.XLinkType);
        }
    }
}
