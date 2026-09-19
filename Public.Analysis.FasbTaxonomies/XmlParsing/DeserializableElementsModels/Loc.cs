using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels
{

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.xbrl.org/2003/linkbase", TypeName = "loc")]
    public partial class Loc
    {

        private string hrefField = string.Empty;

        private string labelField = string.Empty;

        private string typeField = string.Empty;


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

        [XmlIgnore]
        public HRef HRef => new HRef(this.Href);


        [XmlAttribute("label", Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
        public string Label
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
