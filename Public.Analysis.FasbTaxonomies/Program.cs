using Microsoft.Extensions.Configuration;
using Public.Analysis.FasbTaxonomies.Configuration;
using Public.Analysis.FasbTaxonomies.Statement;
using Public.Analysis.FasbTaxonomies.Statement.StatementTree;
using Public.Analysis.FasbTaxonomies.Statement.StatementTree.Labels;
using Public.Analysis.FasbTaxonomies.XmlParsing;
using Public.Analysis.FasbTaxonomies.XmlParsing.DeserializableElementsModels;
using Public.Analysis.FasbTaxonomies.XmlParsing.XmlLinq;
using Public.Analysis.FasbTaxonomies.XmlParsing.XrblXElementParsing;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Public.Analysis.FasbTaxonomies
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Load configuration from appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Bind configuration section to a strongly-typed options model
            var statementOptions = config.GetSection("StatementTreeBuilder").Get<Configuration.StatementTreeBuilderOptions>()
                ?? throw new InvalidOperationException("Missing StatementTreeBuilder configuration section.");
            StatementsToJsonFiles(statementOptions);

        }

        private static void GetLabelRoles(StatementTreeBuilderOptions statementOptions)
        {
            string[] labelsFilePaths = [statementOptions.UsGaapLabelsXmlFilePath, statementOptions.SrtLabelsXmlFilePath];
            string linkBaseNs = "http://www.xbrl.org/2003/linkbase";
            string xlinkNs = "http://www.w3.org/1999/xlink";

            var labelRoles =
                labelsFilePaths.Select(p => XDocument.Load(p))
                .SelectMany(doc =>
                    doc.Root!.Element(XName.Get("labelLink", linkBaseNs))!
                    .Elements(XName.Get("label", linkBaseNs))
                )
                .Select(e => e.Attribute(XName.Get("role", xlinkNs))!.Value)
                .Select(role => (new Uri(role).Segments.Last()))
                .ToHashSet();

            Console.WriteLine($"[{string.Join(",", labelRoles.Select(lr => $"\"{lr}\""))}]");
        }

        private static T DeserializeXmlFile<T>(string fileName) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(fileName))
            {
                return (T)serializer.Deserialize(reader)!;
            }
        }

        private static void StatementsToJsonFiles(StatementTreeBuilderOptions statementOptions)
        {
            LabelsBuilder labelsBuilder = new LabelsBuilder();
            NodesBuilder nodesBuilder = new NodesBuilder();
            XElementParsingUtility xElementParsing = new XElementParsingUtility();

            StatementBuilder statementBuilder = new StatementBuilder(labelsBuilder, nodesBuilder, xElementParsing);


            string usRolesXsdFilePath = statementOptions.UsRolesXsdFilePath;

            string[] eltsFiles = [statementOptions.UsGaapEltsXsdFilePath, statementOptions.SrtEltsXsdFilePath];
            Dictionary<ElementId, IXsElement> elementsByElementId =
                eltsFiles.Select(filePath =>
                {
                    using var stream = new StreamReader(filePath);

                    var parser = new ElementsXDocParser(stream, xElementParsing);
                    return parser.GetElements().Cast<IXsElement>();
                })
                .SelectMany(xsElements => xsElements)
                .ToDictionary(xsElement => xsElement.ElementId);

            string usGaapLabelsXmlFilePath = statementOptions.UsGaapLabelsXmlFilePath;
            string srtLabelsXmlFilePath = statementOptions.SrtLabelsXmlFilePath;

            string[] labelsFiles = [usGaapLabelsXmlFilePath, srtLabelsXmlFilePath];

            var labelsByElementId =
                labelsFiles.Select(filePath => {

                    var labelLink = DeserializeXmlFile<LabelLinkBase>(filePath);
                    var labels = labelsBuilder.BuildLabels(labelLink.LabelLink!);
                    return labels;
                
                }).SelectMany(kvp=>kvp)
                .ToDictionary();

            using var usRolesXsdFileStream = new StreamReader(usRolesXsdFilePath);
            var roleTypesParser = new RoleTypesXDocParser(usRolesXsdFileStream, xElementParsing);

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
                StatementModel statementModel = statementBuilder.BuildStatement(statementXmlFilePath, elementsByElementId, labelsByElementId, roleTypesParser);

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
