using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq
{
    public interface IXElementParsingUtility
    {
        T GetAttributeValue<T>(XElement xElement, IReadOnlyDictionary<string, XNamespace> nameSpaces, string localName, string namespacePrefix = "");
    }
}