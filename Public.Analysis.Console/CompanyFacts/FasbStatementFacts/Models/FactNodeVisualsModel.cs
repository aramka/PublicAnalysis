using Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels;
using Public.Analysis.Console.Visuals.Models;

namespace Public.Analysis.Console.CompanyFacts.FasbStatementFacts.Models
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
