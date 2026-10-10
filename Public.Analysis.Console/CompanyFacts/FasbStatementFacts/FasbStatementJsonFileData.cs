using Microsoft.Extensions.Options;
using Public.Analysis.Console.CompanyFacts.FasbStatementFacts.Models;
using Public.Frameworks.JsonQuery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.CompanyFacts.FasbStatementFacts
{
    public class FasbStatementJsonFileData : IFasbStatementsData
    {
        private readonly IJsonQuery jsonQuery;
        private readonly FasbStatementJsonFileDataOptions options;

        public FasbStatementJsonFileData(IJsonQuery jsonQuery, IOptions<FasbStatementJsonFileDataOptions> options)
        {
            this.jsonQuery = jsonQuery;
            this.options = options.Value;
        }
        public async Task<IEnumerable<StatementTaxonomyModel>> GetStatementTrees(string statementName)
        {
            if (!options.StatementTreeFilePathsByStatementName.TryGetValue(statementName, out string[]? statementTreeFiles))
            {
                return Enumerable.Empty<StatementTaxonomyModel>();
            }
            var statementModels = await Task.WhenAll(statementTreeFiles
                .Select(async statementTreeFileJson =>
                {
                    string fileName = Path.GetFileName(statementTreeFileJson);

                    var statementJsonNode = (await jsonQuery.Query(statementTreeFileJson, [])).Single();
                    var statementModel = System.Text.Json.JsonSerializer.Deserialize<StatementTaxonomyModel>(statementJsonNode);
                    return statementModel!;
                }));
            return statementModels ?? Enumerable.Empty<StatementTaxonomyModel>();
        }
    }
}
