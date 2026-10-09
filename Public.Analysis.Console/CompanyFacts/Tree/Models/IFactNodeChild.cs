using System;

namespace Public.Analysis.Console.CompanyFacts.Tree.Models
{
    public interface IFactNodeChild
    {
        string ChildId { get; }

        string ChildLabel { get; }

        int Order { get; }
    }
}
