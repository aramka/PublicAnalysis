using System.Xml.Schema;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels
{
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.xbrl.org/2003/linkbase", TypeName = "link")]
    public partial class LabelLink
    {

        private string roleField;

        private string typeField;
        private Label[] labels = [];
        private Arc[] arcs = [];
        private Loc[] locs = [];

        [XmlElement("label", typeof(Label))]
        public Label[] Labels
        {
            get
            {
                return this.labels;
            }
            set
            {
                this.labels = value;
            }
        }

        [XmlElement("labelArc", typeof(Arc))]
        public Arc[] Arcs
        {
            get
            {
                return this.arcs;
            }
            set
            {
                this.arcs = value;
            }
        }

        [XmlElement("loc", typeof(Loc))]
        public Loc[] Locs
        {
            get
            {
                return this.locs;
            }
            set
            {
                this.locs = value;
            }
        }


        [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
        public string role
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


        [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
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
