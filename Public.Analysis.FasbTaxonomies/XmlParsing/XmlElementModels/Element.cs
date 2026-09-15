using Public.Analysis.FasbTaxonomies.Parsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.Xml
{
    public class Element
    {
        private readonly IXElementParsingHelper parser;

        public Element(IXElementParsingHelper parser)
        {
            this.parser = parser;
        }
        public bool Abstract => this.parser.GetAttributeValue<bool>(LocalNamesAndPrefixes.AbstractAttribute, LocalNamesAndPrefixes.XsPrefix);
        public string Id => this.parser.GetAttributeValue<string>(LocalNamesAndPrefixes.IdAttribute, LocalNamesAndPrefixes.XsPrefix);
        public string Name => this.parser.GetAttributeValue<string>(LocalNamesAndPrefixes.IdAttribute, LocalNamesAndPrefixes.XsPrefix);
        public bool Nillable => this.parser.GetAttributeValue<bool>(LocalNamesAndPrefixes.Nillable, LocalNamesAndPrefixes.XsPrefix);
    }
}
