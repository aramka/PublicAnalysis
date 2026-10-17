using Microsoft.Extensions.Options;
using Public.Analysis.Console.CompanyFacts.FasbStatementFacts.Models;
using Public.Analysis.Console.CompanyFacts.Models;
using Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels;
using Public.Analysis.Console.Services.Models;
using Public.Analysis.Console.Visuals.Models;
using Public.Analysis.Edgar.RawFacts;
using Public.Analysis.Edgar.TickerToCIK;
using Public.Frameworks.JsonQuery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.CompanyFacts.DerivedFacts.TangibleBookValue
{
    public class TangibleBookValue : IDerivedFact
    {
        private readonly TangibleBookValueOptions options;
        private readonly IEnumerable<string> factIds;
        private readonly IFactsData factsData;

        public TangibleBookValue(IFactsData factsData, IOptions<TangibleBookValueOptions> options)
        {
            this.options = options.Value;
            this.factIds = this.options.AssetsFactNames.Keys.Concat(this.options.LiabilitiesFactNames.Keys).Concat(this.options.IntangiblesFactNames.Keys);
            this.factsData = factsData;
        }

        public string FactName => nameof(TangibleBookValue);

        public Task<bool> CanDerive(IReadOnlyDictionary<string, FactNodeModel> tree)
        {
            return Task.FromResult(this.factIds.All(tree.ContainsKey));
        }

        public async Task<IEnumerable<CompanyFactModel>> GetCompanyFacts(TickerToCIKModel tickerToCikModel, string factName, SecForm secForm, Range<long> dateRange)
        {
            // TODO: need a way to check this is in fact correct. Integration tests
            var tasksByCategory = 
                " ".Select(_=>new { Category = nameof(TangibleBookValueOptions.AssetsFactNames), FactNames = this.options.AssetsFactNames.Values })
                .Concat([new { Category = nameof(TangibleBookValueOptions.LiabilitiesFactNames), FactNames = this.options.LiabilitiesFactNames.Values }])
                .Concat([new { Category = nameof(TangibleBookValueOptions.IntangiblesFactNames), FactNames = this.options.IntangiblesFactNames.Values }])
                .ToDictionary(a => a.Category, (a) => {

                    var tasks = a.FactNames.Select(name => this.factsData.GetCompanyFacts(tickerToCikModel, name, secForm, dateRange));
                    return tasks;
                });
            var completedTasks = await Task.WhenAll(tasksByCategory.Values.SelectMany(t => t));

            var assetsPerFiling = tasksByCategory
                .Select(kvp => new { Category = kvp.Key, Facts = kvp.Value.SelectMany(t => t.Result) })
                .ToDictionary(
                    a => a.Category, a =>
                    a.Facts.GroupBy(f => new { f.AccessionNumber, f.StartDate, f.EndDate, f.FiscalYear, f.FiscalPeriod, f.Form, f.FiledDate })
                    .ToDictionary(b => b.Key, b => b.Sum(f => f.Value))
                );
            var facts = assetsPerFiling
                .SelectMany(kvp => kvp.Value.Keys)
                .ToHashSet()
                .Select(key =>
                {
                    assetsPerFiling[nameof(TangibleBookValueOptions.AssetsFactNames)].TryGetValue(key, out decimal? assets);
                    assetsPerFiling[nameof(TangibleBookValueOptions.LiabilitiesFactNames)].TryGetValue(key, out decimal? liabilities);
                    assetsPerFiling[nameof(TangibleBookValueOptions.IntangiblesFactNames)].TryGetValue(key, out decimal? intangibles);

                    return new CompanyFactModel
                    {
                        AccessionNumber = key.AccessionNumber,
                        EndDate = key.EndDate,
                        FiledDate = key.FiledDate,
                        FiscalPeriod = key.FiscalPeriod,
                        FiscalYear = key.FiscalYear,
                        Form = key.Form,
                        StartDate = key.StartDate,
                        Value = assets ?? 0 - liabilities ?? 0 - intangibles ?? 0
                    };

                }).ToList();
            return facts;
        }

        public IFactNodeVisualsModel GetDerivedFact(IReadOnlyDictionary<string, FactNodeModel> tree)
        {
            // TODO: need a way to show time series with all constituents
            return new FactNodeVisualsModel(new DerivedFactNode(this.FactName, "Tangible Book Value", []), [VisualType.TimeSeries]);
        }
    }
}
