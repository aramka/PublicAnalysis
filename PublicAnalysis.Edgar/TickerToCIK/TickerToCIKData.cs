using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Public.Analysis.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Public.Analysis.Edgar.TickerToCIK
{
    public class TickerToCIKData : ITickerToCIKData
    {
        private readonly HttpClient http;
        private readonly IDataQueryValidation dataQueryValidation;
        private readonly ILogger<TickerToCIKData> logger;
        private readonly EdgarOptions edgarOptions;
        private readonly TickerToCIKDataOptions tickerToCIKDataOptions;
        private Dictionary<string, TickerToCIKModel> cikByTicker = new Dictionary<string, TickerToCIKModel>();
        private bool loaded = false;

        public TickerToCIKData(IOptions<TickerToCIKDataOptions> options, IOptions<EdgarOptions> edgarOptions, HttpClient http, IDataQueryValidation dataQueryValidation, ILogger<TickerToCIKData> logger) {
            this.tickerToCIKDataOptions = options.Value;
            this.http = http;
            this.dataQueryValidation = dataQueryValidation;
            this.logger = logger;
            this.edgarOptions = edgarOptions.Value;
        }

        public string Name => nameof(TickerToCIKData);

        public DataMeta Meta => new DataMeta(this.Name, [new KeyValuePair<string, Type>("Ticker", typeof(string))], DataReturnTypesEnum.String);

        public async Task Load(bool reload = false)
        {
            /*client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "PersonalResearchProject adjdrew@hotmail.com");
            */

            if (loaded && !reload)
            {
                return;
            }



            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, this.tickerToCIKDataOptions.SecGovFilesCompanyTickersJsonUrl);
                request.Headers.TryAddWithoutValidation("User-Agent", this.edgarOptions.UserAgent);

                using CancellationTokenSource ct = new CancellationTokenSource(TimeSpan.FromSeconds(this.tickerToCIKDataOptions.TimeOutSeconds));
                var response = await http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct.Token);

                response = response.EnsureSuccessStatusCode();


                var rawPayload = await response.Content.ReadFromJsonAsync<Dictionary<string, TickerToCIKModel>>(ct.Token);

                var newDict = rawPayload!.ToDictionary(kvp => kvp.Value.Ticker!, kvp => kvp.Value);

                Interlocked.Exchange(ref this.cikByTicker, newDict);
                this.loaded = true;

                // 2. Fetch the official SEC company ticker mapping dictionary
                // string url = "https://www.sec.gov/files/company_tickers.json";
                // string jsonString = await client.GetStringAsync(url);
                //string basePath = @"C:\Users\Andrew\Development\EdgarCLI";
                //File.WriteAllText($"{basePath}\\TickersToCIK.json", jsonString);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

        }

        public async Task<IEnumerable> Query(DataQuery dataSetQuery)
        {
            this.dataQueryValidation.ThrowIfNotValid(dataSetQuery, this.Meta);

            if (!this.loaded)
            {
                throw new InvalidOperationException($"Not loaded. Must call {nameof(Load)} before {nameof(Query)}");
            }

            if (!this.cikByTicker.TryGetValue(dataSetQuery.Path[0], out var tickerToCIKModel))
            {
                return Enumerable.Empty<TickerToCIKModel>();
            }

            return new TickerToCIKModel[] { tickerToCIKModel };
        }
    }
}
