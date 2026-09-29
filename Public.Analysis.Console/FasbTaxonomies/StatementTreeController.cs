using Json.More;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Public.Analysis.Data;
using Public.Analysis.Edgar;
using Public.Analysis.Edgar.RawFacts;
using Public.Frameworks.JsonQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;

namespace Public.Analysis.Console.FasbTaxonomies
{
    [ApiController]
    [Route("fasb-taxonomies/statement-tree")]
    public class StatementTreeController: ControllerBase
    {
        private readonly IRawFactsData factsData;
        private readonly ITickerToCIKData tickerData;
        private readonly IJsonQuery jsonQuery;
        private readonly StatementTreeControllerOptions options;

        public StatementTreeController(IRawFactsData factsData, ITickerToCIKData tickerData, IJsonQuery jsonQuery, IOptions<StatementTreeControllerOptions> options)
        {
            this.factsData = factsData;
            this.tickerData = tickerData;
            this.jsonQuery = jsonQuery;
            this.options = options.Value;
        }
        [HttpGet("{ticker}/{statementName}")]
        public async Task<IActionResult> GetStatementTree(string ticker, string statementName)
        {
            if (!options.StatementTreeFilePathsByStatementName.TryGetValue(statementName, out string[]? statementTreeFiles)) {
                return NotFound(statementName);
            }
            var tickerCik = await tickerData.LookupTicker(ticker);
            if (tickerCik is null)
            {
                return NotFound(ticker);
            }

            DataQuery query = new DataQuery(new string[] { ticker, "facts", "us-gaap" });
            IEnumerable<JsonNode> facts = await this.factsData.GetRawFacts(tickerCik, [new JsonQueryPath("facts"), new JsonQueryPath("us-gaap"), new JsonQueryWildCardPathExpression()]);
            var tickerFacts = facts.ToDictionary(f => f.GetPropertyName());
            var coverageTasks =
                statementTreeFiles
                .Select(async f =>
                {
                    var tree = (await jsonQuery.Query(f, [new JsonQueryPath("Tree")])).Single();
                    var facts = jsonQuery.Query(tree, [new JsonQueryWildCardPathExpression(), new JsonQueryPath("XsElement"), new JsonQueryPath("Name")]);
                    var factsInTree = facts.Select(n => n.ToString()).ToList();
                    int factsInTreeCount = factsInTree.Count();
                    var tickerFactsInTree = factsInTree.Where(treeFactName => { 
                        return tickerFacts.ContainsKey(treeFactName); 
                    });
                    string fileName = Path.GetFileName(f);
                    var tickerFactsInTreeCount = tickerFactsInTree.Count();
                    return new { File = fileName, FactsInTreeCount = factsInTree.Count(), TickerFactsCount = tickerFacts.Count(),  TickerFactsInTreeCount = tickerFactsInTreeCount, Coverage = (decimal)tickerFactsInTreeCount / factsInTreeCount * 100.0M, Tree = tree };

                });

            var coverage = await Task.WhenAll(coverageTasks);

            var bestTree = coverage.MaxBy(a => a.TickerFactsInTreeCount)!.Tree;

            return Ok(bestTree);
        }
    }
}
