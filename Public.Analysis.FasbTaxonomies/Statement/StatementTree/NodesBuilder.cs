using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree
{
    public class NodesBuilder
    {
        public Dictionary<ElementId, IStatementNode> BuildNodes(PresentationLink presentationLink, IReadOnlyDictionary<ElementId, IXsElement> elementsByElementId, IReadOnlyDictionary<ElementId, string> labelsByElementId)
        {
            Dictionary<string, IEnumerable<Arc>> arcsByFrom = presentationLink.Arcs.GroupBy(a => a.From).ToDictionary(a => a.Key, a => a.Select(b => b));

            Dictionary<string, Loc> locsByXLinkLabel = presentationLink.Locs.ToDictionary(l => l.XLinkLabel);

            Dictionary<ElementId, IStatementNode> nodes = new Dictionary<ElementId, IStatementNode>();

            foreach (Loc loc in presentationLink.Locs) //foreach(var arc in presentationLink.Arcs)
            {
                //if(!locsByXLinkLabel.TryGetValue(arc.From, out Loc? parentLoc))
                //{
                //    throw new InvalidOperationException($"Locator From->{arc.From} not found for arc relation From->{arc.From}, To->{arc.To}");
                //}


                if (!elementsByElementId.TryGetValue(loc.ElementId, out IXsElement? element))
                {
                    throw new InvalidOperationException($"Element {loc.ElementId} not found.");
                }

                if (!labelsByElementId.TryGetValue(loc.ElementId, out string? label))
                {
                    throw new InvalidOperationException($"Label for {loc.ElementId} not found.");
                }

                if (!nodes.TryGetValue(loc.ElementId, out IStatementNode? node))
                {
                    node = new StatementNode(element, label, 0);
                    nodes.Add(node.ElementId, node);
                }

                if (arcsByFrom.TryGetValue(loc.XLinkLabel, out IEnumerable<Arc>? parentChildArcs))
                {
                    foreach (Arc arc in parentChildArcs)
                    {
                        if (!locsByXLinkLabel.TryGetValue(arc.To, out Loc? childLoc))
                        {
                            throw new InvalidOperationException($"Loc for {nameof(Arc)}.To->{arc.To} not found for arc relation {nameof(Arc)}.From->{arc.From}, {nameof(Arc)}.To->{arc.To}");
                        }
                        if (!elementsByElementId.TryGetValue(childLoc.ElementId, out IXsElement? childElement))
                        {
                            throw new InvalidOperationException($"Element {childLoc.ElementId} not found.");
                        }
                        if (!labelsByElementId.TryGetValue(childLoc.ElementId, out string? childLabel))
                        {
                            throw new InvalidOperationException($"Label {childLoc.ElementId} not found.");
                        }

                        if (!nodes.TryGetValue(childElement.ElementId, out IStatementNode? childNode))
                        {
                            childNode = new StatementNode(childElement, childLabel, 0);
                            nodes.Add(childNode.ElementId, childNode);
                        }
                        node.AddChild(childNode);
                        childNode.ParentElementId = node.ElementId;
                        childNode.Order = arc.Order;
                    }
                }

            }

            return nodes;
        }
    }
}
