using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.Xml
{
    public class HRef
    {
        private string hRef;
        private string[] parts;

        public HRef(string hRef)
        {
            this.hRef = hRef;
            this.parts = hRef?.Split('#', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();

            if(this.parts.Length != 2)
            {
                throw new InvalidOperationException("Invalid hRef. hRef must be of the form {location}#{anchor}");
            }
        }

        public string Anchor => parts[1];

        public string? Location => parts[0];
    }
}
