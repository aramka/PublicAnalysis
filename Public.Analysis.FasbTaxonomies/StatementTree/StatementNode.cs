using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.StatementTree
{
    public class StatementNode : IStatementNode
    {
        private readonly List<IStatementNode> children;

        public StatementNode(IXsElement xsElement, string label, decimal order)
        {
            this.XsElement = xsElement;
            this.Label = label;
            this.Order = order;
            this.children = new List<IStatementNode>();
        }
        public IXsElement XsElement { get; }

        public ElementId? ParentElementId { get; set; }

        public string Label { get; }

        public void AddChild(IStatementNode child)
        {
            this.children.Add(child);
        }

        public IEnumerable<IStatementNode> Children => this.children.Select(c => c).ToList();

        public decimal Order { get; set; }

        public ElementId ElementId => this.XsElement.ElementId;
    }
}
