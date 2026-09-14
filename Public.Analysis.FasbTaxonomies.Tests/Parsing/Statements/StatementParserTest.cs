using AwesomeAssertions;
using Public.Analysis.FasbTaxonomies.Parsing.Xrbl;
using Public.Analysis.FasbTaxonomies.Tests.Parsing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.Parsing.Statements
{
    [TestClass]
    public class StatementParserTest
    {

        [TestMethod]
        public void RoleRefTest()
        {

            var statementParser = new Statement(FasbXmlElements.StatementDoc, FasbXmlElements.NameSpacesByPrefix);

            var roleRef = statementParser.RoleRef;

            var expectedRoleRef = new RoleRef(FasbXmlElements.RoleRefElement, FasbXmlElements.NameSpacesByPrefix);

            roleRef.Should().BeEquivalentTo(expectedRoleRef);

        }
        [TestMethod]
        public void RoleRefMissingTest()
        {
            var statement = FasbXmlElements.StatementDoc;
            statement.Descendants(FasbXmlElements.NameSpacesByPrefix["link"] + "roleRef").Remove();
            var statementParser = new Statement(statement, FasbXmlElements.NameSpacesByPrefix);

            Assert.ThrowsExactly<InvalidOperationException>(()=>statementParser.RoleRef);
        }
        [TestMethod]
        public void PresentationLinkTest()
        {
            var statement = new Statement(FasbXmlElements.StatementDoc, FasbXmlElements.NameSpacesByPrefix);
            PresentationLink presentationLink = statement.PresentationLink;
            var expectedPresentationLink = new PresentationLink(FasbXmlElements.PresentationLinkElement, FasbXmlElements.NameSpacesByPrefix);

            presentationLink.Should().BeEquivalentTo(expectedPresentationLink);
        }
    }
}
