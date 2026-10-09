using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.CompanyFacts.FasbStatementFacts
{
    public class StatementServiceOptions
    {
        public static readonly string ConfigSectionName = typeof(StatementServiceOptions).FullName!;
        public Dictionary<string, string[]> StatementTreeFilePathsByStatementName { get; set; } = new Dictionary<string, string[]>();
    }
}
