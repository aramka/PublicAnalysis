using Public.Analysis.FasbTaxonomies.XmlParsing.Xrbl;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Tests.Parsing.XrblElement
{
    [TestClass]
    public class PresentationLinkTest
    {
        [TestMethod]
        public void XLinkRole()
        {
            var presentationLink = new PresentationLink(FasbXmlElements.PresentationLinkElement, FasbXmlElements.NameSpacesByPrefix);
            string role = presentationLink.RoleAttributeValue;
            Assert.AreEqual(FasbXmlElements.presentationLinkRoleUri, role);

        }
        [TestMethod]
        public void XLinkRoleMissing()
        {
            var element = FasbXmlElements.PresentationLinkElement;
            element.Attribute(FasbXmlElements.NameSpacesByPrefix["xlink"] + "role")!.Remove();

            var presentationLink = new PresentationLink(element, FasbXmlElements.NameSpacesByPrefix);
            Assert.ThrowsExactly<InvalidOperationException>(() => presentationLink.RoleAttributeValue);
        }
        [TestMethod]
        public void XLinkType()
        {
            var presentationLink = new PresentationLink(FasbXmlElements.PresentationLinkElement, FasbXmlElements.NameSpacesByPrefix);
            string xLinkType = presentationLink.XLinkTypeAttributeValue;
            Assert.AreEqual(FasbXmlElements.presentationLinkXLinkType, xLinkType);
        }
        [TestMethod]
        public void XLinkTypeMissing() {
            var element = FasbXmlElements.PresentationLinkElement;
            element.Attribute(FasbXmlElements.NameSpacesByPrefix["xlink"] + "type")!.Remove();

            var presentationLink = new PresentationLink(element, FasbXmlElements.NameSpacesByPrefix);
            Assert.ThrowsExactly<InvalidOperationException>(() => presentationLink.XLinkTypeAttributeValue);
        }
    }
}
