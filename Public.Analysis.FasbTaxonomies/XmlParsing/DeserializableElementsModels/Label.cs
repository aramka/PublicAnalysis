using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels
{
    /// <remarks/>
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.xbrl.org/2003/linkbase", TypeName ="label")]
    public partial class Label
    {

        private string idField = string.Empty;

        private string labelField = string.Empty;

        private string roleField = string.Empty;

        private string typeField = string.Empty;

        private string langField = string.Empty;

        private string valueField = string.Empty;


        [XmlAttribute("id")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }


        [XmlAttribute("label", Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
        public string XLinkLabel
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }


        [XmlAttribute("role", Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
        public string Role
        {
            get
            {
                return this.roleField;
            }
            set
            {
                this.roleField = value;
            }
        }


        [XmlAttribute("type", Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
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


        [XmlAttribute("lang", Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string Lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }


        [XmlText()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}
