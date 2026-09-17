using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq
{
    public interface IXElementParsingUtility
    {
        T GetAttributeValue<T>(XElement element, string localName, string namespacePrefix = "", IReadOnlyDictionary<string, XNamespace>? nameSpaces = null);
        string GetAttributeValue(XElement element, string localName, string namespacePrefix = "", IReadOnlyDictionary<string, XNamespace>? nameSpaces = null);

        IEnumerable<XElement> GetDescendants(XContainer element, string localName, string namespacePrefix = "", IReadOnlyDictionary<string, XNamespace>? nameSpaces = null);
        IReadOnlyDictionary<string, XNamespace> RootNameSpacesByPrefix(XDocument xDoc);
    }
}