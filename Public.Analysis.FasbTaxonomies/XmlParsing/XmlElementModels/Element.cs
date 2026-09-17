using Public.Analysis.FasbTaxonomies.Parsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.Xml
{
    public class Element
    {
        private readonly XElement xElement;
        private readonly IReadOnlyDictionary<string, XNamespace> namespaces;
        private readonly IXElementParsingUtility parser;

        public Element(XElement xElement, IReadOnlyDictionary<string, XNamespace> namespaces, IXElementParsingUtility parser)
        {
            this.xElement = xElement;
            this.namespaces = namespaces;
            this.parser = parser;
        }
        public bool Abstract => this.parser.TryGetAttributeValue<bool>(this.xElement, LocalNamesAndPrefixes.AbstractAttribute, out bool value) ? value : false;
        public string Id => this.parser.TryGetAttributeValue(this.xElement, LocalNamesAndPrefixes.IdAttribute, out string? id) ? id! : string.Empty;
        public string Name => this.parser.TryGetAttributeValue(this.xElement, LocalNamesAndPrefixes.IdAttribute, out string? name ) ? name! : string.Empty;
        public bool Nillable => this.parser.TryGetAttributeValue<bool>(this.xElement, LocalNamesAndPrefixes.Nillable, out bool value) ? value : false;
    }
}
