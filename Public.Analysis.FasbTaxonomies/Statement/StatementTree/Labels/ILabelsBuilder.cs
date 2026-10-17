using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree.Labels
{
    public interface ILabelsBuilder
    {
        IReadOnlyDictionary<ElementId, IReadOnlyDictionary<LabelRole,string>> BuildLabels(LabelLink labelLink);
    }
}
