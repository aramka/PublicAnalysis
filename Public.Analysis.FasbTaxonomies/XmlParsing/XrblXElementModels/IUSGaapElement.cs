namespace Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels
{
    public interface IUSGaapElement
    {
        bool Abstract { get; }
        string Balance { get; }
        string Id { get; }
        string Name { get; }
        bool Nillable { get; }
        string PeriodType { get; }
        string Type { get; }
    }
}