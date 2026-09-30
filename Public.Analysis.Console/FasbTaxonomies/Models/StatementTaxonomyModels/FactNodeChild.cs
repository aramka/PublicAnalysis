using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Public.Analysis.Console.FasbTaxonomies.Models.StatementTaxonomyModels
{
    public class FactNodeChild
    {
        [JsonPropertyName("ChildElementId")] public string ChildElementId { get; set; } = string.Empty;
        [JsonPropertyName("ChildLabel")]
        public string ChildLabel { get; set; } = string.Empty;

        [JsonPropertyName("Order")]
        public int Order { get; set; } = 0;
    }
}
