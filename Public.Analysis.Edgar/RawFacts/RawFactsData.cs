using Microsoft.Extensions.Options;
using Public.Analysis.Data;
using Public.Analysis.Edgar.Models;
using Public.Analysis.Edgar.TickerToCIK;
using System.Collections;
using System.Linq;
using System.Text.Json;
using Json.More;
using Json.Path;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using Public.Frameworks.JsonQuery;

namespace Public.Analysis.Edgar.RawFacts
{
    public class RawFactsData : IEdgarData
    {
        private readonly ITickerToCIKData tickerToCIKData;
        private readonly IDataQueryValidation dataQueryValidation;
        private readonly HttpClient http;
        private readonly IJsonQuery queryJson;
        private readonly EdgarOptions edgarOptions;
        private readonly FactsDataOptions factsDataOptions;

        public string Name => nameof(RawFactsData);

        public DataMeta Meta => new DataMeta(this.Name, [new KeyValuePair<string, Type>("Ticker", typeof(string))], DataReturnTypesEnum.UserDefined);

        public RawFactsData(ITickerToCIKData tickerToCIKData, IDataQueryValidation dataQueryValidation, HttpClient http, IOptions<EdgarOptions> edgarOptions, IOptions<FactsDataOptions> factsDataOptions, Frameworks.JsonQuery.IJsonQuery queryJson)
        {
            this.tickerToCIKData = tickerToCIKData;
            this.dataQueryValidation = dataQueryValidation;
            this.http = http;
            this.queryJson = queryJson;
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

            JsonNode? jsonNode = await JsonNode.ParseAsync(contentStream);

            return this.queryJson.Query(jsonNode, dataSetQuery.Path.Skip(1).Select(p => new JsonQueryPath(p) as IJsonQueryExpression));
		}
    }
}
