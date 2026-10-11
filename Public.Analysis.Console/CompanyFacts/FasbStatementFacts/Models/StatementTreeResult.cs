using Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels;

namespace Public.Analysis.Console.CompanyFacts.FasbStatementFacts.Models
{
    public class StatementTreeResult : IStatementTreeResult
    {
        public int TotalTreeFactsCount { get; set; }
        public int TotalTickerFactsCount { get; set; }
        public int MatchingFactsCount { get; set; }
        public decimal Coverage { get; set; }
        public IReadOnlyDictionary<string, IFactNodeVisualsModel> Tree { get; set; } = new Dictionary<string, IFactNodeVisualsModel>();
        public string Description { get; set; } = string.Empty;

        IReadOnlyDictionary<string, IFactNodeVisualsModel> IStatementTreeResult.Tree => this.Tree.ToDictionary(kvp=>kvp.Key, kvp=> (kvp.Value as IFactNodeVisualsModel)!);
    }


}
