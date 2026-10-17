namespace Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing
{
    public interface IRoleTypesParser
    {
        IEnumerable<RoleTypeXElementParser> GetUSRoleTypes();
    }
}