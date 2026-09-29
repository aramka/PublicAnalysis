using Public.Analysis.Data;
using Public.Analysis.Edgar.TickerToCIK;
using Public.Frameworks.JsonQuery;
using System.Text.Json.Nodes;

namespace Public.Analysis.Edgar.RawFacts
{
    public interface IRawFactsData : IEdgarData
    {
        Task<IEnumerable<JsonNode>> GetRawFacts(TickerToCIKModel ticker, IEnumerable<IJsonQueryExpression> jsonQuery);
    }
}