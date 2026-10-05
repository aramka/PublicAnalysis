using Public.Analysis.Console.CompanyFacts.Models;
using Public.Analysis.Console.Services.Models;
using Public.Analysis.Edgar.TickerToCIK;

namespace Public.Analysis.Console.CompanyFacts
{
    public interface IFactsData
    {
        Task<IEnumerable<CompanyFactModel>> GetCompanyFacts(TickerToCIKModel tickerToCikModel, string factName, SecForm secForm, Range<long> dateRange);
    }
}