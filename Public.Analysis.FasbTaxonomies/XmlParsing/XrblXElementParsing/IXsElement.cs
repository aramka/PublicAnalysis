using Public.Analysis.FasbTaxonomies.Statement.StatementTree;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing
{
    public interface IXsElement
    {
        public ElementId ElementId { get; }
        bool Abstract { get; }
        string Balance { get; }
        string Id { get; }
        string Name { get; }
        bool Nillable { get; }
        string PeriodType { get; }
        string Type { get; }
    }
}