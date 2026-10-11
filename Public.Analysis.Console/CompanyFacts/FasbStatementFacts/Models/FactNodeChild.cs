using System.Text.Json.Serialization;
using Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels;

namespace Public.Analysis.Console.CompanyFacts.FasbStatementFacts.Models
{

    public record FactNodeChild(
        [property: JsonPropertyName("ChildElementId")] string ChildElementId = "",
        [property: JsonPropertyName("ChildLabel")] string ChildLabel = "",
        [property: JsonPropertyName("Order")] int Order = 0
    ) : IFactNodeChild
    {
        public string ChildId => this.ChildElementId;
    }
}
