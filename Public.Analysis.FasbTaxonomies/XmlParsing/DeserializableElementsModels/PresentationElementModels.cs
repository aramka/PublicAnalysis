namespace Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels.PresentationElementModels;

using System.Xml.Serialization;


[Serializable]
[XmlType(AnonymousType = true, Namespace = "http://www.xbrl.org/2003/linkbase", TypeName ="linkbase")]
[XmlRoot(Namespace = "http://www.xbrl.org/2003/linkbase", IsNullable = false, ElementName ="linkbase")]
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
public partial class StatementLinkBase
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
{

    private RoleRef? roleRefField = null;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private PresentationLink presentationLinkField;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    [XmlElement("roleRef")]
    public RoleRef? RoleRef
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

    
    public PresentationLink presentationLink
    {
        get
        {
            return this.presentationLinkField;
        }
        set
        {
            this.presentationLinkField = value;
        }
    }
}

