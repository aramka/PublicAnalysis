using Public.Analysis.FasbTaxonomies.XmlLinq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.Parsing.Xrbl
{
    public class PresentationLink
    {
        private readonly XElement presentationLinkElement;
        private readonly IReadOnlyDictionary<string, XNamespace> nameSpacesByPrefix;
        private string? roleAttributeValue = null;
        private string? xLinkTypeAttributeValue = null;

        public PresentationLink(XElement presentationLinkElement, IReadOnlyDictionary<string, XNamespace> nameSpacesByPrefix)
        {
            this.presentationLinkElement = presentationLinkElement;
            this.nameSpacesByPrefix = nameSpacesByPrefix;
        }

        public string RoleAttributeValue { 
            get
            {

                if (this.roleAttributeValue is not null)
                {
                    return this.roleAttributeValue;
                }

                this.roleAttributeValue = this.presentationLinkElement.GetAttributeValue(LocalNamesAndPrefixes.RoleAttribute, LocalNamesAndPrefixes.XLinkPrefix, this.nameSpacesByPrefix);
                return this.roleAttributeValue;
            } 
        }
        public string XLinkTypeAttributeValue {
            get
            {

                if (this.xLinkTypeAttributeValue is not null)
                {
                    return this.xLinkTypeAttributeValue;
                }

                this.xLinkTypeAttributeValue = this.presentationLinkElement.GetAttributeValue(LocalNamesAndPrefixes.Type, LocalNamesAndPrefixes.XLinkPrefix, this.nameSpacesByPrefix);
                return this.xLinkTypeAttributeValue;
            }
        }
    }
}
