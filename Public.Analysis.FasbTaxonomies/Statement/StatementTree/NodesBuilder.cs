using Public.Analysis.FasbTaxonomies.Statement.StatementTree.Labels;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree
{
    public class NodesBuilder : INodesBuilder
    {

        public Dictionary<ElementId, IStatementNode> BuildNodes(PresentationLink presentationLink, IReadOnlyDictionary<ElementId, IXsElement> elementsByElementId, IReadOnlyDictionary<ElementId, IReadOnlyDictionary<LabelRole, string>> labelsByElementId)
        {
            Dictionary<ElementId, IStatementNode> nodesByElementId = new Dictionary<ElementId, IStatementNode>();
            Dictionary<string, IStatementNode> nodesByXLinkLabel = new Dictionary<string, IStatementNode>();
            foreach(Loc loc in presentationLink.Locs)
            {
                if (!elementsByElementId.TryGetValue(loc.ElementId, out IXsElement? value))
                {
                    throw new InvalidOperationException($"Element {loc.ElementId} not found.");
                }
                string label = TryGetLabel( loc.ElementId, LabelRole.Label.Value, labelsByElementId);
                var statementNode = new StatementNode(value, label);
                nodesByElementId[loc.ElementId] = statementNode;
                nodesByXLinkLabel[loc.XLinkLabel] = statementNode;
            }

            foreach (Arc arc in presentationLink.Arcs)
            {
                if (!nodesByXLinkLabel.TryGetValue(arc.From, out IStatementNode? parent))
                {
                    throw new InvalidOperationException($"Loc for {nameof(Arc)}.From->{arc.From} not found for arc relation {nameof(Arc)}.From->{arc.From}, {nameof(Arc)}.To->{arc.To}");
                }
                if (!nodesByXLinkLabel.TryGetValue(arc.To, out IStatementNode? child))
                {
                    throw new InvalidOperationException($"Loc for {nameof(Arc)}.To->{arc.To} not found for arc relation {nameof(Arc)}.From->{arc.From}, {nameof(Arc)}.To->{arc.To}");
                }
                var childLabel = TryGetLabel(child.ElementId, arc.PreferredLabel, labelsByElementId);
                parent.AddChild(child.ElementId, childLabel, arc.Order);
                child.AddParent(parent.ElementId);
            }

            return nodesByElementId;
            
        }

        private static string TryGetLabel( ElementId elementId, string preferredLabel, IReadOnlyDictionary<ElementId, IReadOnlyDictionary<LabelRole, string>> labelsByElementId)
        {
            var labelRole = string.IsNullOrWhiteSpace(preferredLabel) ? LabelRole.Label : new LabelRole(preferredLabel);
            if (!labelsByElementId.TryGetValue(elementId, out var labelsByRole))
            {
                throw new InvalidOperationException($"Label for {elementId} not found.");
            }
            if (!labelsByRole.TryGetValue(labelRole, out string? label))
            {
                throw new InvalidOperationException($"Label for {elementId} and label role {labelRole} not found.");
            }

            return label;
        }
    }
}
