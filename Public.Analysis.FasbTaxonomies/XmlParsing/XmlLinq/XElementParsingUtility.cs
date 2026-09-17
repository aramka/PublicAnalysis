using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq
{
    public class XElementParsingUtility : IXElementParsingUtility
    {
        public T GetAttributeValue<T>(XElement xElement, IReadOnlyDictionary<string, XNamespace> nameSpaces,string localName, string namespacePrefix = "")
        {
            return xElement.GetAttributeValue<T>(localName, namespacePrefix, nameSpaces);
        }

    }
}
