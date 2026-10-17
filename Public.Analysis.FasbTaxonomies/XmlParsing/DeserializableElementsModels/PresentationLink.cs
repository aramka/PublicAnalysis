using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels
{

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "http://www.xbrl.org/2003/linkbase", TypeName = "link")]
    public partial class PresentationLink
    {

        private Loc[] locField = [];

        private Arc[] presentationArcField = [];

        private string roleField = string.Empty;

        private string typeField = string.Empty;


        [XmlElement("loc")]
        public Loc[] Locs
        {
            get
            {
                return this.locField;
            }
            set
            {
                this.locField = value;
            }
        }


        [XmlElement("presentationArc")]
        public Arc[] Arcs
        {
            get
            {
                return this.presentationArcField;
            }
            set
            {
                this.presentationArcField = value;
            }
        }


        [XmlAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink", AttributeName ="role")]
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


        [XmlAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink", AttributeName ="type")]
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
