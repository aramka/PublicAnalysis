using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Public.Analysis.Console.FasbTaxonomies.Models.StatementTaxonomyModels
{
    using Public.Analysis.Console.CompanyFacts.Tree.Models;

    public class FactNodeChild : IFactNodeChild
    {
        [JsonPropertyName("ChildElementId")] public string ChildElementId { get; set; } = string.Empty;
        [JsonPropertyName("ChildLabel")]
        public string ChildLabel { get; set; } = string.Empty;

        [JsonPropertyName("Order")]
        public int Order { get; set; } = 0;

        // Explicit interface mapping so we don't need to change existing property names
        string IFactNodeChild.ChildId { get => this.ChildElementId; }
        string IFactNodeChild.ChildLabel { get => this.ChildLabel; }
        int IFactNodeChild.Order { get => this.Order; }
    }
}
