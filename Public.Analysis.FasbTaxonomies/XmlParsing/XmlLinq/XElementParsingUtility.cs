using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq
{
    public class XElementParsingUtility : IXElementParsingUtility
    {
        public string GetAttributeValue(XElement  element, string localName, string namespacePrefix = "", IReadOnlyDictionary<string, XNamespace>? nameSpaces = null)
        {
            XNamespace ns = this.GetExpandedNameSpaceForPrefix(element, namespacePrefix, nameSpaces);

            var attribute = element.Attribute(ns + localName);
            if (attribute == null)
            {
                throw new InvalidOperationException($"Attribute {ns + localName} is missing.");
            }
            return attribute.Value;
        }
        public T GetAttributeValue<T>(XElement  element, string localName, string namespacePrefix = "", IReadOnlyDictionary<string, XNamespace>? nameSpaces = null)
        {
            string rawValue = this.GetAttributeValue(element, localName, namespacePrefix, nameSpaces);

            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromString(rawValue)!;
        }

        public XElement GetDescendant(XContainer element, string localName, string namespacePrefix = "", IReadOnlyDictionary<string, XNamespace>? nameSpaces = null)
        {
            var descendants = this.GetDescendants(element, localName, namespacePrefix, nameSpaces);
            var cnt = descendants.Count();

            if (cnt > 1)
            {
                XNamespace ns = this.GetExpandedNameSpaceForPrefix(element, namespacePrefix, nameSpaces);
                throw new InvalidOperationException($"More than one descendant element found for {ns + localName}.");
            }
            return descendants.Single();
        }

        public IEnumerable<XElement> GetDescendants(XContainer element, string localName, string namespacePrefix = "", IReadOnlyDictionary<string, XNamespace>? nameSpaces = null)
        {
            XNamespace ns = this.GetExpandedNameSpaceForPrefix(element, namespacePrefix, nameSpaces);
            
           var descendants = element.Descendants(ns + localName);

            if (!descendants.Any())
            {
                throw new InvalidOperationException($"No descendants found with name {ns + localName}");
            }
            return descendants;
        }

        public IReadOnlyDictionary<string, XNamespace> RootNameSpacesByPrefix(XDocument xDoc) => xDoc.Root!.Attributes().Where(a => a.IsNamespaceDeclaration).ToDictionary(a => a.Name.LocalName, a => (XNamespace)a.Value);

        private XNamespace GetExpandedNameSpaceForPrefix(XContainer xElement, string prefix, IReadOnlyDictionary<string, XNamespace>? nameSpaces)
        {
            if (nameSpaces is null || !nameSpaces.TryGetValue(prefix, out XNamespace? ns))
            {
                return XNamespace.None;
            }
            return ns;
        }
    }
}
