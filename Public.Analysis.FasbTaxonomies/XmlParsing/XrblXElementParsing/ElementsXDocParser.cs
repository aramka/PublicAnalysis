using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing
{
    public class ElementsXDocParser : IElementsParser
    {
        private readonly XDocument eltsXDoc;
        private IReadOnlyDictionary<string, XNamespace> namespacesByPrefix;
        private readonly IXElementParsingUtility xElementParsingUtility;

        public ElementsXDocParser(XDocument eltsXsdDoc, IXElementParsingUtility xElementParsingUtility)
        {
            this.eltsXDoc = eltsXsdDoc;
            this.xElementParsingUtility = xElementParsingUtility;
            this.namespacesByPrefix = this.xElementParsingUtility.RootNameSpacesByPrefix(eltsXsdDoc);
        }

        public ElementsXDocParser(StreamReader eltsFileStream, IXElementParsingUtility xElementParsingUtility):this(XDocument.Load(eltsFileStream), xElementParsingUtility)
        {
        }

        public IEnumerable<XsElement> GetElements()
        {

            return this.xElementParsingUtility.GetDescendants(this.eltsXDoc, LocalNamesAndPrefixes.XsElement, LocalNamesAndPrefixes.XsPrefix, this.namespacesByPrefix)
                .Select(xElement => new XsElement(xElement, this.namespacesByPrefix, this.xElementParsingUtility))
                .ToList()
                ?? Enumerable.Empty<XsElement>();
        }
    }
}
