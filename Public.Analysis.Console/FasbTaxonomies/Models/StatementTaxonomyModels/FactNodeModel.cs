using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Public.Analysis.Console.FasbTaxonomies.Models.StatementTaxonomyModels
{
    public class FactNodeModel
    {
        [JsonPropertyName("XsElement")] public FactInfoModel StatementTaxonomyFactInfo { get; set; } = new FactInfoModel();
        [JsonPropertyName("ParentsElementIds")]
        public List<string> ParentsElementIds { get; set; } = new List<string>();

        [JsonPropertyName("Children")]
        public List<FactNodeChild> Children { get; set; } = new List<FactNodeChild>();

        [JsonPropertyName("Label")]
        public string Label { get; set; } = string.Empty;

        [JsonPropertyName("ElementId")]
        public string ElementId { get; set; } = string.Empty;
    }
}
