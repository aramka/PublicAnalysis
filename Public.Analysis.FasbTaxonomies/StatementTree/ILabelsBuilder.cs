using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.StatementTree
{
    public interface ILabelsBuilder
    {
        IReadOnlyDictionary<ElementId, string> BuildLabels(LabelLink labelLink);
    }
}
