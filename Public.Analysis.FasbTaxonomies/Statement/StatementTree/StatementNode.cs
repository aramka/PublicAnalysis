using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree
{
    public class StatementNode : IStatementNode
    {
        public StatementNode(IXsElement xsElement, string label, decimal order)
        {
            this.XsElement = xsElement;
            this.Label = label;
            this.Order = order;
        }
        public IXsElement XsElement { get; }

        private HashSet<ElementId> parentsElementIds = new HashSet<ElementId>();
        public IEnumerable<ElementId> ParentsElementIds => this.parentsElementIds.Select(p => p);

        public void AddParent(ElementId elementId)
        {
            this.parentsElementIds.Add(elementId);
        }

        public string Label { get; }


        private readonly HashSet<ElementId> childElementIds = new HashSet<ElementId>();

        public void AddChild(ElementId elementId)
        {
            this.childElementIds.Add(elementId);
        }

        public IEnumerable<ElementId> ChildElementIds => this.childElementIds.Select(e=>e).ToList();

        public decimal Order { get; set; }

        public ElementId ElementId => this.XsElement.ElementId;
    }
}
