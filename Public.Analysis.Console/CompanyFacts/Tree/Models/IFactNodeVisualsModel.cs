using System.Collections.Generic;
using Public.Analysis.Console.Visuals.Models;

namespace Public.Analysis.Console.CompanyFacts.Tree.Models
{
    public interface IFactNodeVisualsModel
    {
        IFactNode FactNode { get; }

        IList<VisualType> Visuals { get; }
    }
}
