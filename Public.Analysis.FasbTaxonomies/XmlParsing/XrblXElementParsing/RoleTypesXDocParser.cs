using Public.Analysis.FasbTaxonomies.XmlParsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing
{
    public class RoleTypesXDocParser : IRoleTypesParser
    {
        private readonly XDocument usRolesXsdDoc;
        private IReadOnlyDictionary<string, XNamespace> namespacesByPrefix;
        private readonly IXElementParsingUtility xElementParsingUtility;

        public RoleTypesXDocParser(XDocument usRolesXsdDoc, IXElementParsingUtility xElementParsingUtility)
        {
            this.usRolesXsdDoc = usRolesXsdDoc;
            this.xElementParsingUtility = xElementParsingUtility;
            this.namespacesByPrefix = this.xElementParsingUtility.RootNameSpacesByPrefix(usRolesXsdDoc);
        }

        public RoleTypesXDocParser(StreamReader usRolesXsdFileStream, IXElementParsingUtility xElementParsingUtility):this(XDocument.Load(usRolesXsdFileStream), xElementParsingUtility)
        {
            
            
        }

        public IEnumerable<RoleTypeXElementParser> GetUSRoleTypes()
        {

            return this.xElementParsingUtility.GetDescendants(this.usRolesXsdDoc, LocalNamesAndPrefixes.RoleTypeElement, LocalNamesAndPrefixes.LinkPrefix, this.namespacesByPrefix)
                .Select(xElement => new RoleTypeXElementParser(xElement, this.namespacesByPrefix, this.xElementParsingUtility))
                .ToList()
                ?? Enumerable.Empty<RoleTypeXElementParser>();
        }
    }
}
