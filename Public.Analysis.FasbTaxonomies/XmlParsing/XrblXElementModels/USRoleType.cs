using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels
{
    public class USRoleType : IUSRoleType
    {
        private XElement xElement;
        private IReadOnlyDictionary<string, XNamespace> namespacesByPrefix;
        private IXElementParsingUtility xElementParsingUtility;

        public USRoleType(XElement xElement, IReadOnlyDictionary<string, XNamespace> namespacesByPrefix, IXElementParsingUtility xElementParsingUtility)
        {
            this.xElement = xElement;
            this.namespacesByPrefix = namespacesByPrefix;
            this.xElementParsingUtility = xElementParsingUtility;
        }

        public string LinkRoleTypeId => this.xElementParsingUtility.GetAttributeValue(this.xElement, LocalNamesAndPrefixes.IdAttribute);

        public string LinkRoleTypeLinkDefinition => this.xElementParsingUtility.GetDescendants(this.xElement, LocalNamesAndPrefixes.DefinitionElement, LocalNamesAndPrefixes.LinkPrefix, this.namespacesByPrefix).Single().Value;
    }
}