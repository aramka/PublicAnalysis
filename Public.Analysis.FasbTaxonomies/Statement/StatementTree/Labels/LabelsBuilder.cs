using Public.Analysis.FasbTaxonomies.XmlParsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree.Labels
{
    public class LabelsBuilder : ILabelsBuilder
    {
        public IReadOnlyDictionary<ElementId, IReadOnlyDictionary<LabelRole, string>> BuildLabels(LabelLink labelLink)
        {
            Dictionary<string, Dictionary<LabelRole, string>> labelsByXLinkLabel =
                labelLink.Labels.GroupBy(l => l.XLinkLabel)
                .ToDictionary(g => g.Key, g => g.ToDictionary(l => new LabelRole(l.LabelRole),l=>l.Value));

            Dictionary<string, ElementId> elementIdsByXLinkLabel = labelLink.Locs.ToDictionary(loc => loc.XLinkLabel, loc => loc.ElementId);

            Dictionary<ElementId, IReadOnlyDictionary<LabelRole, string>> labelsByElementThenLabelRole = new Dictionary<ElementId, IReadOnlyDictionary<LabelRole, string>>();

            foreach(var arc in labelLink.Arcs)
            {
                if (!elementIdsByXLinkLabel.TryGetValue(arc.From, out ElementId? elementId))
                {
                    throw new InvalidOperationException($"Arc from {arc.From} was not found in {nameof(LabelLink.Locs)}.");
                }
                if (!labelsByXLinkLabel.TryGetValue(arc.To, out Dictionary<LabelRole,string>? labelsByLabelRole))
                {
                    throw new InvalidOperationException($"Label from {arc.To} was not found in {nameof(LabelLink.Labels)}.");
                }
                labelsByElementThenLabelRole[elementId] = labelsByLabelRole;
            }

            return labelsByElementThenLabelRole;
            
        }
    }
}
