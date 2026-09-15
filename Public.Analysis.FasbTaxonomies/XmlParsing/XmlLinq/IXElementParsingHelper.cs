namespace Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq
{
    public interface IXElementParsingHelper
    {
        T GetAttributeValue<T>(string localName, string namespacePrefix = "");
    }
}