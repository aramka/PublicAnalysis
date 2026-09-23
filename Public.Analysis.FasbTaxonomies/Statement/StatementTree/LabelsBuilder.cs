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
                    var byXLinkRole = g.GroupBy(label => new Uri(label.Role).Segments.Last());
                    HashSet<string> suitableLabelRoles = new HashSet<string> { LocalNamesAndPrefixes.XLinkRoleTotalLabelSuffix, LocalNamesAndPrefixes.XLinkRoleStandardLabelSuffix };

                    var suitableLabel = byXLinkRole.FirstOrDefault(xLinkRoleGroup => suitableLabelRoles.Contains(xLinkRoleGroup.Key) && xLinkRoleGroup.Count() == 1)?.Single();
                    if(suitableLabel is null)
                    {
                        throw new InvalidOperationException($"No suitable label role found for {nameof(l.XLinkLabel)}");
                    }
                    l = suitableLabel;
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
