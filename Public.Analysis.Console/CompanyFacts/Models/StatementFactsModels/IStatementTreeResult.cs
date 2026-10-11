using System.Collections.Generic;

namespace Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels
{
    public interface IStatementTreeResult
    {

        int TotalTreeFactsCount { get; }

        int TotalTickerFactsCount { get; }

        int MatchingFactsCount { get; }

        decimal Coverage { get; }

        IReadOnlyDictionary<string, IFactNodeVisualsModel> Tree { get; }

        string Description { get; }
    }
}
