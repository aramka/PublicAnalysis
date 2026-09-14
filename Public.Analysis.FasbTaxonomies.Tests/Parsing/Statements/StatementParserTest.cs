using Public.Analysis.FasbTaxonomies.Parsing.XrblElements;
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
        //private readonly XDocument statementDoc;

        //public StatementParserTest(XDocument statementDoc)
        //{
        //    this.statementDoc = statementDoc;
        //}

        //private RoleRefTest roleRef = null;
        //public RoleRefTest RoleRef => roleRef switch { null => new RoleRefTest(statementDoc.Descendants(XName.Get("roleRef", "link")).Single()), _ => this.roleRef };

        //private static readonly string statementDocXmlString = @"
        //    <link:linkbase xmlns:link='http://www.xbrl.org/2003/linkbase' xmlns:xlink='http://www.w3.org/1999/xlink' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:schemaLocation='http://www.xbrl.org/2003/linkbase http://www.xbrl.org/2003/xbrl-linkbase-2003-12-31.xsd'>
        //        <link:roleRef roleURI='http://fasb.org/us-gaap/role/statement/StatementOfCashFlowsIndirectDepositBasedOperations' xlink:href='../elts/us-roles-2026.xsd#scf-dbo' xlink:type='simple' />
        //    </link:linkbase>
        //";

        [TestMethod]
        public void RoleRefTest()
        {

            var statementParser = new StatementParser(FasbXmlElements.StatementDoc);

            var roleRef = statementParser.RoleRef();

            Assert.IsNotNull(roleRef);
        }
    }
}
