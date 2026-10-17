using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
using System.Text.Json.Serialization;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing
{
    public interface IXsElement
    {
        [JsonIgnore]
        public ElementId ElementId { get; }
        bool Abstract { get; }
        [JsonIgnore]
        string Balance { get; }
        [JsonIgnore]
        string Id { get; }
        string Name { get; }
        [JsonIgnore]
        bool Nillable { get; }
        [JsonIgnore]
        string PeriodType { get; }
        [JsonIgnore]
        string Type { get; }
    }
}