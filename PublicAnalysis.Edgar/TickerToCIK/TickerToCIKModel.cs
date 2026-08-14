using System.Text.Json.Serialization;

namespace Public.Analysis.Edgar.TickerToCIK
{
    public class TickerToCIKModel
    {
        [JsonPropertyName("cik_str")]
        public int CikStr { get; set; }
        
        [JsonPropertyName("ticker")]
        public string? Ticker { get; set; }
        
        [JsonPropertyName("title")]
        public string? Title { get; set; }
    }
}