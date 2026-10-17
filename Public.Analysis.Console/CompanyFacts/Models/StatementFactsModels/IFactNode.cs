using System.Collections.Generic;

namespace Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels
{
    public interface IFactNode
    {
        string Id { get; }

        string Label { get; }

        IReadOnlyList<string>? ParentsIds { get; }

        IReadOnlyList<IFactNodeChild>? Children { get; }

        string Name { get;  }
    }
}
