
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels.LabelsElementModels;

[Serializable()]
[XmlType(AnonymousType = true, Namespace = "http://www.xbrl.org/2003/linkbase", TypeName ="linkbase")]
[XmlRoot(Namespace = "http://www.xbrl.org/2003/linkbase", IsNullable = false, ElementName ="linkbase")]
public partial class LabelLinkBase
{

    private linkbaseRoleRef roleRefField;

    private LabelLink labelLinkField;

    
    public linkbaseRoleRef roleRef
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
    public LabelLink LabelLink
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


[Serializable()]
[XmlType(AnonymousType = true, Namespace = "http://www.xbrl.org/2003/linkbase")]
public partial class linkbaseRoleRef
{

    private string roleURIField;

    private string hrefField;

    private string typeField;

    
    [XmlAttribute()]
    public string roleURI
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

    
    [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
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

