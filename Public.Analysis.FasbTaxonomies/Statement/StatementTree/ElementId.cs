using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree
{
    public record ElementId(string elementId)
    {
        public ElementId(IXsElement element) : this(element.Id) { }
    }
}
