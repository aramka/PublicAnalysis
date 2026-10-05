using Microsoft.AspNetCore.Authentication;
using Public.Analysis.Console.CompanyFacts.Models;
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

        public CompanyFactsService(IFactsData factsData, ITickerToCIKData tickerToCIKData)
        {
            this.factsData = factsData;
            this.tickerToCIKData = tickerToCIKData;
        }
        public Task<ServiceResponse<IEnumerable<TimeSeriesDataPoint>>> GetCompanyFactTimeSeries(string ticker, string factName)
        {
            return this.GetCompanyFactTimeSeries(ticker, factName, SecForm.TenQ, DateRange.AllDates);
        }

        public async Task<ServiceResponse<IEnumerable<TimeSeriesDataPoint>>> GetCompanyFactTimeSeries(string ticker, string factName, SecForm secForm, Range<long> dateRange)
        {
            TickerToCIKModel? tickerToCikModel = await this.tickerToCIKData.LookupTicker(ticker);

            if(tickerToCikModel is null)
            {
                return new ServiceResponse<IEnumerable<TimeSeriesDataPoint>>(Enumerable.Empty<TimeSeriesDataPoint>(), [$"Ticker {ticker} was not found."]);
            }

            IEnumerable<CompanyFactModel> facts = await this.factsData.GetCompanyFacts(tickerToCikModel, factName, secForm, dateRange);

            return new ServiceResponse<IEnumerable<TimeSeriesDataPoint>>(facts.Select(fact => new TimeSeriesDataPoint
            {
                TimeStamp = fact.EndDateUnixSeconds,
                Value = fact.Value.HasValue ? fact.Value.Value : 0
            }), []);
        }
        
    }
}
