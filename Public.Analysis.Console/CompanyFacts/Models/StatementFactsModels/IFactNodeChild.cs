using System;

namespace Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels
{
    public interface IFactNodeChild
    {
        string ChildId { get; }

        string ChildLabel { get; }

        int Order { get; }
    }
}
