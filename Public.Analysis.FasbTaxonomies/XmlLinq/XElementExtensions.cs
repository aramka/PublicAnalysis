using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlLinq
{
    public static class XElementExtensions
    {
        public static string GetAttributeValue(this XElement element, string localName, string namespacePrefix = "",IReadOnlyDictionary<string, XNamespace>? nameSpaces = null)
        {
            XNamespace ns = element.GetExpandedNameSpaceForPrefix(namespacePrefix, nameSpaces);

            var attribute = element.Attribute(ns + localName);
            if (attribute == null)
            {
                throw new InvalidOperationException($"Attribute {ns + localName} is missing.");
            }
            return attribute.Value;
        }
        public static XElement GetDescendant(this XContainer element, string localName, string namespacePrefix = "", IReadOnlyDictionary<string,XNamespace>? nameSpaces = null)
        {
            XNamespace ns = element.GetExpandedNameSpaceForPrefix(namespacePrefix,nameSpaces);
            var descendants = element.Descendants(ns + localName);

            var cnt = descendants.Count();

            if (cnt > 1)
            {
                throw new InvalidOperationException($"More than one descendant element found for {ns + localName}.");
            }
            if (cnt == 0)
            {
                throw new InvalidOperationException($"No descendent element found for {ns + localName}");
            }

            return descendants.Single();
        }
        private static XNamespace GetExpandedNameSpaceForPrefix(this XContainer xElement, string prefix, IReadOnlyDictionary<string, XNamespace>? nameSpaces)
        {
            if(nameSpaces is null)
            {
                return XNamespace.None;
            }
            XNamespace ns = nameSpaces.TryGetValue(prefix, out XNamespace? v) switch { true => v, _ => XNamespace.None };
            return ns;
        }
    }
}
