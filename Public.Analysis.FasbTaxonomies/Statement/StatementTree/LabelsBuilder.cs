using Public.Analysis.FasbTaxonomies.XmlParsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree
{
    public class LabelsBuilder : ILabelsBuilder
    {
        public IReadOnlyDictionary<ElementId, string> BuildLabels(LabelLink labelLink)
        {
            var elementLocsByXLinkLabel = labelLink.Locs.ToDictionary(l => l.XLinkLabel);
            var labelLocsByXLinkLabel = new Dictionary<string, Label>();
            
            foreach(var g in labelLink.Labels.GroupBy(l => l.XLinkLabel))
            {
                Label? l = null;
                if (g.Count() == 1)
                {
                    l = g.Single();
                }
                else 
                {
                    var totalLabels = g.Where(l => l.Role.EndsWith(LocalNamesAndPrefixes.XLinkRoleTotalLabelSuffix));
                    if (totalLabels.Count() != 1)
                    {
                        throw new InvalidOperationException($"More than one total label found for {nameof(Label.XLinkLabel)} {g.Key}");
                    }
                    l = totalLabels.Single();
                }

                labelLocsByXLinkLabel.Add(l.XLinkLabel, l);
            }

            Dictionary<ElementId, string> labelsDict = new Dictionary<ElementId, string>();

            foreach(Arc link in labelLink.Arcs)
            {
                if(!elementLocsByXLinkLabel.TryGetValue(link.From, out Loc? elementLocator))
                {
                    throw new InvalidOperationException($"Arc from {link.From} was not found in {nameof(LabelLink.Locs)}.");
                }
                if(!labelLocsByXLinkLabel.TryGetValue(link.To, out Label? labelLocator))
                {
                    throw new InvalidOperationException($"Label from {link.To} was not found in {nameof(LabelLink.Labels)}.");
                }
                labelsDict[elementLocator.ElementId] = labelLocator.Value;
            }
           
            return labelsDict;
        }
    }
}
