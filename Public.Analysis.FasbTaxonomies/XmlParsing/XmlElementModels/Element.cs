using Public.Analysis.FasbTaxonomies.Parsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.Xml
{
    public class Element
    {
        private readonly XElement xElement;
        private readonly Dictionary<string, XNamespace> namespaces;
        private readonly IXElementParsingUtility parser;

        public Element(XElement xElement, Dictionary<string, XNamespace> namespaces, IXElementParsingUtility parser)
        {
            this.xElement = xElement;
            this.namespaces = namespaces;
            this.parser = parser;
        }
        public bool Abstract => this.parser.GetAttributeValue<bool>(this.xElement, this.namespaces, LocalNamesAndPrefixes.AbstractAttribute, LocalNamesAndPrefixes.XsPrefix);
        public string Id => this.parser.GetAttributeValue<string>(this.xElement, this.namespaces, LocalNamesAndPrefixes.IdAttribute, LocalNamesAndPrefixes.XsPrefix);
        public string Name => this.parser.GetAttributeValue<string>(this.xElement, this.namespaces, LocalNamesAndPrefixes.IdAttribute, LocalNamesAndPrefixes.XsPrefix);
        public bool Nillable => this.parser.GetAttributeValue<bool>(this.xElement, this.namespaces, LocalNamesAndPrefixes.Nillable, LocalNamesAndPrefixes.XsPrefix);
    }
}
