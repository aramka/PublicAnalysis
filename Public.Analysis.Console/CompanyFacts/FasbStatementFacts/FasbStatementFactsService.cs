using Microsoft.Extensions.Options;
using Public.Analysis.Console.CompanyFacts.FasbStatementFacts.Models;
using Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels;
using Public.Analysis.Console.Services.Models;
using Public.Analysis.Console.Visuals.Models;
using Public.Analysis.Edgar;
using Public.Analysis.Edgar.RawFacts;
using Public.Frameworks.JsonQuery;
using System.Text.Json.Nodes;

namespace Public.Analysis.Console.CompanyFacts.FasbStatementFacts
{
    public class FasbStatementFactsService : IStatementFactsService
    {
        private readonly IFactsData factsData;
        private readonly ITickerToCIKData tickerData;
        private readonly IFasbStatementsData fasbStatementsData;

        public FasbStatementFactsService(IFactsData factsData, ITickerToCIKData tickerData, IFasbStatementsData fasbStatementsData)
        {
            this.factsData = factsData;
            this.tickerData = tickerData;
            this.fasbStatementsData = fasbStatementsData;
        }
        public async Task<ServiceResponse<IStatementTreeResult>> GetStatementTree(string statementName, string entity)
        {
            var tickerCik = await tickerData.LookupTicker(entity);
            if (tickerCik is null)
            {
                return ServiceResponse<IStatementTreeResult>.Failure($"Ticker not found: {entity}");
            }
            
            Task<HashSet<string>> factNamesForTicker = this.factsData.GetFactNames(tickerCik);

            Task<IEnumerable<StatementTaxonomyModel>> statementTaxonomyModels = this.fasbStatementsData.GetStatementTrees(statementName);
            
            await Task.WhenAll(factNamesForTicker, statementTaxonomyModels);

            HashSet<string> factNames = factNamesForTicker.Result;
            IEnumerable<StatementTaxonomyModel> statementModels = statementTaxonomyModels.Result;

            if(!statementModels.Any())
            {
                return ServiceResponse<IStatementTreeResult>.Failure($"No statement models found for statement: {statementName}");
            }

            var bestTree =
                statementModels
                .Select( statementModel =>
                {
                    var treeNodesForTicker = statementModel.Tree.Where(kvp =>
                    {
                        return factNames.Contains(kvp.Value.XsElement.Name);
                    }).ToDictionary();

                    return new 
                    {
                        TotalFactsCount = statementModel.Tree.Count,
                        TreeNodesForTicker = treeNodesForTicker,
                        StatementModel = statementModel
                    };

                })
                .MaxBy(a=>a.TreeNodesForTicker.Count);

            if(bestTree is null)
            {
                return ServiceResponse<IStatementTreeResult>.Failure($"No matching facts found for ticker {entity} in statement {statementName}");
            }

            StatementTreeResult finalTree = BuildFinalTree(bestTree.TreeNodesForTicker, bestTree.StatementModel, bestTree.TreeNodesForTicker.Count);

            return new ServiceResponse<IStatementTreeResult>(finalTree);
        }

        private StatementTreeResult BuildFinalTree(Dictionary<string, FactNodeModel> treeNodesForTicker, StatementTaxonomyModel statementModel, int totalFactsCount)
        {
            var toProcess = treeNodesForTicker.Values.Select(n =>
            {

                n.Children = n.Children.Where(c => treeNodesForTicker.ContainsKey(c.ChildElementId)).ToList();
                return n;
            });
            (Dictionary<string, FactNodeModel> tree, Dictionary<string, Dictionary<string, List<FactNodeChild>>> children) = BuildFinalTreeNodes(statementModel, toProcess);

            var finalTree = new Dictionary<string, IFactNodeVisualsModel>();

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
