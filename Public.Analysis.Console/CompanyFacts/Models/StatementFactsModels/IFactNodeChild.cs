using System;

namespace Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels
{
    public interface IFactNodeChild
    {
        string Id { get; }

        string Label { get; }

        int Order { get; }
    }
}
