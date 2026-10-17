namespace Public.Analysis.FasbTaxonomies.Configuration
{
    public class StatementTreeBuilderOptions
    {
        public string UsRolesXsdFilePath { get; set; } = string.Empty;
        public string UsGaapEltsXsdFilePath { get; set; } = string.Empty;
        public string SrtEltsXsdFilePath { get; set; } = string.Empty;
        public string UsGaapLabelsXmlFilePath { get; set; } = string.Empty;
        public string SrtLabelsXmlFilePath { get; set; } = string.Empty;
        public string StatementFilesDirectoryPath { get; set; } = string.Empty;
        public string[] StatementFileSearchPatterns { get; set; } = [];
        public string StatementJsonOutputDirectory { get; set; } = string.Empty;
    }
}
