using Public.Analysis.FasbTaxonomies.Tests.XmlParsing;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.Tests.Parsing
{
    public static class FasbXmlElements
    {
        public static string roleRefRoleUri = @"http://fasb.org/us-gaap/role/statement/StatementOfCashFlowsIndirectDepositBasedOperations";
        public static readonly string roleXLinkHRef = "../elts/us-roles-2026.xsd#scf-dbo";

        public static readonly string roleXLinkType = "simple";

        public static readonly string presentationLinkRoleUri = "http://fasb.org/us-gaap/role/statement/CommonDomainMembers";
        public static readonly string presentationLinkXLinkType = "extended";
        public static readonly string locXLinkHRef1 = "https://xbrl.fasb.org/srt/2026/elts/srt-2026.xsd#srt_RestatementAxis";
        public static readonly string locXLinkHRef2 = "../elts/us-gaap-2026.xsd#us-gaap_StatementTable";
        public static readonly string locLabel1 = "loc_RestatementAxis";
        public static readonly string locLabel2 = "loc_StatementTable";
        public static readonly string locXLinkType = "locator";
        public static readonly string statementDocXmlString = $@"
            <link:linkbase xmlns:link='http://www.xbrl.org/2003/linkbase' xmlns:xlink='http://www.w3.org/1999/xlink' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:schemaLocation='http://www.xbrl.org/2003/linkbase http://www.xbrl.org/2003/xbrl-linkbase-2003-12-31.xsd'>
                <link:roleRef roleURI='{roleRefRoleUri}' xlink:href='{roleXLinkHRef}' xlink:type='{roleXLinkType}'/>
                <link:presentationLink xlink:role='{presentationLinkRoleUri}' xlink:type='{presentationLinkXLinkType}'>
                    <link:loc xlink:href='{locXLinkHRef1}' xlink:label='{locLabel1}' xlink:type='{locXLinkType}' />
                    <link:loc xlink:href='{locXLinkHRef2}' xlink:label='{locLabel2}' xlink:type='{locXLinkType}' />
                    <link:presentationArc order='10' xlink:arcrole='http://www.xbrl.org/2003/arcrole/parent-child' xlink:from='loc_StatementOfCashFlowsAbstract' xlink:to='loc_StatementTable' xlink:type='arc' />
                </link:presentationLink>
            </link:linkbase>
        ";
        public static XmlStringsHelper helper = new XmlStringsHelper(statementDocXmlString);

        public static IReadOnlyDictionary<string, XNamespace> NameSpacesByPrefix => helper.RootNameSpacesByPrefix;

        public static XElement RoleRefElement => new XElement(helper.XDoc.Descendants(helper.RootNameSpacesByPrefix["link"] + "roleRef").Single());

        public static XElement PresentationLinkElement => new XElement(helper.XDoc.Descendants(helper.RootNameSpacesByPrefix["link"] + "presentationLink").Single());

        public static XElement LocElement(string label) => new XElement(helper.XDoc.Descendants(helper.RootNameSpacesByPrefix["link"] + "loc").Single(xe => { var attr = xe.Attribute(helper.RootNameSpacesByPrefix["xlink"] + "label"); return attr is not null && attr.Value == label; }));

        public static XElement LocElement1 => LocElement(locLabel1);
        public static XElement LocElement2 => LocElement(locLabel2);


    }
}
