using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
using Public.Analysis.FasbTaxonomies.XmlParsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing
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
                    bool value = this.parser.GetAttributeValue<bool>(this.xElement, LocalNamesAndPrefixes.AbstractAttribute);
                    return value;
                }
                catch (InvalidOperationException) { return false; }
            }
        }
        [JsonIgnore]
        public string Id => this.parser.GetAttributeValue(this.xElement, LocalNamesAndPrefixes.IdAttribute);
        public string Name => this.parser.GetAttributeValue(this.xElement, LocalNamesAndPrefixes.XsElementNameAttribute);

        [JsonIgnore]
        public bool Nillable => this.parser.GetAttributeValue<bool>(this.xElement, LocalNamesAndPrefixes.Nillable);

        [JsonIgnore]
        public string @Type => this.parser.GetAttributeValue(this.xElement, LocalNamesAndPrefixes.TypeAttribute);

        [JsonIgnore]
        public string Balance => this.parser.GetAttributeValue(this.xElement, LocalNamesAndPrefixes.BalanceAttribute, throwIfNotFound: false);

        [JsonIgnore]
        public string PeriodType => this.parser.GetAttributeValue(this.xElement, LocalNamesAndPrefixes.PeriodTypeAttribute, LocalNamesAndPrefixes.XbrliPrefix, namespaces, throwIfNotFound:false);

        [JsonIgnore]
        public ElementId ElementId => new ElementId(this.Id);
    }
}
