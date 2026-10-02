using Json.More;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Public.Analysis.Console.Visuals.Models;
using Public.Analysis.Console.FasbTaxonomies.Models.StatementTaxonomyModels;
using Public.Analysis.Data;
using Public.Analysis.Edgar;
using Public.Analysis.Edgar.RawFacts;
using Public.Frameworks.JsonQuery;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json.Nodes;
using Public.Analysis.Console.DataSets.Edgar;

namespace Public.Analysis.Console.FasbTaxonomies
{
    [ApiController]
    [Route("statement-tree")]
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
        [HttpGet("{statementTreeType}/{statementName}/{entity}")]
        public async Task<IActionResult> GetStatementTree(string statementTreeType, string statementName, string entity)
        {
            if (!options.StatementTreeFilePathsByStatementName.TryGetValue(statementName, out string[]? statementTreeFiles)) {
                return NotFound(statementName);
            }
            var tickerCik = await tickerData.LookupTicker(entity);
            if (tickerCik is null)
            {
                return NotFound(entity);
            }

            IEnumerable<JsonNode> rawFacts = await this.factsData.GetRawFacts(tickerCik, [new JsonQueryPath("facts"), new JsonQueryPath("us-gaap"), new JsonQueryWildCardPathExpression()]);
            var factNames = rawFacts.ToDictionary(f => f.GetPropertyName());

            var coverageTasks =
                statementTreeFiles
                .Select( async statementTreeFileJson =>
                {
                    string fileName = Path.GetFileName(statementTreeFileJson);

                    var statementJsonNode = (await jsonQuery.Query(statementTreeFileJson, [])).Single();
                    var statementModel = System.Text.Json.JsonSerializer.Deserialize<StatementTaxonomyModel>(statementJsonNode);

                    var treeNodesForTicker = statementModel!.Tree.Where(kvp =>
                    {
                        return factNames.ContainsKey(kvp.Value.StatementTaxonomyFactInfo.Name);
                    }).ToDictionary();

                    return new 
                    {
                        StatementTreeFileName = fileName,
                        TotalFactsCount = statementModel.Tree.Count,
                        TreeNodesForTicker = treeNodesForTicker,
                        StatementModel = statementModel
                    };

                });

            var coverage = await Task.WhenAll(coverageTasks);

            var bestTree = coverage.MaxBy(a => a.TreeNodesForTicker.Count);

            if(bestTree is null)
            {
                return NotFound($"No matching facts found for ticker {entity} in statement {statementName}");
            }

            StatementTreeResult finalTree = BuildFinalTree(bestTree.TreeNodesForTicker, bestTree.StatementModel, bestTree.StatementTreeFileName, bestTree.TreeNodesForTicker.Count);

            return Ok(finalTree);
        }

        private StatementTreeResult BuildFinalTree(Dictionary<string, FactNodeModel> treeNodesForTicker, StatementTaxonomyModel statementModel, string statementFileName, int totalFactsCount)
        {
            var toProcess = treeNodesForTicker.Values.Select(n =>
            {

                n.Children = n.Children.Where(c => treeNodesForTicker.ContainsKey(c.ChildElementId)).ToList();
                return n;
            });

            var queue = new Queue<FactNodeModel>(toProcess);
            Dictionary<string, FactNodeModel> tree = new Dictionary<string, FactNodeModel>();
            Dictionary<string, Dictionary<string, List<FactNodeChild>>> children = new Dictionary<string, Dictionary<string, List<FactNodeChild>>>();
            var queued = new HashSet<string>(queue.Select(n => n.ElementId));

            while (queue.Any())
            {
                var node = queue.Dequeue();

                tree.Add(node.ElementId, node);

                foreach (var parentId in node.ParentsElementIds)
                {
                    FactNodeModel parentNode = statementModel.Tree[parentId];

                    if (!parentNode.ChildrenByElementId.ContainsKey(node.ElementId))
                    {
                        throw new InvalidOperationException($"Expected child node for element ID {node.ElementId} under parent {parentNode.ElementId}");
                    }

                    children.TryAdd(parentNode.ElementId, new Dictionary<string, List<FactNodeChild>>());

                    var childsWithDiffLabelAndOrder = parentNode.ChildrenByElementId[node.ElementId]; //Its valid for a parent to have multiple children with the same elementId but different label and order.

                    children[parentNode.ElementId].TryAdd(node.ElementId, childsWithDiffLabelAndOrder);

                    if (queued.Contains(parentNode.ElementId))
                    {
                        continue;
                    }
                    queued.Add(parentNode.ElementId);
                    queue.Enqueue(parentNode);
                }
            }
            var finalTree = new Dictionary<string, FactNodeVisualsModel>();
            foreach (KeyValuePair<string, FactNodeModel> node in tree)
            {

                // TODO: retrieve visuals by ticker, datasetName, and datapointName. For now, we will use placeholder values.
                Visual[] visuals = [];
                if (treeNodesForTicker.ContainsKey(node.Key))
                {
                    visuals = [new Visual(VisualType.TimeSeries, CompanyFacts.DataSetName, node.Value.StatementTaxonomyFactInfo.Name)];
                }
                finalTree.Add(node.Key, new FactNodeVisualsModel { FactNode = node.Value, Visuals = visuals });
                if(!children.TryGetValue(node.Key, out var childElements))
                {
                    continue;
                }
                node.Value.Children = childElements.SelectMany(c => c.Value).ToList();
            }

            return new StatementTreeResult
            {
                File = statementFileName,
                TotalTreeFactsCount = statementModel.Tree.Count,
                TotalTickerFactsCount = totalFactsCount,
                MatchingFactsCount = treeNodesForTicker.Count,
                Coverage = (decimal)treeNodesForTicker.Count / statementModel.Tree.Count * 100.0M,
                Tree = finalTree,
                Description = statementModel.Description
            };
        }
    }

    public class StatementTreeResult
    {
        public string File { get; set; } = string.Empty;
        public int TotalTreeFactsCount { get; set; }
        public int TotalTickerFactsCount { get; set; }
        public int MatchingFactsCount { get; set; }
        public decimal Coverage { get; set; }
        public Dictionary<string, FactNodeVisualsModel> Tree { get; set; } = new Dictionary<string, FactNodeVisualsModel>();
        public string Description { get; set; } = string.Empty;
    }


}
