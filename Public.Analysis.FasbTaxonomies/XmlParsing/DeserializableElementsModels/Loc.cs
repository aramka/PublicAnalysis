using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels
{

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.xbrl.org/2003/linkbase", TypeName = "loc")]
    public partial class Loc
    {

        private string hrefField;

        private string labelField;

        private string typeField;


        [XmlAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
        public string href
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


        [XmlAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
        public string label
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


        [XmlAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
        public string type
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
