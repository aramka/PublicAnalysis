using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.StatementNodesBuilder
{
    public record ElementIdRecord(string elementId)
    {
        public ElementIdRecord(IXsElement element) : this(element.Id) { }
    }
}
