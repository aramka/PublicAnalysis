using System;
using System.Collections.Generic;
using System.Linq;
using Public.Analysis.Console.Visuals.Models;
using Public.Analysis.Console.CompanyFacts.Tree.Models;

namespace Public.Analysis.Console.FasbTaxonomies.Models.StatementTaxonomyModels
{
    public class FactNodeVisualsModel : IFactNodeVisualsModel
    {
        public FactNodeModel FactNode { get; set; } = new FactNodeModel();
        public VisualType[] Visuals { get; set; } = Array.Empty<VisualType>();

        // Explicit interface implementations to avoid changing existing property names
        IFactNode IFactNodeVisualsModel.FactNode { get => this.FactNode; }
        IList<VisualType> IFactNodeVisualsModel.Visuals { get => this.Visuals?.ToList() ?? new List<VisualType>(); }
    }
}
