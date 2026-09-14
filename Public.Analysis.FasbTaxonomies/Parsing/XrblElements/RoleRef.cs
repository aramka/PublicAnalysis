using Public.Analysis.FasbTaxonomies.XmlLinq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.Parsing.XrblElements
{
    public class RoleRef
    {
        private XElement roleRefElement;

        public RoleRef(XElement roleRefElement)
        {
            this.roleRefElement = roleRefElement;
        }

        public string HRef(IReadOnlyDictionary<string, XNamespace>? nameSpaces = null)
        {
            string href = this.roleRefElement.GetAttributeValue("href", "xlink", nameSpaces);
            return href;
        }

        public Uri RoleUri()
        {
            string roleUriString = this.roleRefElement.GetAttributeValue("roleURI");
            return new Uri(roleUriString);
        }

        public string XLinkType(IReadOnlyDictionary<string, XNamespace>? nameSpaces = null)
        {
            string xlinkType = this.roleRefElement.GetAttributeValue("type", "xlink", nameSpaces);
            return xlinkType;
        }

    }
}
