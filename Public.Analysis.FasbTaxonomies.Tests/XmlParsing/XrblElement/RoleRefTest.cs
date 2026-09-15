using AwesomeAssertions;
using Public.Analysis.FasbTaxonomies.XmlParsing.Xml;
using Public.Analysis.FasbTaxonomies.Tests.Parsing;
using System.Xml.Linq;
using Public.Analysis.FasbTaxonomies.XmlParsing.Xrbl;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XrblElement
{
    [TestClass]
    public class RoleRefTest
    {
        [TestMethod]
        public void RoleUriTest()
        {
            var roleRefElement = new RoleRef(FasbXmlElements.RoleRefElement, FasbXmlElements.NameSpacesByPrefix);
            var expectedRoleUri = new Uri("http://fasb.org/us-gaap/role/statement/StatementOfCashFlowsIndirectDepositBasedOperations");
            var actualRoleUri = roleRefElement.RoleUri;
            Assert.AreEqual(expectedRoleUri, actualRoleUri);
        }
        [TestMethod]
        public void RoleUriMissingTest()
        {
            var roleRefElement =  FasbXmlElements.RoleRefElement;
            roleRefElement.Attribute("roleURI")!.Remove();

            var roleRef = new RoleRef(roleRefElement, FasbXmlElements.NameSpacesByPrefix);
            Assert.Throws<InvalidOperationException>(() => roleRef.RoleUri);
        }
        [TestMethod]
        public void HRefTest()
        {
            var roleRefElement = new RoleRef(FasbXmlElements.RoleRefElement, FasbXmlElements.NameSpacesByPrefix);
            HRef hRef = roleRefElement.HRef;
            var expectedHRef = new HRef(FasbXmlElements.roleXLinkHRef);
            hRef.Should().BeEquivalentTo(expectedHRef);
        }
        [TestMethod]
        public void HRefMissingTest()
        {
            var roleRefElement = FasbXmlElements.RoleRefElement;
            roleRefElement.Attribute(XName.Get("href", "http://www.w3.org/1999/xlink"))!.Remove();
            var roleRef = new RoleRef(roleRefElement, FasbXmlElements.NameSpacesByPrefix);
            Assert.Throws<InvalidOperationException>(() => roleRef.HRef);
        }
        [TestMethod]
        public void XLinkTypeTest()
        {
            var roleRefElement = new RoleRef(FasbXmlElements.RoleRefElement, FasbXmlElements.NameSpacesByPrefix);
            var actualXLinkType = roleRefElement.XLinkType;
            Assert.AreEqual(FasbXmlElements.roleXLinkType, actualXLinkType);
        }
        [TestMethod]
        public void XLinkTypeMissingTest()
        {
            var roleRefElement = FasbXmlElements.RoleRefElement;
            roleRefElement.Attribute(XName.Get("type", "http://www.w3.org/1999/xlink"))!.Remove();
            var roleRef = new RoleRef(roleRefElement, FasbXmlElements.NameSpacesByPrefix);
            Assert.Throws<InvalidOperationException>(() => roleRef.XLinkType);
        }
    }
}
