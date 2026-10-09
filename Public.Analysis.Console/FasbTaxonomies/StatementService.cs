using Microsoft.Extensions.Options;
using Public.Analysis.Console.CompanyFacts.Tree.Models;
using Public.Analysis.Console.FasbTaxonomies.Models.StatementTaxonomyModels;
using Public.Analysis.Console.FinancialStatements;
using Public.Analysis.Console.Services.Models;
using Public.Analysis.Console.Visuals.Models;
using Public.Analysis.Edgar;
using Public.Analysis.Edgar.RawFacts;
using Public.Frameworks.JsonQuery;
using System.Text.Json.Nodes;

namespace Public.Analysis.Console.FasbTaxonomies
{
    public class StatementService : IStatementService
    {
        private readonly IRawFactsData factsData;
        private readonly ITickerToCIKData tickerData;
        private readonly IJsonQuery jsonQuery;
        private readonly StatementServiceOptions options;

        public StatementService(IRawFactsData factsData, ITickerToCIKData tickerData, IJsonQuery jsonQuery, IOptions<StatementServiceOptions> options)
        {
            this.factsData = factsData;
            this.tickerData = tickerData;
            this.jsonQuery = jsonQuery;
            this.options = options.Value;
        }
        public async Task<ServiceResponse<IStatementTreeResult>> GetStatementTree(string statementName, string entity)
        {
            if (!options.StatementTreeFilePathsByStatementName.TryGetValue(statementName, out string[]? statementTreeFiles)) {
                return ServiceResponse<IStatementTreeResult>.Failure($"No statement tree files found for statement: {statementName}");
            }
            var tickerCik = await tickerData.LookupTicker(entity);
            if (tickerCik is null)
            {
                return ServiceResponse<IStatementTreeResult>.Failure($"Ticker not found: {entity}");
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
                        return factNames.ContainsKey(kvp.Value.XsElement.Name);
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
                return ServiceResponse<IStatementTreeResult>.Failure($"No matching facts found for ticker {entity} in statement {statementName}");
            }

            StatementTreeResult finalTree = BuildFinalTree(bestTree.TreeNodesForTicker, bestTree.StatementModel, bestTree.StatementTreeFileName, bestTree.TreeNodesForTicker.Count);

            return new ServiceResponse<IStatementTreeResult>(finalTree, new string[0]);
        }

        private StatementTreeResult BuildFinalTree(Dictionary<string, FactNodeModel> treeNodesForTicker, StatementTaxonomyModel statementModel, string statementFileName, int totalFactsCount)
        {
            var toProcess = treeNodesForTicker.Values.Select(n =>
            {

                n.Children = n.Children.Where(c => treeNodesForTicker.ContainsKey(c.ChildElementId)).ToList();
                return n;
            });
            (Dictionary<string, FactNodeModel> tree, Dictionary<string, Dictionary<string, List<FactNodeChild>>> children) = BuildFinalTreeNodes(statementModel, toProcess);

            var finalTree = new Dictionary<string, FactNodeVisualsModel>();

            IEnumerable<FactNodeVisualsModel> derivedFacts = this

            foreach (KeyValuePair<string, FactNodeModel> node in tree)
            {

                // TODO: retrieve visuals by ticker, datasetName, and datapointName. For now, we will use placeholder values.
                if (children.TryGetValue(node.Key, out var childElements))
                {
                    node.Value.Children = childElements.SelectMany(c => c.Value).ToList(); ;
                }
                VisualType[] visuals = [];
                if (treeNodesForTicker.ContainsKey(node.Key))
                {
                    visuals = [VisualType.TimeSeries];
                }

                finalTree.Add(node.Key, new FactNodeVisualsModel { FactNode = node.Value, Visuals = visuals });
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

        private (Dictionary<string, FactNodeModel> tree, Dictionary<string, Dictionary<string, List<FactNodeChild>>> children) BuildFinalTreeNodes(StatementTaxonomyModel statementModel, IEnumerable<FactNodeModel> toProcess)
        {
            var queue = new Queue<FactNodeModel>(toProcess);
            var tree = new Dictionary<string, FactNodeModel>();
            var children = new Dictionary<string, Dictionary<string, List<FactNodeChild>>>();
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
            return (tree, children);
        }
    }


}
