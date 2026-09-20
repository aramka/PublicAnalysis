
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;

[Serializable()]
[XmlType(AnonymousType = true, Namespace = "http://www.xbrl.org/2003/linkbase", TypeName ="linkbase")]
[XmlRoot(Namespace = "http://www.xbrl.org/2003/linkbase", IsNullable = false, ElementName ="linkbase")]
public partial class LabelLinkBase
{

    private RoleRef? roleRefField = null;

    private LabelLink? labelLinkField = null;

    [XmlElement("roleRef")]
    public RoleRef? roleRef
    {
        get
        {
            return this.roleRefField;
        }
        set
        {
            this.roleRefField = value;
        }
    }

    [XmlElement("labelLink")]
    public LabelLink? LabelLink
    {
        get
        {
            return this.labelLinkField;
        }
        set
        {
            this.labelLinkField = value;
        }
    }
}

