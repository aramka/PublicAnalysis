using Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels;
using Public.Analysis.Console.Visuals.Models;
using System.Text.Json.Serialization;

namespace Public.Analysis.Console.CompanyFacts.FasbStatementFacts.Models
{
    public record FactNodeVisualsModel(
        [property: JsonPropertyName("FactNode")] FactNodeModel FactNode,
        [property: JsonPropertyName("Visuals")] IReadOnlyList<VisualType> Visuals
    ) : IFactNodeVisualsModel
    {
        public FactNodeVisualsModel() : this(new FactNodeModel(), Array.Empty<VisualType>()) { }

        IFactNode IFactNodeVisualsModel.FactNode => this.FactNode;
    }
}
