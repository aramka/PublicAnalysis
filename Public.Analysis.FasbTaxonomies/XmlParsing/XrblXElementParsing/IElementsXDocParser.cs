namespace Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing
{
    public interface IElementsParser
    {
        IEnumerable<XsElement> GetElements();
    }
}