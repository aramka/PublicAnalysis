using Public.Analysis.FasbTaxonomies.Parsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.Xml
{
    public class USGaapElement
    {
        private readonly XElement xElement;
        private readonly IReadOnlyDictionary<string, XNamespace> namespaces;
        private readonly IXElementParsingUtility parser;

        public USGaapElement(XElement xElement, IReadOnlyDictionary<string, XNamespace> namespaces, IXElementParsingUtility parser)
        {
            this.xElement = xElement;
            this.namespaces = namespaces;
            this.parser = parser;
        }
        public bool Abstract { get {
                try { 
                    bool value = this.parser.GetAttributeValue<bool>(this.xElement, LocalNamesAndPrefixes.AbstractAttribute, LocalNamesAndPrefixes.XsPrefix, this.namespaces);
                    return value;
                } 
                catch (InvalidOperationException) { return false; } 
            } 
        }
        public string Id => this.parser.GetAttributeValue<string>(this.xElement, LocalNamesAndPrefixes.IdAttribute, LocalNamesAndPrefixes.XsPrefix, this.namespaces);
        public string Name => this.parser.GetAttributeValue<string>(this.xElement, LocalNamesAndPrefixes.IdAttribute, LocalNamesAndPrefixes.XsPrefix, this.namespaces);
        public bool Nillable => this.parser.GetAttributeValue<bool>(this.xElement, LocalNamesAndPrefixes.Nillable, LocalNamesAndPrefixes.XsPrefix, this.namespaces);
    }
}
