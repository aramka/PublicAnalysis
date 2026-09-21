using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.StatementTree
{
    public class NodesBuilder
    {
        public Dictionary<ElementId, IStatementNode> BuildNodes(PresentationLink presentationLink, IReadOnlyDictionary<ElementId, IXsElement> elementsByElementId, IReadOnlyDictionary<ElementId, string> labelsByElementId)
        {
            Dictionary<string, Loc> locsByXLinkLabel = presentationLink.Locs.ToDictionary(l => l.XLinkLabel);

            Dictionary<ElementId, IStatementNode> nodes = new Dictionary<ElementId, IStatementNode>();

            foreach(var arc in presentationLink.Arcs)
            {
                if(!locsByXLinkLabel.TryGetValue(arc.From, out Loc? parentLoc))
                {
                    throw new InvalidOperationException($"Locator From->{arc.From} not found for arc relation From->{arc.From}, To->{arc.To}");
                }
                if(!locsByXLinkLabel.TryGetValue(arc.To, out Loc? childLoc))
                {
                    throw new InvalidOperationException($"Locator To->{arc.To} not found for arc relation From->{arc.From}, To->{arc.To}");
                }
                if(!elementsByElementId.TryGetValue(parentLoc.ElementId, out IXsElement? parentElement))
                {
                    throw new InvalidOperationException($"Element {parentLoc.ElementId} not found.");
                }
                if(!elementsByElementId.TryGetValue(childLoc.ElementId, out IXsElement? childElement))
                {
                    throw new InvalidOperationException($"Element {childLoc.ElementId} not found.");
                }
                if (!labelsByElementId.TryGetValue(parentLoc.ElementId, out string? parentLabel))
                {
                    throw new InvalidOperationException($"Label for {parentLoc.ElementId} not found.");
                }
                if (!labelsByElementId.TryGetValue(childLoc.ElementId, out string? childLabel))
                {
                    throw new InvalidOperationException($"Label {childLoc.ElementId} not found.");
                }

                BuildAndAddParentAndChildNodes(parentElement, parentLabel, childElement, childLabel, arc.Order, nodes);
            }

            return nodes;
        }

        private void BuildAndAddParentAndChildNodes(IXsElement parentElement, string parentLabel, IXsElement childElement, string childLabel, decimal order, Dictionary<ElementId, IStatementNode> existingNodes)
        {
            if(!existingNodes.TryGetValue(parentElement.ElementId, out IStatementNode? parentNode))
            {
                parentNode = new StatementNode(parentElement, parentLabel, 0);
                existingNodes.Add(parentNode.ElementId, parentNode);
            }
            if(!existingNodes.TryGetValue(childElement.ElementId, out IStatementNode? childNode))
            {
                childNode = new StatementNode(childElement, childLabel, 0);
                existingNodes.Add(childNode.ElementId, childNode);
            }
            parentNode.AddChild(childNode);
            childNode.ParentElementId = parentNode.ElementId;
            childNode.Order = order;
        }
    }
}
