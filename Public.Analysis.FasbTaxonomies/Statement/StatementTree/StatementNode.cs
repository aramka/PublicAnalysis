using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;
using System.Text.Json.Serialization;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree
{
    public class StatementNode : IStatementNode
    {
        public StatementNode(IXsElement xsElement, string label)
        {
            this.FactInfo = xsElement;
            this.Label = label;
        }
        [JsonIgnore]
        public IXsElement FactInfo { get; }

        private HashSet<ElementId> parentsElementIds = new HashSet<ElementId>();
        public IEnumerable<ElementId> ParentsElementIds => this.parentsElementIds.Select(p => p);

        public void AddParent(ElementId elementId)
        {
            this.parentsElementIds.Add(elementId);
        }
        public string Label { get; }

        private HashSet<ChildStatementNode> children = new HashSet<ChildStatementNode>();

        public void AddChild(ElementId elementId, string childLabel, decimal order)
        {
            var childNode = new ChildStatementNode(elementId, childLabel, order);
            if (children.Contains(childNode))
            {
                throw new InvalidOperationException($"Child {childNode} already in children from parent with ElementId {this.Id}");
            }

            this.children.Add(childNode);
        }

        public void RemoveParent(ElementId id)
        {
            this.parentsElementIds.Remove(id);
        }

        public void RemoveChild(ElementId Id)
        {
            this.children = this.children.Where(c => c.Id != Id).ToHashSet();
        }

        public IEnumerable<ChildStatementNode> Children => this.children.Select(e=>e).ToList();

        public ElementId Id => this.FactInfo.ElementId;

        public string Name => this.FactInfo.Name;
    }
}
