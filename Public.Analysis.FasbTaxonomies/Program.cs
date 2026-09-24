using System.Xml;
using System.Xml.Schema;
using System.Linq;
using System.Xml.Linq;
using System.Reflection.Metadata.Ecma335;
using Public.Analysis.FasbTaxonomies.Statement;
using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;

namespace Public.Analysis.FasbTaxonomies
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LabelsBuilder labelsBuilder = new LabelsBuilder();
            NodesBuilder nodesBuilder = new NodesBuilder();
            XElementParsingUtility xElementParsing = new XElementParsingUtility();

            StatementBuilder statementBuilder = new StatementBuilder(labelsBuilder, nodesBuilder, xElementParsing);


            string usRolesXsdFilePath = @"..\..\..\..\..\fasb_taxonomies\us-gaap-2026\elts\us-roles-2026.xsd";
            string usGaapEltsXsdFilePath = @"..\..\..\..\..\fasb_taxonomies\us-gaap-2026\elts\us-gaap-2026.xsd";
            string srtEltsXsdFilePath = @"..\..\..\..\..\fasb_taxonomies\us-gaap-2026\elts\srt-2026.xsd";
            string usGaapLabelsXmlFilePath = @"..\..\..\..\..\fasb_taxonomies\us-gaap-2026\elts\us-gaap-lab-2026.xml";
            string srtLabelsXmlFilePath = @"..\..\..\..\..\fasb_taxonomies\us-gaap-2026\elts\srt-lab-2026.xml";

            string statementDirectory = @"..\..\..\..\..\fasb_taxonomies\us-gaap-2026\stm\";
            string[] statementSearchPatterns = ["*us-gaap-stm-soi*pre*", "*us-gaap-stm-sfp*pre", "*us-gaap-stm-scf*pre"];

            var statementsFilePaths = statementSearchPatterns.SelectMany(sp => Directory.GetFiles(statementDirectory, sp));

            foreach (string statementXmlFilePath in statementsFilePaths)
            {
                StatementModel statementModel = statementBuilder.BuildStatement(usRolesXsdFilePath, usGaapEltsXsdFilePath, srtEltsXsdFilePath, statementXmlFilePath, usGaapLabelsXmlFilePath, srtLabelsXmlFilePath);
            }
        }
    }
}
