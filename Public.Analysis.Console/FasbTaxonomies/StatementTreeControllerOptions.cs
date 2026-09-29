using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.FasbTaxonomies
{
    public class StatementTreeControllerOptions
    {
        public Dictionary<string, string[]> StatementTreeFilePathsByStatementName { get; set; } = new Dictionary<string, string[]>();
    }
}
