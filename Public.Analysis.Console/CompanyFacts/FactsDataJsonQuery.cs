using Public.Analysis.Console.CompanyFacts.Models;
using Public.Analysis.Console.Services.Models;
using Public.Analysis.Edgar.RawFacts;
using Public.Analysis.Edgar.TickerToCIK;
using Public.Frameworks.JsonQuery;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Public.Analysis.Console.CompanyFacts
{
    public class FactsDataJsonQuery : IFactsData
    {
        private const long SecondsPerDay = 24*60*60;
        private long SecondsPer90Days => SecondsPerDay*90;

        private readonly IRawFactsData rawFactsData;

        public FactsDataJsonQuery(IRawFactsData rawFactsData)
        {
            this.rawFactsData = rawFactsData;
        }

        public async Task<IEnumerable<CompanyFactModel>> GetCompanyFacts(TickerToCIKModel tickerToCikModel, string factName, SecForm secForm, Range<long> dateRange)
        {
            long lowerThresholdDays = -1;
            long upperThresholdDays = -1;
            if(secForm== SecForm.TenK)
            {
                lowerThresholdDays = 360;
                upperThresholdDays = 370;
            }
            else
            {
                lowerThresholdDays = 85;
                upperThresholdDays = 95;
            }
            //TODO: is the filtering correct? it seems like 10-K data points are returned even though we are filtering for one or the other
            IJsonQueryExpression[] jsonQuery =[new JsonQueryPath("facts"), new JsonQueryPath("us-gaap"), new JsonQueryPath(factName), new JsonQueryPath("units"), new JsonQueryPath("USD"), new JsonQueryFilter("form",JsonQueryFilterOperators.Eq,secForm== SecForm.TenK? "10-K":"10-Q") ];
            IEnumerable<JsonNode> factNodes = await this.rawFactsData.GetRawFacts(tickerToCikModel, jsonQuery);
            List<CompanyFactModel> companyFacts = factNodes
                .Select(n => JsonSerializer.Deserialize<CompanyFactModel>(n))
                .Where(f => f != null)
                .Cast<CompanyFactModel>()
                .ToList();
            var scan = companyFacts
                
                .Aggregate(new { HaveStart = 0, HaveEnd = 0, Total = 0 }, (state, f) =>
                {
                    return new { HaveStart = state.HaveStart + Convert.ToInt16(!string.IsNullOrWhiteSpace(f.StartDate)), HaveEnd = state.HaveEnd + Convert.ToInt16(!string.IsNullOrWhiteSpace(f.EndDate)), Total = state.Total + 1 };
                });
            if(scan.Total != scan.HaveEnd || (scan.HaveStart>0 && scan.HaveStart != scan.HaveEnd))
            {
                throw new InvalidOperationException($"Fact {tickerToCikModel.Ticker}, {secForm}, {factName} has inconsistent start and end dates. HaveStart: {scan.HaveStart}, HaveEnd: {scan.HaveEnd}, Total: {scan.Total}");
            }
            IEnumerable<CompanyFactModel> results =  companyFacts
                    .GroupBy(f => new { f.StartDate, f.EndDate })
                    .Select(g => g.OrderBy(f => f.FiledDateUnixSeconds).First())
                    .Select(f => new { Fact = f, StartEndDiffDays = (f.EndDateUnixSeconds - f.StartDateUnixSeconds ?? SecondsPer90Days) / SecondsPerDay })
                    .Where(f=>f.StartEndDiffDays >= lowerThresholdDays && f.StartEndDiffDays <= upperThresholdDays)
                    .Select(f => f.Fact).ToList();

            return companyFacts;
        }
    }
}
