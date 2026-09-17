using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq
{
    public interface IXElementParsingUtility
    {
        T GetAttributeValue<T>(XElement xElement, IReadOnlyDictionary<string, XNamespace> nameSpaces, string localName, string namespacePrefix = "");
        string GetAttributeValue(XElement xElement, IReadOnlyDictionary<string, XNamespace> nameSpaces, string localName, string namespacePrefix = "");

        IEnumerable<XElement> GetDescendants(XContainer element, string localName, string namespacePrefix = "", IReadOnlyDictionary<string, XNamespace>? nameSpaces = null);
        IReadOnlyDictionary<string, XNamespace> RootNameSpacesByPrefix(XDocument xDoc);
    }
}