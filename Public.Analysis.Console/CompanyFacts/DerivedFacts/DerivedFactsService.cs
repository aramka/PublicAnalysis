using Public.Analysis.Console.CompanyFacts.FasbStatementFacts.Models;
using Public.Analysis.Console.CompanyFacts.Models;
using Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels;
using Public.Analysis.Console.Services.Models;
using Public.Analysis.Edgar.TickerToCIK;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.CompanyFacts.DerivedFacts
{
    public class DerivedFactsService : IDerivedFactsService
    {
        private readonly IReadOnlyDictionary<string, IDerivedFact> derivedFacts = new Dictionary<string, IDerivedFact>();

        public DerivedFactsService(IEnumerable<IDerivedFact> derivedFacts)
        {
            this.derivedFacts = derivedFacts.ToDictionary(df=>df.FactName);
        }

        public async Task<IEnumerable<CompanyFactModel>> GetCompanyFacts(TickerToCIKModel tickerToCikModel, string factName, SecForm secForm, Range<long> dateRange)
        {
            if(!derivedFacts.TryGetValue(factName, out IDerivedFact? derivedFact))
            {
                return Enumerable.Empty<CompanyFactModel>();
            }
            IEnumerable<CompanyFactModel> facts = await derivedFact.GetCompanyFacts(tickerToCikModel, factName, secForm, dateRange);
            return facts;
        }

        public async Task<IEnumerable<IFactNodeVisualsModel>> GetDerivedFacts(Dictionary<string, FactNodeModel> tree)
        {
            List<IFactNodeVisualsModel> facts = new List<IFactNodeVisualsModel>();
            foreach (IDerivedFact derivedFact in this.derivedFacts.Values)
            {
                bool canDerive = await derivedFact.CanDerive(tree);

                if (!canDerive)
                {
                    continue;
                }

                IFactNodeVisualsModel fact = derivedFact.GetDerivedFact(tree);

                facts.Add(fact);
            }
            return facts;

        }

        public bool IsDerivedFact(string factName)
        {
            return this.derivedFacts.TryGetValue(factName, out IDerivedFact? value);
        }
    }
}
