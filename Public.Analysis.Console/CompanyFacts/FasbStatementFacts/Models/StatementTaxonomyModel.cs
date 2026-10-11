namespace Public.Analysis.Console.CompanyFacts.FasbStatementFacts.Models
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    public record StatementTaxonomyModel(
        [property: JsonPropertyName("Name")] string Name = "",
        [property: JsonPropertyName("Description")] string Description = "",
        [property: JsonPropertyName("Id")] string Id = ""
    )
    {
        [property: JsonPropertyName("Tree")]
        public Dictionary<string, FactNodeModel> Tree { get; init; } = new Dictionary<string, FactNodeModel>();
    }
}
