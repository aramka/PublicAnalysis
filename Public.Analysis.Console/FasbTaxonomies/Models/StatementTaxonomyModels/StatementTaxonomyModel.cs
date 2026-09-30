using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.FasbTaxonomies.Models.StatementTaxonomyModels
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    public class StatementTaxonomyModel
    {
        [JsonPropertyName("Name")] public string Name { get; set; } = string.Empty;
        [JsonPropertyName("Description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("Id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("Tree")]
        public Dictionary<string, FactNodeModel> Tree { get; set; } = new Dictionary<string, FactNodeModel>();
    }
}
