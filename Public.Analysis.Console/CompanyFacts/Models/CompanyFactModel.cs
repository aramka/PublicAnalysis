using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.CompanyFacts.Models
{
    using System;
    using System.Text.Json.Serialization;

    public class CompanyFactModel
    {
        [JsonPropertyName("end")]
        public string? EndDate { get; set; }
        public long EndDateUnixSeconds => EndDate is not null ? DateTimeOffset.ParseExact(EndDate, "yyyy-MM-dd", null).ToUnixTimeSeconds() : long.MinValue;

        [JsonPropertyName("val")]
        public decimal? Value { get; set; }

        [JsonPropertyName("accn")]
        public string? AccessionNumber { get; set; }

        [JsonPropertyName("fy")]
        public int? FiscalYear { get; set; }

        [JsonPropertyName("fp")]
        public string? FiscalPeriod { get; set; }

        [JsonPropertyName("form")]
        public string? Form { get; set; }

        [JsonPropertyName("filed")]
        public string? FiledDate { get; set; }

        public long FiledDateUnixSeconds => FiledDate is not null ? DateTimeOffset.ParseExact(FiledDate, "yyyy-MM-dd", null).ToUnixTimeSeconds() : long.MinValue;

        [JsonPropertyName("start")]
        public string? StartDate { get; set; }

        public long? StartDateUnixSeconds => StartDate is not null ? DateTimeOffset.ParseExact(StartDate, "yyyy-MM-dd", null).ToUnixTimeSeconds() : long.MinValue;
    }
}
