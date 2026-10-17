using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Public.Analysis.Data;
using Public.Analysis.Edgar.Models;
using Public.Analysis.Edgar.TickerToCIK;
using Public.Frameworks.Cache;
using Public.Frameworks.JsonQuery;
using System.Collections;
using System.Text.Json.Nodes;

namespace Public.Analysis.Edgar.RawFacts
{
    public class RawFactsData : IRawFactsData
    {
        public const string RawFactsDataCacheServiceKey = "RawFactsDataCacheService";
        private readonly ITickerToCIKData tickerToCIKData;
        private readonly IDataQueryValidation dataQueryValidation;
        private readonly HttpClient http;
        private readonly IJsonQuery queryJson;
        private readonly ICache<TickerToCIKModel, JsonNode, RawFactsCacheOptions> cache;
        private readonly EdgarOptions edgarOptions;
        private readonly FactsDataOptions factsDataOptions;

        public string Name => nameof(RawFactsData);

        public DataMeta Meta => new DataMeta(this.Name, [new KeyValuePair<string, Type>("Ticker", typeof(string))], DataReturnTypesEnum.UserDefined);

        public RawFactsData(ITickerToCIKData tickerToCIKData, IDataQueryValidation dataQueryValidation, HttpClient http, IOptions<EdgarOptions> edgarOptions, IOptions<FactsDataOptions> factsDataOptions, IJsonQuery queryJson,
            [FromKeyedServices(RawFactsDataCacheServiceKey)] ICache<TickerToCIKModel, JsonNode, RawFactsCacheOptions> cache)
        {
            this.tickerToCIKData = tickerToCIKData;
            this.dataQueryValidation = dataQueryValidation;
            this.http = http;
            this.queryJson = queryJson;
            this.cache = cache;
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
            TickerToCIKModel ticker = tickerData.Single();

            return await GetRawFacts(ticker, dataSetQuery.Path.Skip(1).Select(p => new JsonQueryPath(p) as IJsonQueryExpression));
        }

        public async Task<IEnumerable<JsonNode>> GetRawFacts(TickerToCIKModel ticker, IEnumerable<IJsonQueryExpression> jsonQuery)
        {

            // TODO: Implement caching
            // return await RawFactsFactory(ticker, jsonQuery);
            async Task<JsonNode> RawFactsFactory(TickerToCIKModel ticker, IEnumerable<IJsonQueryExpression> jsonQuery)
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{this.factsDataOptions.DataSecGovApiXbrlCompanyBaseUrl}/CIK{ticker.ToCIKString(this.edgarOptions.CIKMaxLen)}.json");
                request.Headers.TryAddWithoutValidation("User-Agent", this.edgarOptions.UserAgent);

                using CancellationTokenSource ct = new CancellationTokenSource(TimeSpan.FromSeconds(this.edgarOptions.TimeOutSeconds));
                var response = await http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct.Token);

                response = response.EnsureSuccessStatusCode();

                using var contentStream = await response.Content.ReadAsStreamAsync();

                JsonNode? jsonNode = await JsonNode.ParseAsync(contentStream);

                return jsonNode!;
            }

            var factsJson = await this.cache.GetOrAdd(ticker, async (ticker) => { return await RawFactsFactory(ticker, jsonQuery); }, this.cache.Options.CacheMissWaitToUpdateWithFactoryMilli);

            return this.queryJson.Query(factsJson, jsonQuery);
        }
    }
}
