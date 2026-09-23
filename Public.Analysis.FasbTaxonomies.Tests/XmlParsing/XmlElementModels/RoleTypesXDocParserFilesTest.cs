using AwesomeAssertions;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Public.Analysis.FasbTaxonomies.Tests.XmlParsing.XmlElementModels
{
    [TestClass]
    public class USGaapRolesFilesTest
    {

        [TestMethod]
        [DataRow(@"..\..\..\..\..\fasb_taxonomies\us-gaap-2026\elts\us-roles-2026.xsd", DisplayName = "us-roles-2026.xsd")]
        public void ReadAllRoleTypesFromFile(string rolesFilePath)
        {
            using (StreamReader reader = new StreamReader(rolesFilePath))
            {
                XDocument xDoc = XDocument.Load(reader);

                var roles = new RoleTypesXDocParser(xDoc, new XElementParsingUtility());

                var allRoles = roles.GetUSRoleTypes().ToList();

                allRoles.Should().NotBeEmpty();

                var ids = allRoles
                    .Select(r => r.LinkRoleTypeId)
                    .Where(id=>!string.IsNullOrWhiteSpace(id))
                    .ToList();
                ids.Should().NotBeEmpty();

                var definitions = allRoles
                    .Select(r => r.LinkRoleTypeLinkDefinitionValue)
                    .Where(d => !string.IsNullOrWhiteSpace(d))
                    .ToList();
                definitions.Should().NotBeEmpty();
            }
        }
    }
}
