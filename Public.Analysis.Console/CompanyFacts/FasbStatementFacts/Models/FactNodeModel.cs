using Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels;
using System.Text.Json.Serialization;


namespace Public.Analysis.Console.CompanyFacts.FasbStatementFacts.Models
{

    public record FactNodeModel(
        [property: JsonPropertyName("ParentsElementIds")] IReadOnlyList<string> ParentsElementIds,
        [property: JsonPropertyName("Children")] IReadOnlyList<FactNodeChild> Children,
        [property: JsonPropertyName("Label")] string Label,
        [property: JsonPropertyName("Id")] string Id,
        [property: JsonPropertyName("Name")] string Name
    ) : IFactNode
    {
        public FactNodeModel() : this( Array.Empty<string>(), Array.Empty<FactNodeChild>(), string.Empty, string.Empty, string.Empty) { }

        private IReadOnlyDictionary<string, List<FactNodeChild>> childrenByElementId = new Dictionary<string, List<FactNodeChild>>();

        public IReadOnlyDictionary<string, List<FactNodeChild>> ChildrenByElementId
        {
            get
            {
                if (this.Children is null)
                {
                    return new Dictionary<string, List<FactNodeChild>>();

                }
                this.childrenByElementId = this.Children.GroupBy(c => c.Id).ToDictionary(g => g.Key, g => g.ToList());
                return this.childrenByElementId;
            }
        }

        /// <summary>
        /// Create a new FactNodeModel copying this instance's properties when a corresponding
        /// optional parameter is not provided.
        /// </summary>
        public FactNodeModel New(
            IReadOnlyList<string>? parentsElementIds = null,
            IReadOnlyList<FactNodeChild>? children = null,
            string? label = null,
            string? elementId = null,
            string? name = null)
        {
            return new FactNodeModel(
                parentsElementIds ?? this.ParentsElementIds,
                children ?? this.Children,
                label ?? this.Label,
                elementId ?? this.Id,
                name ?? this.Name
            );
        }

        string IFactNode.Id => this.Id;

        IReadOnlyList<string>? IFactNode.ParentsIds => this.ParentsElementIds;

        IReadOnlyList<IFactNodeChild>? IFactNode.Children => this.Children?.Cast<IFactNodeChild>().ToArray();
    }
}
