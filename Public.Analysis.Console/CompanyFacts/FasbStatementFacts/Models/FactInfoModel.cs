using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Public.Analysis.Console.CompanyFacts.FasbStatementFacts.Models
{
    public record FactInfoModel(
        [property: JsonPropertyName("ElementId")] string ElementId = "",
        [property: JsonPropertyName("Abstract")] bool Abstract = false,
        [property: JsonPropertyName("Balance")] string Balance = "",
        [property: JsonPropertyName("Id")] string Id = "",
        [property: JsonPropertyName("Name")] string Name = "",
        [property: JsonPropertyName("Nillable")] bool Nillable = false,
        [property: JsonPropertyName("PeriodType")] string PeriodType = "",
        [property: JsonPropertyName("Type")] string Type = ""
    );
}
