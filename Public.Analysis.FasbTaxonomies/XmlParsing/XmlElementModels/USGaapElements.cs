using Public.Analysis.FasbTaxonomies.Parsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.Xml;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XmlElementModels
{
    public class USGaapElements
    {
        private readonly XDocument eltsXDoc;
        private IReadOnlyDictionary<string, XNamespace> namespacesByPrefix;
        private readonly IXElementParsingUtility xElementParsingUtility;

        public USGaapElements(XDocument eltsXsdDoc, IXElementParsingUtility xElementParsingUtility)
        {
            this.eltsXDoc = eltsXsdDoc;
            this.xElementParsingUtility = xElementParsingUtility;
            this.namespacesByPrefix = this.xElementParsingUtility.RootNameSpacesByPrefix(eltsXsdDoc);
        }

        public IEnumerable<USGaapElement> GetElements()
        {

            return this.xElementParsingUtility.GetDescendants(this.eltsXDoc, LocalNamesAndPrefixes.XsElement, LocalNamesAndPrefixes.XsPrefix, this.namespacesByPrefix)
                .Select(xElement=>new USGaapElement(xElement, this.namespacesByPrefix, this.xElementParsingUtility))
                .ToList() 
                ?? Enumerable.Empty<USGaapElement>();
        }
    }
}
