using System.Collections.Generic;

namespace Public.Analysis.Console.CompanyFacts.Tree.Models
{
    public interface IStatementTreeResult
    {
        string File { get; }

        int TotalTreeFactsCount { get; }

        int TotalTickerFactsCount { get; }

        int MatchingFactsCount { get; }

        decimal Coverage { get; }

        IDictionary<string, IFactNodeVisualsModel> Tree { get; }

        string Description { get; }
    }
}
