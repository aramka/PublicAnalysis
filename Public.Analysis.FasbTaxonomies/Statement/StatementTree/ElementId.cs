using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Public.Analysis.FasbTaxonomies.Statement.StatementTree
{
    [JsonConverter(typeof(ElementIdJsonConverter))]
    public record ElementId(string elementId)
    {
        public ElementId(IXsElement element) : this(element.Id) { }

        public static ElementId Empty => new ElementId("");
    }
}
