using Public.Analysis.FasbTaxonomies.Parsing.Xml;
using Public.Analysis.FasbTaxonomies.XmlLinq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.Parsing.Xrbl
{
    public class Loc
    {
        private readonly XElement locElement;
        private readonly IReadOnlyDictionary<string, XNamespace>? nameSpaces;
        private HRef? hRef = null;
        private string? label = null;
        private string? xLinkType = null;

        public Loc(XElement locElement, IReadOnlyDictionary<string, XNamespace>? nameSpaces = null)
        {
            this.locElement = locElement;
            this.nameSpaces = nameSpaces;
        }

        public HRef HRef
        {
            get
            {
                if (this.hRef is null)
                {
                    string href = this.locElement.GetAttributeValue(LocalNamesAndPrefixes.HRefAttribute, LocalNamesAndPrefixes.XLinkPrefix, this.nameSpaces);
                    this.hRef = new HRef(href);
                }

                return this.hRef;
            }
        }

        public string Label
        {
            get
            {
                if (this.label is null)
                {
                    this.label = this.locElement.GetAttributeValue("label", LocalNamesAndPrefixes.XLinkPrefix, this.nameSpaces);
                }
                return this.label;
            }
        }

        public string XLinkType
        {
            get
            {
                if (this.xLinkType is null)
                {
                    this.xLinkType = this.locElement.GetAttributeValue(LocalNamesAndPrefixes.TypeAttribute, LocalNamesAndPrefixes.XLinkPrefix, this.nameSpaces);
                }
                return this.xLinkType;
            }
        }
    }
}
