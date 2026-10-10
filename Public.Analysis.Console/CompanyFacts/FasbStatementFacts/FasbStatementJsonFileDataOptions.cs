using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.CompanyFacts.FasbStatementFacts
{
    public class FasbStatementJsonFileDataOptions
    {
        public static readonly string ConfigSectionName = typeof(FasbStatementJsonFileDataOptions).FullName!;
        public Dictionary<string, string[]> StatementTreeFilePathsByStatementName { get; set; } = new Dictionary<string, string[]>();
    }
}
