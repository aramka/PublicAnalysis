using Public.Analysis.FasbTaxonomies.Parsing.Xml;
using Public.Analysis.FasbTaxonomies.XmlLinq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.Parsing.Xrbl
{
    public class RoleRef
    {
        private XElement roleRefElement;
        private readonly IReadOnlyDictionary<string, XNamespace>? nameSpaces;
        private HRef? hRef=null;
        private Uri? roleUri=null;
        private string? xLinkType=null;

        public RoleRef(XElement roleRefElement, IReadOnlyDictionary<string, XNamespace>? nameSpaces = null)
        {
            this.roleRefElement = roleRefElement;
            this.nameSpaces = nameSpaces;
        }

        public HRef HRef
        {
            get
            {
                if (this.hRef is null)
                {

                    string href = this.roleRefElement.GetAttributeValue(LocalNamesAndPrefixes.HRefAttribute, LocalNamesAndPrefixes.XLinkPrefix, this.nameSpaces);
                    this.hRef = new HRef(href);
                }

                return this.hRef;
            }
        }

        public Uri RoleUri
        {
            get
            {
                if (this.roleUri is null)
                {
                    string roleUriString = this.roleRefElement.GetAttributeValue(LocalNamesAndPrefixes.RoleUriAttribute);
                    this.roleUri = new Uri(roleUriString);
                }
                return this.roleUri;
            }
        }

        public string XLinkType
        {
            get
            {
                if (this.xLinkType is null)
                {
                    this.xLinkType = this.roleRefElement.GetAttributeValue(LocalNamesAndPrefixes.TypeAttribute, LocalNamesAndPrefixes.XLinkPrefix, this.nameSpaces);
                }
                return this.xLinkType;
            }
        }

    }
}
