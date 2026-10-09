using System.Collections.Generic;

namespace Public.Analysis.Console.CompanyFacts.Tree.Models
{
    public interface IFactNode
    {
        string Id { get; }

        string Label { get; }

        IList<string>? ParentsIds { get; }

        IList<IFactNodeChild>? Children { get; }

        string Name { get;  }
    }
}
