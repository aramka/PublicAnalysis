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
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

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

            
            // Bind configuration section to a strongly-typed options model
            var statementOptions = config.GetSection("StatementTreeBuilder").Get<Configuration.StatementTreeBuilderOptions>()
                ?? throw new InvalidOperationException("Missing StatementTreeBuilder configuration section.");

            string usRolesXsdFilePath = statementOptions.UsRolesXsdFilePath;
            string usGaapEltsXsdFilePath = statementOptions.UsGaapEltsXsdFilePath;
            string srtEltsXsdFilePath = statementOptions.SrtEltsXsdFilePath;
            string usGaapLabelsXmlFilePath = statementOptions.UsGaapLabelsXmlFilePath;
            string srtLabelsXmlFilePath = statementOptions.SrtLabelsXmlFilePath;
            string statementDirectory = statementOptions.StatementFilesDirectoryPath;
            string statementJsonOutputDirectory = statementOptions.StatementJsonOutputDirectory;

            if (!Directory.Exists(statementJsonOutputDirectory))
            {
                Directory.CreateDirectory(statementJsonOutputDirectory);
            }

            string[] statementSearchPatterns = statementOptions.StatementFileSearchPatterns ?? Array.Empty<string>();

            var statementsFilePaths = statementSearchPatterns.SelectMany(sp => Directory.GetFiles(statementDirectory, sp));

            foreach (string statementXmlFilePath in statementsFilePaths)
            {
                StatementModel statementModel = statementBuilder.BuildStatement(usRolesXsdFilePath, usGaapEltsXsdFilePath, srtEltsXsdFilePath, statementXmlFilePath, usGaapLabelsXmlFilePath, srtLabelsXmlFilePath);

                string jsonFilePath = Path.Combine(statementJsonOutputDirectory, $"{Path.GetFileNameWithoutExtension(statementXmlFilePath)}.json");
                if (File.Exists(jsonFilePath))
                {
                    File.Delete(jsonFilePath);
                }
                using var fileStreamWriter = File.OpenWrite(jsonFilePath);

                JsonSerializer.Serialize<StatementModel>(fileStreamWriter, statementModel);
            }
        }
    }
}
