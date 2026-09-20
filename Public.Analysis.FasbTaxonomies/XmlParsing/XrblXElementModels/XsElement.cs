using Public.Analysis.FasbTaxonomies.XmlParsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels
{
    public class XsElement : IXsElement
    {
        private readonly XElement xElement;
        private readonly IReadOnlyDictionary<string, XNamespace> namespaces;
        private readonly IXElementParsingUtility parser;

        public XsElement(XElement xElement, IReadOnlyDictionary<string, XNamespace> namespaces, IXElementParsingUtility parser)
        {
            this.xElement = xElement;
            this.namespaces = namespaces;
            this.parser = parser;
        }
        public bool Abstract
        {
            get
            {
                try
                {
                    bool value = this.parser.GetAttributeValue<bool>(this.xElement, LocalNamesAndPrefixes.AbstractAttribute, LocalNamesAndPrefixes.XsPrefix, this.namespaces);
                    return value;
                }
                catch (InvalidOperationException) { return false; }
            }
        }
        public string Id => this.parser.GetAttributeValue(this.xElement, LocalNamesAndPrefixes.IdAttribute, LocalNamesAndPrefixes.XsPrefix, this.namespaces);
        public string Name => this.parser.GetAttributeValue(this.xElement, LocalNamesAndPrefixes.IdAttribute, LocalNamesAndPrefixes.XsPrefix, this.namespaces);
        public bool Nillable => this.parser.GetAttributeValue<bool>(this.xElement, LocalNamesAndPrefixes.Nillable, LocalNamesAndPrefixes.XsPrefix, this.namespaces);

        public string @Type => this.parser.GetAttributeValue(this.xElement, LocalNamesAndPrefixes.TypeAttribute);

        public string Balance => this.parser.GetAttributeValue(this.xElement, LocalNamesAndPrefixes.BalanceAttribute, LocalNamesAndPrefixes.XbrliPrefix, this.namespaces);

        public string PeriodType => this.parser.GetAttributeValue(this.xElement, LocalNamesAndPrefixes.PeriodTypeAttribute, LocalNamesAndPrefixes.XbrliPrefix, this.namespaces);
    }
}
