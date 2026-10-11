using System.Collections.ObjectModel;

namespace Public.Analysis.Console.CompanyFacts.DerivedFacts.TangibleBookValue
{
    public class TangibleBookValueOptions
    {
        public const string ConfigSectionName = "TangibleBookValueOptions";
        public Dictionary<string, string> AssetsFactNames { get; set; } = new Dictionary<string, string>(new Dictionary<string,string>());
        public Dictionary<string, string> LiabilitiesFactNames { get; set; } = new Dictionary<string, string>(new Dictionary<string, string>());
        public Dictionary<string, string> IntangiblesFactNames { get; set; } = new Dictionary<string, string>(new Dictionary<string, string>());
    }
}