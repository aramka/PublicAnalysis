using Public.Analysis.FasbTaxonomies.XmlParsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementModels
{
    public class USRoleTypes
    {
        private readonly XDocument usRolesXsdDoc;
        private IReadOnlyDictionary<string, XNamespace> namespacesByPrefix;
        private readonly IXElementParsingUtility xElementParsingUtility;

        public USRoleTypes(XDocument usRolesXsdDoc, IXElementParsingUtility xElementParsingUtility)
        {
            this.usRolesXsdDoc = usRolesXsdDoc;
            this.xElementParsingUtility = xElementParsingUtility;
            this.namespacesByPrefix = this.xElementParsingUtility.RootNameSpacesByPrefix(usRolesXsdDoc);
        }

        public IEnumerable<USRoleType> GetUSRoleTypes()
        {

            return this.xElementParsingUtility.GetDescendants(this.usRolesXsdDoc, LocalNamesAndPrefixes.RoleTypeElement, LocalNamesAndPrefixes.LinkPrefix, this.namespacesByPrefix)
                .Select(xElement => new USRoleType(xElement, this.namespacesByPrefix, this.xElementParsingUtility))
                .ToList()
                ?? Enumerable.Empty<USRoleType>();
        }
    }
}
