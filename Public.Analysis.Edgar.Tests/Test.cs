using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Public.Analysis.Data;
using Public.Analysis.Edgar.Models;
using Public.Analysis.Edgar.RawFacts;
using Public.Analysis.Edgar.TickerToCIK;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text.Json.Nodes;
using System.Text.Json;
using AwesomeAssertions;

namespace Public.Analysis.Edgar.Tests
{

    [TestClass]
    public class Test
    {
        [TestMethod]
        public async Task  Test1()
        {
            int aaplCik = 1234;
            string aaplTicker = "AAPL";

            Mock<ITickerToCIKData> tickerCIKDataMoq = new Mock<ITickerToCIKData>();
            tickerCIKDataMoq.Setup(a => a.Query(It.IsAny<DataQuery>())).ReturnsAsync(new TickerToCIKModel[] { new TickerToCIKModel { CikStr = aaplCik, Ticker = aaplTicker } });

            Mock<IDataQueryValidation> dataQueryValidationMoq = new Mock<IDataQueryValidation>();
            string applFactsJsonString = EdgarFactsFactory.EdgarFactsAPJsonString; // File.ReadAllText("EdgarFactsJSONData\\2026-AAPL-CIK0000320193-Facts-AccountsPayable.json");

            HttpResponseMessage responseMessage = new HttpResponseMessage { Content = new StringContent(applFactsJsonString, System.Text.Encoding.UTF8, new MediaTypeHeaderValue("application/json")) };

            Mock<HttpMessageHandler> httpHandlerMoq = new Mock<HttpMessageHandler>();
            httpHandlerMoq.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(responseMessage);


            HttpClient clientHttp = new HttpClient(httpHandlerMoq.Object);
            EdgarOptions options = new EdgarOptions { CIKMaxLen = 10, TimeOutSeconds = 30, UserAgent = "Test User Agent" };
            Mock<IOptions<EdgarOptions>> edgarOptionsMoq = new Mock<IOptions<EdgarOptions>>();
            edgarOptionsMoq.Setup(a => a.Value).Returns(options);
            FactsDataOptions factsDataOptions = new FactsDataOptions { DataSecGovApiXbrlCompanyBaseUrl = "http://secfactsbaseurl" };
            Mock<IOptions<FactsDataOptions>> factsOptionsMoq = new Mock<IOptions<FactsDataOptions>>();
            factsOptionsMoq.Setup(a => a.Value).Returns(factsDataOptions);

            RawFactsData rawFacts = new RawFactsData(tickerCIKDataMoq.Object, dataQueryValidationMoq.Object, clientHttp, edgarOptionsMoq.Object, factsOptionsMoq.Object);
            List<string> factsPath = new List<string> { "facts", "us-gaap", "AccountsPayable","units","USD" };

            var actual = await rawFacts.Query(new DataQuery(factsPath.ToArray()));
            var match = actual.Cast<JsonNode>().ToList(); //the single matching node
            JsonArray jArray = match[0].AsArray();
            Assert.HasCount(2, jArray);

            var actualItems = jArray.Deserialize<AccountEntry[]>();

            actualItems.Should().BeEquivalentTo(EdgarFactsFactory.PayablesEntriesExpected);

        }
    }
}
