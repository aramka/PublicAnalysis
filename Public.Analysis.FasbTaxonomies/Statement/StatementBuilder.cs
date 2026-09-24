using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
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
        public StatementModel BuildStatement(string usRolesXsdFilePath, string usGaapEltsXsdFilePath, string srtEltsXsdFilePath, string statementXmlFilePath, string usGaapLabelsXmlFilePath,string srtLabelsXmlFilePath)
        {
            using var usRolesXsdFileStream = new StreamReader(usRolesXsdFilePath);
            var roleTypesParser = new RoleTypesXDocParser(usRolesXsdFileStream, this.xElementParsing);
            
            using var usGaapEltsXsdFileStream = new StreamReader(usGaapEltsXsdFilePath);
            var usGaapEltsParser = new ElementsXDocParser(usGaapEltsXsdFileStream, this.xElementParsing);

            using var srtEltsXsdFileStream = new StreamReader(srtEltsXsdFilePath);
            var srtEltsParser = new ElementsXDocParser(srtEltsXsdFileStream, this.xElementParsing);

            var statementLinkBase = DeserializeXmlFile<StatementLinkBase>(statementXmlFilePath);
            var usGaapLabelsLinkBase = DeserializeXmlFile<LabelLinkBase>(usGaapLabelsXmlFilePath);
            var srtLabelsLinkBase = DeserializeXmlFile<LabelLinkBase>(srtLabelsXmlFilePath);

            return this.BuildStatement(roleTypesParser, usGaapEltsParser, srtEltsParser, statementLinkBase, usGaapLabelsLinkBase.LabelLink!, srtLabelsLinkBase.LabelLink!);
        }

        private T DeserializeXmlFile<T>(string fileName) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(fileName))
            {
                return (T)serializer.Deserialize(reader)!;
            }
        }

        public StatementModel BuildStatement(IRoleTypesParser roleTypesParser, IElementsParser usGaapElementsParser, IElementsParser srtElementsParser, StatementLinkBase statementLinkBase, LabelLink usGaapLabelLink, LabelLink srtLabelLink)
        {
            var labels = labelsBuilder.BuildLabels(usGaapLabelLink);
            var srtLabels = labelsBuilder.BuildLabels(srtLabelLink);
            labels = labels.Concat(srtLabels).ToDictionary();
            
            var fasbElements = usGaapElementsParser.GetElements().Cast<IXsElement>();
            var srtElements = srtElementsParser.GetElements().Cast<IXsElement>();
            var elementsByElementId = fasbElements.Concat(srtElements).ToDictionary(a => a.ElementId);

            var nodes = this.nodesBuilder.BuildNodes(statementLinkBase.presentationLink, elementsByElementId, labels);
            if(statementLinkBase.RoleRef is null)
            {
                throw new InvalidOperationException($"{nameof(statementLinkBase.RoleRef)} is null");
            }
            var statementInfo = roleTypesParser.GetUSRoleTypes().SingleOrDefault(r => r.LinkRoleTypeId == statementLinkBase.RoleRef.LocationAndAnchor.Anchor);
            if(statementInfo is null)
            {
                throw new InvalidOperationException($"Statement roleType id {statementLinkBase.RoleRef.LocationAndAnchor.Anchor} not found in {nameof(roleTypesParser)}.");
            }

            string statementName = new Uri(statementLinkBase.RoleRef.RoleUri).Segments.Last();

            return new StatementModel(statementName, statementInfo.LinkRoleTypeLinkDefinitionValue, statementInfo.LinkRoleTypeId, nodes);
        }
    }
}
