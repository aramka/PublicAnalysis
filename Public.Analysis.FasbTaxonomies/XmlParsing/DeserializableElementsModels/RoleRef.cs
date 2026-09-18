using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels
{


    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.xbrl.org/2003/linkbase")]
    public partial class RoleRef
    {

        private string roleURIField=string.Empty;

        private string hrefField=string.Empty;

        private string typeField=string.Empty;


        [XmlAttribute("roleURI")]
        public string RoleUri
        {
            get
            {
                return this.roleURIField;
            }
            set
            {
                this.roleURIField = value;
            }
        }


        [XmlAttribute("href", Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
        public string Href
        {
            get
            {
                return this.hrefField;
            }
            set
            {
                this.hrefField = value;
            }
        }


        [XmlAttribute("type", Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }
}
