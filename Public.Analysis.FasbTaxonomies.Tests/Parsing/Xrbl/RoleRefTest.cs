using AwesomeAssertions;
using Public.Analysis.FasbTaxonomies.Parsing.Xml;
using Public.Analysis.FasbTaxonomies.Tests.Parsing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.Parsing.Xrbl
{
    [TestClass]
    public class RoleRefTest
    {
        [TestMethod]
        public void RoleUriTest()
        {
            var roleRefElement = new RoleRef(FasbXmlElements.RoleRefElement);
            var expectedRoleUri = new Uri("http://fasb.org/us-gaap/role/statement/StatementOfCashFlowsIndirectDepositBasedOperations");
            var actualRoleUri = roleRefElement.RoleUri();
            Assert.AreEqual(expectedRoleUri, actualRoleUri);
        }
        [TestMethod]
        public void RoleUriMissingTest()
        {
            var roleRefElement =  FasbXmlElements.RoleRefElement;
            roleRefElement.Attribute("roleURI")!.Remove();

            var roleRef = new RoleRef(roleRefElement);
            Assert.Throws<InvalidOperationException>(() => roleRef.RoleUri());
        }
        [TestMethod]
        public void HRefTest()
        {
            var roleRefElement = new RoleRef(FasbXmlElements.RoleRefElement);
            HRef hRef = roleRefElement.HRef(FasbXmlElements.NameSpacesByElementName);
            var expectedHRef = new HRef(FasbXmlElements.roleXLinkHRef);
            hRef.Should().BeEquivalentTo(expectedHRef);
        }
        [TestMethod]
        public void HRefMissingTest()
        {
            var roleRefElement = FasbXmlElements.RoleRefElement;
            roleRefElement.Attribute(XName.Get("href", "http://www.w3.org/1999/xlink"))!.Remove();
            var roleRef = new RoleRef(roleRefElement);
            Assert.Throws<InvalidOperationException>(() => roleRef.HRef(FasbXmlElements.NameSpacesByElementName));
        }
        [TestMethod]
        public void XLinkTypeTest()
        {
            var roleRefElement = new RoleRef(FasbXmlElements.RoleRefElement);
            var actualXLinkType = roleRefElement.XLinkType(FasbXmlElements.NameSpacesByElementName);
            Assert.AreEqual(FasbXmlElements.roleXLinkType, actualXLinkType);
        }
        [TestMethod]
        public void XLinkTypeMissingTest()
        {
            var roleRefElement = FasbXmlElements.RoleRefElement;
            roleRefElement.Attribute(XName.Get("type", "http://www.w3.org/1999/xlink"))!.Remove();
            var roleRef = new RoleRef(roleRefElement);
            Assert.Throws<InvalidOperationException>(() => roleRef.XLinkType(FasbXmlElements.NameSpacesByElementName));
        }
    }
}
