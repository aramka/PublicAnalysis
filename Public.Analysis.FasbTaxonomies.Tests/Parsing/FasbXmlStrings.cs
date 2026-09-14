using Microsoft.Identity.Client;
using Public.Analysis.FasbTaxonomies.Parsing.Xrbl;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace Public.Analysis.FasbTaxonomies.Tests.Parsing
{
    public static class FasbXmlElements
    {
        public static string roleRefRoleUri = @"http://fasb.org/us-gaap/role/statement/StatementOfCashFlowsIndirectDepositBasedOperations";
        public static readonly string roleXLinkHRef = "../elts/us-roles-2026.xsd#scf-dbo";

        public static readonly string roleXLinkType = "simple";

        public static readonly string presentationLinkRoleUri = "http://fasb.org/us-gaap/role/statement/CommonDomainMembers";
        public static readonly string presentationLinkXLinkType = "extended";
        public static readonly string statementDocXmlString = $@"
            <link:linkbase xmlns:link='http://www.xbrl.org/2003/linkbase' xmlns:xlink='http://www.w3.org/1999/xlink' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:schemaLocation='http://www.xbrl.org/2003/linkbase http://www.xbrl.org/2003/xbrl-linkbase-2003-12-31.xsd'>
                <link:roleRef roleURI='{roleRefRoleUri}' xlink:href='{roleXLinkHRef}' xlink:type='{roleXLinkType}'/>
                <link:presentationLink xlink:role='{presentationLinkRoleUri}' xlink:type='{presentationLinkXLinkType}'></link:presentationLink>
            </link:linkbase>
        ";

        private static readonly MemoryStream statementDocMemStream = new MemoryStream(Encoding.UTF8.GetBytes(statementDocXmlString));

        private static readonly XDocument statementDoc = XDocument.Load(statementDocMemStream);
        public static XDocument StatementDoc => new XDocument(statementDoc);

        public static Dictionary<string,XNamespace> NameSpacesByPrefix => statementDoc.Root!.Attributes().Where(a => a.IsNamespaceDeclaration).ToDictionary(a => a.Name.LocalName, a => (XNamespace)a.Value);


        public static XElement RoleRefElement => new XElement(statementDoc.Descendants(NameSpacesByPrefix["link"] + "roleRef").Single());

        public static XElement PresentationLinkElement => new XElement(statementDoc.Descendants(NameSpacesByPrefix["link"] + "presentationLink").Single());

    }
}
