using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlLinq
{
    public static class XElementExtensions
    {
        public static string GetAttributeValue(this XElement element, string localName, string namespacePrefix = "",IReadOnlyDictionary<string, XNamespace>? nameSpaces = null)
        {
            nameSpaces = nameSpaces ?? new Dictionary<string, XNamespace>();
            Dictionary<string, XNamespace> nameSpacesWithLocalOverrides = new Dictionary<string, XNamespace>(nameSpaces);

            element
                .Attributes()
                .Where(a => a.IsNamespaceDeclaration)
                .ToList()
                .ForEach(a => nameSpacesWithLocalOverrides[a.Name.LocalName] = (XNamespace)a.Value);
            XNamespace ns = nameSpacesWithLocalOverrides.TryGetValue(namespacePrefix, out XNamespace? v) switch { true => v, _ => XNamespace.None };

            var attribute = element.Attribute(ns + localName);
            if (attribute == null)
            {
                throw new InvalidOperationException($"Attribute {ns + localName} is missing.");
            }
            return attribute.Value;
        }
    }
}
