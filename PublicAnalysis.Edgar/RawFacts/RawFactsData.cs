using Microsoft.Extensions.Options;
using Public.Analysis.Data;
using Public.Analysis.Edgar.Models;
using Public.Analysis.Edgar.TickerToCIK;
using System.Collections;
using System.Linq;
using System.Text.Json;

namespace Public.Analysis.Edgar.RawFacts
{
    public class RawFactsData : IEdgarData
    {
        private readonly ITickerToCIKData tickerToCIKData;
        private readonly IDataQueryValidation dataQueryValidation;
        private readonly HttpClient http;
        private readonly EdgarOptions edgarOptions;
        private readonly FactsDataOptions factsDataOptions;

        public string Name => nameof(RawFactsData);

        public DataMeta Meta => new DataMeta(this.Name, [new KeyValuePair<string, Type>("Ticker", typeof(string))], DataReturnTypesEnum.UserDefined);

        public RawFactsData(ITickerToCIKData tickerToCIKData, IDataQueryValidation dataQueryValidation, HttpClient http, IOptions<EdgarOptions> edgarOptions, IOptions<FactsDataOptions> factsDataOptions)
        {
            this.tickerToCIKData = tickerToCIKData;
            this.dataQueryValidation = dataQueryValidation;
            this.http = http;
            this.edgarOptions = edgarOptions.Value;
            this.factsDataOptions = factsDataOptions.Value;
        }

        public async Task<IEnumerable> Query(DataQuery dataSetQuery)
        {
            dataQueryValidation.ThrowIfNotValid(dataSetQuery, this.Meta);

            IEnumerable<TickerToCIKModel> tickerData = (await this.tickerToCIKData.Query(dataSetQuery) as IEnumerable<TickerToCIKModel>)!;

            if (tickerData is null || !tickerData.Any())
            {
                throw new ArgumentException($"Ticker {dataSetQuery.Path[0]} not found");
            }
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{this.factsDataOptions.DataSecGovApiXbrlCompanyBaseUrl}/CIK{tickerData.Single().ToCIKString(this.edgarOptions.CIKMaxLen)}.json");
            request.Headers.TryAddWithoutValidation("User-Agent", this.edgarOptions.UserAgent);

            using CancellationTokenSource ct = new CancellationTokenSource(TimeSpan.FromSeconds(this.edgarOptions.TimeOutSeconds));
            var response = await http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct.Token);

            response = response.EnsureSuccessStatusCode();

            using var contentStream = await response.Content.ReadAsStreamAsync();

            using JsonDocument jsonDoc = await JsonDocument.ParseAsync(contentStream);
            
            return new JsonElement[] { jsonDoc.RootElement };
		}
    }
}
