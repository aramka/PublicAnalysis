using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing
{
    public class RoleTypeXElementParser : IRoleTypeParser
    {
        private XElement xElement;
        private IReadOnlyDictionary<string, XNamespace> namespacesByPrefix;
        private IXElementParsingUtility xElementParsingUtility;

        public RoleTypeXElementParser(XElement xElement, IReadOnlyDictionary<string, XNamespace> namespacesByPrefix, IXElementParsingUtility xElementParsingUtility)
        {
            this.xElement = xElement;
            this.namespacesByPrefix = namespacesByPrefix;
            this.xElementParsingUtility = xElementParsingUtility;
        }

        public string LinkRoleTypeId => this.xElementParsingUtility.GetAttributeValue(this.xElement, LocalNamesAndPrefixes.IdAttribute);

        public string LinkRoleTypeLinkDefinitionValue => this.xElementParsingUtility.GetDescendants(this.xElement, LocalNamesAndPrefixes.DefinitionElement, LocalNamesAndPrefixes.LinkPrefix, this.namespacesByPrefix).Single().Value;
    }
}