using Json.More;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Public.Analysis.Console.FasbTaxonomies.Models.StatementTaxonomyModels;
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

            IEnumerable<JsonNode> facts = await this.factsData.GetRawFacts(tickerCik, [new JsonQueryPath("facts"), new JsonQueryPath("us-gaap"), new JsonQueryWildCardPathExpression()]);
            var tickerFacts = facts.ToDictionary(f => f.GetPropertyName());
            var coverageTasks =
                statementTreeFiles
                .Select(async statementTreeFileJson =>
                {
                    var statementJsonNode = (await jsonQuery.Query(statementTreeFileJson, [])).Single();

                    var statementModel = System.Text.Json.JsonSerializer.Deserialize<StatementTaxonomyModel>(statementJsonNode);

                    var matchedFacts = statementModel!.Tree.Where(kvp => { 
                        return tickerFacts.ContainsKey(kvp.Value.StatementTaxonomyFactInfo.Name); 
                    }).ToList();
                    string fileName = Path.GetFileName(statementTreeFileJson);

                    Dictionary<string,FactNodeModel> matchedFactsWithParents = matchedFacts.ToDictionary();

                    var parents = matchedFacts
                    .SelectMany(fact => fact.Value.ParentsElementIds)
                    .Select(parentElementId => statementModel.Tree[parentElementId]);
                    var queue = new Queue<FactNodeModel>(parents);

                    while (queue.Any())
                    {
                        var parent = queue.Dequeue();
                        if (matchedFactsWithParents.ContainsKey(parent.ElementId))
                        {
                            continue;
                        }
                        matchedFactsWithParents.Add(parent.ElementId, parent);
                        
                        foreach(var parentId in parent.ParentsElementIds)
                        {
                            var nextParent = statementModel.Tree[parentId];
                            queue.Enqueue(nextParent);
                        }
                    }

                    return new { File = fileName, TotalTreeFactsCount = statementModel.Tree.Count, TotalTickerFactsCount = tickerFacts.Count(),  MatchingFactsCount = matchedFacts.Count, Coverage = (decimal)matchedFacts.Count / statementModel.Tree.Count * 100.0M, Tree = matchedFactsWithParents };

                });

            var coverage = await Task.WhenAll(coverageTasks);

            var bestTree = coverage.MaxBy(a => a.MatchingFactsCount);

            return Ok(bestTree);
        }
    }
}
