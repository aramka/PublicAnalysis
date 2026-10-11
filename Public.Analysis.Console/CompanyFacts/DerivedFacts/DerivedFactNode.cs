using Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.CompanyFacts.DerivedFacts
{
    public record DerivedFactNode(string Id, string Label,IReadOnlyList<string> ParentsIds) : IFactNode
    {
        public string Name => this.Id;
        public IReadOnlyList<IFactNodeChild>? Children => [];
    }
}
