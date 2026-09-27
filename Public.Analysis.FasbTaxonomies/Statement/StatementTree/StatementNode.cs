using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree
{
    public class StatementNode : IStatementNode
    {
        public StatementNode(IXsElement xsElement, string label)
        {
            this.XsElement = xsElement;
            this.Label = label;
        }
        public IXsElement XsElement { get; }

        private HashSet<ElementId> parentsElementIds = new HashSet<ElementId>();
        public IEnumerable<ElementId> ParentsElementIds => this.parentsElementIds.Select(p => p);

        public void AddParent(ElementId elementId)
        {
            this.parentsElementIds.Add(elementId);
        }
        public string Label { get; }

        private readonly HashSet<ChildStatementNode> children = new HashSet<ChildStatementNode>();

        public void AddChild(ElementId elementId, string childLabel, decimal order)
        {
            var childNode = new ChildStatementNode(elementId, childLabel, order);
            if (children.Contains(childNode))
            {
                throw new InvalidOperationException($"Child {childNode} already in children from parent with ElementId {this.ElementId}");
            }

            this.children.Add(childNode);
        }

        public IEnumerable<ChildStatementNode> Children => this.children.Select(e=>e).ToList();

        public ElementId ElementId => this.XsElement.ElementId;
    }
}
