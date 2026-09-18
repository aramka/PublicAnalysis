using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels.CnReferenceElementModels;

[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.xbrl.org/2003/linkbase", TypeName = "linkbase")]
[XmlRoot(Namespace = "http://www.xbrl.org/2003/linkbase", IsNullable = false, ElementName = "linkbase")]
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
public partial class ReferenceLinkBase
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
{

    private RoleRef[] roleRefField = [];

    private ReferenceLink referenceLinkField = new ReferenceLink();

    [XmlElement("roleRef")]
    public RoleRef[] RoleRef
    {
        get { return this.roleRefField; }
        set { this.roleRefField = value; }
    }

    [XmlElement("referenceLink")]
    public ReferenceLink referenceLink
    {
        get { return this.referenceLinkField; }
        set { this.referenceLinkField = value; }
    }
}


[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.xbrl.org/2003/linkbase", TypeName = "referenceLink")]
public partial class ReferenceLink
{
    private Loc[] locField = Array.Empty<Loc>();
    private Reference[] referenceField = Array.Empty<Reference>();
    private Arc[] referenceArcField = Array.Empty<Arc>();
    private string roleField = string.Empty;
    private string typeField = string.Empty;

    [XmlElement("loc")]
    public Loc[] Locs
    {
        get { return this.locField; }
        set { this.locField = value; }
    }

    [XmlElement("reference")]
    public Reference[] References
    {
        get { return this.referenceField; }
        set { this.referenceField = value; }
    }

    [XmlElement("referenceArc")]
    public Arc[] ReferenceArcs
    {
        get { return this.referenceArcField; }
        set { this.referenceArcField = value; }
    }

    [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink", AttributeName = "role")]
    public string Role
    {
        get { return this.roleField; }
        set { this.roleField = value; }
    }

    [XmlAttribute(Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink", AttributeName = "type")]
    public string Type
    {
        get { return this.typeField; }
        set { this.typeField = value; }
    }
}


[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.xbrl.org/2003/linkbase", TypeName = "reference")]
public partial class Reference
{
    private string labelField = string.Empty;
    private string roleField = string.Empty;
    private string typeField = string.Empty;
    private XmlElement[] anyField = Array.Empty<XmlElement>();

    [XmlAttribute("label", Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
    public string Label
    {
        get { return this.labelField; }
        set { this.labelField = value; }
    }

    [XmlAttribute("role", Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
    public string Role
    {
        get { return this.roleField; }
        set { this.roleField = value; }
    }

    [XmlAttribute("type", Form = XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/1999/xlink")]
    public string Type
    {
        get { return this.typeField; }
        set { this.typeField = value; }
    }

    [XmlAnyElement(Namespace ="cn-part")]
    public XmlElement[] CnParts
    {
        get { return this.anyField; }
        set { this.anyField = value; }
    }
}

