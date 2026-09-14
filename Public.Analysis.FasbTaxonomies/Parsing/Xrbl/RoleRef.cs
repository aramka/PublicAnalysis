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
        private HRef? hRef=null;
        private Uri? roleUri=null;
        private string? xLinkType=null;

        public RoleRef(XElement roleRefElement)
        {
            this.roleRefElement = roleRefElement;
        }

        public HRef HRef(IReadOnlyDictionary<string, XNamespace>? nameSpaces = null)
        {
            if (this.hRef is null)
            {

                string href = this.roleRefElement.GetAttributeValue("href", "xlink", nameSpaces);
                this.hRef = new HRef(href);
            }

            return this.hRef;
        }

        public Uri RoleUri()
        {
            if (this.roleUri is null)
            {
                string roleUriString = this.roleRefElement.GetAttributeValue("roleURI");
                this.roleUri = new Uri(roleUriString);
            }
            return this.roleUri;
        }

        public string XLinkType(IReadOnlyDictionary<string, XNamespace>? nameSpaces = null)
        {
            if (this.xLinkType is null)
            {
                this.xLinkType = this.roleRefElement.GetAttributeValue("type", "xlink", nameSpaces);
            }
            return this.xLinkType;
        }

    }
}
