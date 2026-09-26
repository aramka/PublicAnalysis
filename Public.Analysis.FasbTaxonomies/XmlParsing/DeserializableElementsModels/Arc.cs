using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels
{
    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.xbrl.org/2003/linkbase")]
    public partial class Arc
    {

        private decimal orderField;

        private string arcroleField = string.Empty;

        private string fromField = string.Empty;

        private string toField = string.Empty;

        private string typeField = string.Empty;
        private string preferredLabelField = string.Empty;

        /// <remarks/>
        [XmlAttribute("order")]
        public decimal Order
        {
            get
            {
                return this.orderField;
            }
            set
            {
                this.orderField = value;
            }
        }

        [XmlAttribute("preferredLabel")]
        public string PreferredLabel
        {
            get { return this.preferredLabelField; }
            set { this.preferredLabelField = value; }
        }

        /// <remarks/>
        [XmlAttribute("arcrole", Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
        public string Arcrole
        {
            get
            {
                return this.arcroleField;
            }
            set
            {
                this.arcroleField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("from", Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
        public string From
        {
            get
            {
                return this.fromField;
            }
            set
            {
                this.fromField = value;
            }
        }

        /// <remarks/>
        [XmlAttribute("to", Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
        public string To
        {
            get
            {
                return this.toField;
            }
            set
            {
                this.toField = value;
            }
        }

        /// <remarks/>
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
