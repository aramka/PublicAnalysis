using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Public.Analysis.Console.FasbTaxonomies.Models.StatementTaxonomyModels
{
    public class FactInfoModel
    {
        [JsonPropertyName("ElementId")] public string ElementId { get; set; } = string.Empty;
        [JsonPropertyName("Abstract")]
        public bool Abstract { get; set; } = false;

        [JsonPropertyName("Balance")]
        public string Balance { get; set; } = string.Empty;

        [JsonPropertyName("Id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("Nillable")]
        public bool Nillable { get; set; } = false;

        [JsonPropertyName("PeriodType")]
        public string PeriodType { get; set; } = string.Empty;

        [JsonPropertyName("Type")]
        public string Type { get; set; } = string.Empty;
    }
}
