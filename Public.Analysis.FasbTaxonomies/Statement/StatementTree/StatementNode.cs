using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree
{
    public class StatementNode : IStatementNode
    {
        private readonly Dictionary<ElementId, IStatementNode> children;

        public StatementNode(IXsElement xsElement, string label, decimal order)
        {
            this.XsElement = xsElement;
            this.Label = label;
            this.Order = order;
            this.children = new Dictionary<ElementId, IStatementNode>();
        }
        public IXsElement XsElement { get; }

        private List<ElementId> parentsElementIds = new List<ElementId>();
        public IEnumerable<ElementId> ParentsElementIds => this.parentsElementIds.Select(p => p);

        public void AddParent(ElementId parentElementId)
        {
            this.parentsElementIds.Add(parentElementId);
        }

        public string Label { get; }

        public void AddChild(IStatementNode child)
        {
            if(!this.children.TryAdd(child.ElementId, child))
            {
                throw new InvalidOperationException($"Child {child.ElementId} has already been added to node {this.ElementId}");
            }
        }

        public IEnumerable<IStatementNode> Children => this.children.Select(kvp=>kvp.Value).ToList();

        public decimal Order { get; set; }

        public ElementId ElementId => this.XsElement.ElementId;
    }
}
