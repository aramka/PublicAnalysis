using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Linq;
using System.Xml.Linq;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Configuration;
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


            // Load configuration from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            
            var statementTreeBuilderOptions = config.GetSection("StatementTreeBuilder");
            string usRolesXsdFilePath = statementTreeBuilderOptions["UsRolesXsdFilePath"]!;
            string usGaapEltsXsdFilePath = statementTreeBuilderOptions["UsGaapEltsXsdFilePath"]!;
            string srtEltsXsdFilePath = statementTreeBuilderOptions["SrtEltsXsdFilePath"]!;
            string usGaapLabelsXmlFilePath = statementTreeBuilderOptions["UsGaapLabelsXmlFilePath"]!;
            string srtLabelsXmlFilePath = statementTreeBuilderOptions["SrtLabelsXmlFilePath"]!;
            string statementDirectory = statementTreeBuilderOptions["StatementFilesDirectoryPath"]!;

            string[] statementSearchPatterns = statementTreeBuilderOptions.GetSection("SearchPatterns").Get<string[]>() ?? Array.Empty<string>();

            var statementsFilePaths = statementSearchPatterns.SelectMany(sp => Directory.GetFiles(statementDirectory, sp));

            foreach (string statementXmlFilePath in statementsFilePaths)
            {
                StatementModel statementModel = statementBuilder.BuildStatement(usRolesXsdFilePath, usGaapEltsXsdFilePath, srtEltsXsdFilePath, statementXmlFilePath, usGaapLabelsXmlFilePath, srtLabelsXmlFilePath);
            }
        }
    }
}
