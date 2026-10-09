using Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels;
using System.Text.Json.Serialization;


namespace Public.Analysis.Console.CompanyFacts.FasbStatementFacts.Models
{

    public class FactNodeModel : IFactNode
    {
        [JsonPropertyName("XsElement")] public FactInfoModel XsElement { get; set; } = new FactInfoModel();
        [JsonPropertyName("ParentsElementIds")]
        public List<string> ParentsElementIds { get; set; } = new List<string>();


        private Dictionary<string, List<FactNodeChild>> childrenByElementId = new Dictionary<string, List<FactNodeChild>>();

        public Dictionary<string, List<FactNodeChild>> ChildrenByElementId
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
        [JsonPropertyName("Children")]
        public List<FactNodeChild> Children { get; set; } = new List<FactNodeChild>();


        [JsonPropertyName("Label")]
        public string Label { get; set; } = string.Empty;

        [JsonPropertyName("ElementId")]
        public string ElementId { get; set; } = string.Empty;

        // Explicit interface mappings to preserve existing property names
        string IFactNode.Id { get => this.ElementId; }
        string IFactNode.Label { get => this.Label; }
        IList<string>? IFactNode.ParentsIds { get => this.ParentsElementIds; }
        IList<IFactNodeChild>? IFactNode.Children { get => this.Children?.Cast<IFactNodeChild>().ToList(); }

        string IFactNode.Name => this.XsElement.Name;
    }
}
