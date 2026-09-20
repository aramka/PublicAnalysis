using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.StatementTree
{
    public record ElementIdRecord(string elementId)
    {
        public ElementIdRecord(IXsElement element) : this(element.Id) { }
    }
}
