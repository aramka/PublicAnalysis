using System.Text.Json.Serialization;
using Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels;

namespace Public.Analysis.Console.CompanyFacts.FasbStatementFacts.Models
{

    public record FactNodeChild(
        [property: JsonPropertyName("Id")] string Id = "",
        [property: JsonPropertyName("Label")] string Label = "",
        [property: JsonPropertyName("Order")] int Order = 0
    ) : IFactNodeChild;
}
