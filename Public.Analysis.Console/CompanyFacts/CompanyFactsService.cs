using Public.Analysis.Console.CompanyFacts.FasbStatementFacts.Models;
using Public.Analysis.Console.CompanyFacts.Models;
using Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels;
using Public.Analysis.Console.Services.Models;
using Public.Analysis.Console.Visuals.Models;
using Public.Analysis.Edgar;
using Public.Analysis.Edgar.TickerToCIK;
using DateRange = Public.Analysis.Console.Services.Models.Range<long>;

namespace Public.Analysis.Console.CompanyFacts
{
    public class CompanyFactsService : ICompanyFactsService
    {
        private readonly IFactsData factsData;
        private readonly ITickerToCIKData tickerToCIKData;
        private readonly IStatementFactsService statementService;

        public CompanyFactsService(IFactsData factsData, ITickerToCIKData tickerToCIKData, IStatementFactsService statementService)
        {
            this.factsData = factsData;
            this.tickerToCIKData = tickerToCIKData;
            this.statementService = statementService;
        }
        public Task<ServiceResponse<IEnumerable<TimeSeriesDataPoint>>> GetCompanyFactTimeSeries(string ticker, string factName)
        {
            return this.GetCompanyFactTimeSeries(ticker, factName, SecForm.TenQ, DateRange.AllDates);
        }

        public async Task<ServiceResponse<IEnumerable<TimeSeriesDataPoint>>> GetCompanyFactTimeSeries(string ticker, string factName, SecForm secForm, Range<long> dateRange)
        {
        // Next up is computed facts, facts of facts. facts derived from other facts. For example Tangible book value. Also think about existing facts, summary/total facts and have a way to get the consituents. For example, current assets total consists of AR and cash, and inventory.
            TickerToCIKModel? tickerToCikModel = await this.tickerToCIKData.LookupTicker(ticker);

            if(tickerToCikModel is null)
            {
                return new ServiceResponse<IEnumerable<TimeSeriesDataPoint>>(Enumerable.Empty<TimeSeriesDataPoint>(), [$"Ticker {ticker} was not found."]);
            }

            IEnumerable<CompanyFactModel> facts = await this.factsData.GetCompanyFacts(tickerToCikModel, factName, secForm, dateRange);
            /* TODO: For display in the UI, we need to include human readable label for the factName. Also, for troubleshooting we need to include the meta data about the datapoint so that any particular datapoint can be found with a single click, ideally, or at least */
            return new ServiceResponse<IEnumerable<TimeSeriesDataPoint>>(facts.Select(fact => new TimeSeriesDataPoint
            {
                TimeStamp = fact.EndDateUnixSeconds,
                Value = fact.Value.HasValue ? fact.Value.Value : 0
            }), []);
        }
        public async Task<ServiceResponse<IStatementTreeResult>> GetCompanyFactAsTree(string ticker, string statementName)
        {
            // Minimal implementation to satisfy interface. Return an empty StatementTaxonomyModel which implements the interface.
            var response = await this.statementService.GetStatementTree(statementName, ticker);
            return response;
        }
        
    }
}
