using System.Collections.Generic;
using Public.Analysis.Console.Visuals.Models;

namespace Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels
{
    public interface IFactNodeVisualsModel
    {
        IFactNode FactNode { get; }

        IReadOnlyList<VisualType> Visuals { get; }
    }
}
