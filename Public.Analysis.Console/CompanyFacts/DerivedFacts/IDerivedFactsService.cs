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
    public interface IDerivedFactsService
    {
        Task<IEnumerable<CompanyFactModel>> GetCompanyFacts(TickerToCIKModel tickerToCikModel, string factName, SecForm secForm, Range<long> dateRange);
        Task<IEnumerable<IFactNodeVisualsModel>> GetDerivedFacts(Dictionary<string, FactNodeModel> tree);
        bool IsDerivedFact(string factName);
    }
}
