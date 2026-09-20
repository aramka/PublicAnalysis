using Public.Analysis.FasbTaxonomies.StatementTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels
{
    public class LocationAndAnchor
    {
        private readonly string[] parts;

        public LocationAndAnchor(string hRef)
        {
            this.parts = hRef?.Split('#', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();

            if(this.parts.Length != 2)
            {
                throw new InvalidOperationException("Invalid hRef. hRef must be of the form {location}#{anchor}");
            }
        }

        public string Anchor => parts[1];

        public string? Location => parts[0];

        public ElementId ElementIdRecord => new ElementId(this.Anchor);
    }
}
