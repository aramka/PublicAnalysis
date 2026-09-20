using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.StatementTree
{
    public class LabelsBuilder : ILabelsBuilder
    {
        public IReadOnlyDictionary<ElementIdRecord, string> BuildLabels(LabelLink labelLink)
        {
            var elementLocsByXLinkLabel = labelLink.Locs.ToDictionary(l => l.XLinkLabel);
            var labelLocsByXLinkLabel = labelLink.Labels.ToDictionary(l => l.XLinkLabel);

            var labelsDict = labelLink.Arcs.ToDictionary(a => elementLocsByXLinkLabel[a.From].ElementId, arc => labelLocsByXLinkLabel[arc.To].Value);

            return labelsDict;
        }
    }
}
