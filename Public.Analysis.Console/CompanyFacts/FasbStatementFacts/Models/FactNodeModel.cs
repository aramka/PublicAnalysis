using Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels;
using System.Text.Json.Serialization;


namespace Public.Analysis.Console.CompanyFacts.FasbStatementFacts.Models
{

    public record FactNodeModel(
        [property: JsonPropertyName("XsElement")] FactInfoModel XsElement,
        [property: JsonPropertyName("ParentsElementIds")] IReadOnlyList<string> ParentsElementIds,
        [property: JsonPropertyName("Children")] IReadOnlyList<FactNodeChild> Children,
        [property: JsonPropertyName("Label")] string Label,
        [property: JsonPropertyName("ElementId")] string ElementId
    ) : IFactNode
    {
        public FactNodeModel() : this(new FactInfoModel(), Array.Empty<string>(), Array.Empty<FactNodeChild>(), string.Empty, string.Empty) { }

        private IReadOnlyDictionary<string, List<FactNodeChild>> childrenByElementId = new Dictionary<string, List<FactNodeChild>>();

        public IReadOnlyDictionary<string, List<FactNodeChild>> ChildrenByElementId
        {
            get
            {
                if (this.Children is null)
                {
                    return new Dictionary<string, List<FactNodeChild>>();

                }
                this.childrenByElementId = this.Children.GroupBy(c => c.ChildElementId).ToDictionary(g => g.Key, g => g.ToList());
                return this.childrenByElementId;
            }
        }

        /// <summary>
        /// Create a new FactNodeModel copying this instance's properties when a corresponding
        /// optional parameter is not provided.
        /// </summary>
        public FactNodeModel New(
            FactInfoModel? xsElement = null,
            IReadOnlyList<string>? parentsElementIds = null,
            IReadOnlyList<FactNodeChild>? children = null,
            string? label = null,
            string? elementId = null)
        {
            return new FactNodeModel(
                xsElement ?? this.XsElement,
                parentsElementIds ?? this.ParentsElementIds,
                children ?? this.Children,
                label ?? this.Label,
                elementId ?? this.ElementId
            );
        }

        // Compatibility convenience property
        public string Name => this.XsElement?.Name ?? string.Empty;

        string IFactNode.Id => this.ElementId;

        IReadOnlyList<string>? IFactNode.ParentsIds => this.ParentsElementIds;

        IReadOnlyList<IFactNodeChild>? IFactNode.Children => this.Children?.Cast<IFactNodeChild>().ToArray();
    }
}
