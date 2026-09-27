using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
using Public.Analysis.FasbTaxonomies.Statement.StatementTree.Labels;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels.PresentationElementModels;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies.Statement
{
    public class StatementBuilder
    {
        private readonly ILabelsBuilder labelsBuilder;
        private readonly INodesBuilder nodesBuilder;
        private readonly IXElementParsingUtility xElementParsing;

        public StatementBuilder(ILabelsBuilder labelsBuilder, INodesBuilder nodesBuilder, IXElementParsingUtility xElementParsing)
        {
            this.labelsBuilder = labelsBuilder;
            this.nodesBuilder = nodesBuilder;
            this.xElementParsing = xElementParsing;
        }

        private T DeserializeXmlFile<T>(string fileName) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(fileName))
            {
                return (T)serializer.Deserialize(reader)!;
            }
        }

        public StatementModel BuildStatement(string statementFilePath, IReadOnlyDictionary<ElementId, IXsElement> elementsByElementId, IReadOnlyDictionary<ElementId, IReadOnlyDictionary<LabelRole, string>> labelsByElementId, IRoleTypesParser roleTypesParser)
        {
            var statementLinkBase = DeserializeXmlFile<StatementLinkBase>(statementFilePath);
            var nodes = this.nodesBuilder.BuildNodes(statementLinkBase.presentationLink, elementsByElementId, labelsByElementId);
            if (statementLinkBase.RoleRef is null)
            {
                throw new InvalidOperationException($"{nameof(statementLinkBase.RoleRef)} is null");
            }
            var statementInfo = roleTypesParser.GetUSRoleTypes().SingleOrDefault(r => r.LinkRoleTypeId == statementLinkBase.RoleRef.LocationAndAnchor.Anchor);
            if (statementInfo is null)
            {
                throw new InvalidOperationException($"Statement roleType id {statementLinkBase.RoleRef.LocationAndAnchor.Anchor} not found in {nameof(roleTypesParser)}.");
            }

            string statementName = new Uri(statementLinkBase.RoleRef.RoleUri).Segments.Last();

            return new StatementModel(statementName, statementInfo.LinkRoleTypeLinkDefinitionValue, statementInfo.LinkRoleTypeId, nodes);
        }

    }
}
