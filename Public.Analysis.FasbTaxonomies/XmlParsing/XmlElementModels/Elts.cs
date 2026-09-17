using Public.Analysis.FasbTaxonomies.Parsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.Xml;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XmlElementModels
{
    public class Elts
    {
        private readonly XDocument eltsXDoc;
        private IReadOnlyDictionary<string, XNamespace> namespacesByPrefix;
        private readonly IXElementParsingUtility xElementParsingUtility;

        public Elts(XDocument eltsXsdDoc, IXElementParsingUtility xElementParsingUtility)
        {
            this.eltsXDoc = eltsXsdDoc;
            this.xElementParsingUtility = xElementParsingUtility;
            this.namespacesByPrefix = this.xElementParsingUtility.RootNameSpacesByPrefix(eltsXsdDoc);
        }

        public IEnumerable<Element> GetElements()
        {

            return this.xElementParsingUtility.GetDescendants(this.eltsXDoc, LocalNamesAndPrefixes.XsElement, LocalNamesAndPrefixes.XsPrefix, this.namespacesByPrefix)
                .Select(xElement=>new Element(xElement, this.namespacesByPrefix, this.xElementParsingUtility))
                .ToList() 
                ?? Enumerable.Empty<Element>();
        }
    }
}
