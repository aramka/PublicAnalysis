using System.Text.Json.Serialization;

namespace Public.Analysis.Edgar.TickerToCIK
{
    public record TickerToCIKModel(
        [property: JsonPropertyName("cik_str")] int CikStr, 
        [property: JsonPropertyName("ticker")] string? Ticker, 
        [property: JsonPropertyName("title")] string? Title)
    {
        public string ToCIKString(int cikMaxLen)
        {
            return $"{this.CikStr.ToString().PadLeft(cikMaxLen, '0')}";
        }
    }
}