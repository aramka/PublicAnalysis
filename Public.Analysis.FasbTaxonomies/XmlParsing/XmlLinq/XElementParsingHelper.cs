using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq
{
    public class XElementParsingHelper : IXElementParsingHelper
    {
        private readonly XElement xElement;
        private readonly IReadOnlyDictionary<string, XNamespace> nameSpaces;

        public XElementParsingHelper(XElement xElement, IReadOnlyDictionary<string, XNamespace> nameSpaces)
        {
            this.xElement = xElement;
            this.nameSpaces = nameSpaces;
        }

        public T GetAttributeValue<T>(string localName, string namespacePrefix = "")
        {
            return this.xElement.GetAttributeValue<T>(localName, namespacePrefix, this.nameSpaces);
        }

    }
}
