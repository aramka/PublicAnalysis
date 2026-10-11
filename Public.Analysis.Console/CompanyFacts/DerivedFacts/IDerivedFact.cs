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
    public interface IDerivedFact
    {
        string FactName { get; }

        Task<bool> CanDerive(IReadOnlyDictionary<string, FactNodeModel> tree);
        Task<IEnumerable<CompanyFactModel>> GetCompanyFacts(TickerToCIKModel tickerToCikModel, string factName, SecForm secForm, Range<long> dateRange);
        IFactNodeVisualsModel GetDerivedFact(IReadOnlyDictionary<string, FactNodeModel> tree);
    }
}
