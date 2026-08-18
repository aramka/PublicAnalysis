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
using System.Collections.ObjectModel;
using Public.Frameworks.JsonQuery;

namespace Public.Analysis.Edgar.Tests
{

    [TestClass]
    public class Test
    {

        readonly int aaplCik = 1234;
        readonly string aaplTicker = "AAPL";
        private readonly RawFactsData rawFacts;

        private readonly string[] factsPath = new string[] { "AAPL", "facts", "us-gaap", "AccountsPayable", "units", "USD" };

        private readonly Mock<ITickerToCIKData> tickerCIKDataMoq = new Mock<ITickerToCIKData>();
        private readonly Mock<IJsonQuery> jsonQueryMoq = new Mock<IJsonQuery>();

        public Test()
        {

            tickerCIKDataMoq.Setup(a => a.Query(It.Is<DataQuery>(q => q.Path[0] == aaplTicker))).ReturnsAsync(new TickerToCIKModel[] { new TickerToCIKModel { CikStr = aaplCik, Ticker = aaplTicker } });

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

            this.rawFacts = new RawFactsData(tickerCIKDataMoq.Object, dataQueryValidationMoq.Object, clientHttp, edgarOptionsMoq.Object, factsOptionsMoq.Object, jsonQueryMoq.Object);
            
        }
        [TestMethod]
        public async Task Query_GetsTicker()
        {
            
            var actual = await rawFacts.Query(new DataQuery(factsPath.ToArray()));
            
            this.tickerCIKDataMoq.Verify(a => a.Query(It.Is<DataQuery>(q => q.Path[0] == aaplTicker)), Times.Once);

        }

        [TestMethod]
        public async Task Query_ThrowsIfNoTicker()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await rawFacts.Query(new DataQuery(new string[] { "NOTICKER", "facts", "us-gaap", "AccountsPayable", "units", "USD" })));
        }

        [TestMethod]
        public async Task Query_CallsIQueryJson()
        {
            JsonNode jsonNodePassedToQuery = null;
            IEnumerable<string> pathPassedToQuery = null;

            var expectedReturn = new List<JsonNode> { JsonNode.Parse("{\"val\": 1234}")! };
            jsonQueryMoq.Setup(a => a.Query(It.IsAny<JsonNode>(), It.IsAny<IEnumerable<string>>()))
                .Returns(expectedReturn)
                .Callback<JsonNode, IEnumerable<string>>((jsonNode, path) =>
                {
                    jsonNodePassedToQuery = jsonNode;
                    pathPassedToQuery = path;
                });

            var actualReturn = await rawFacts.Query(new DataQuery(factsPath.ToArray()));

            actualReturn.Should().BeEquivalentTo(expectedReturn);

            pathPassedToQuery.Should().BeEquivalentTo(factsPath.Skip(1)); // Skip the ticker in the path
            jsonNodePassedToQuery.Should().NotBeNull();

            var nodePassedToQueryExpeced = JsonNode.Parse(EdgarFactsFactory.EdgarFactsAPJsonString);
            jsonNodePassedToQuery.ToString().Should().BeEquivalentTo(nodePassedToQueryExpeced!.ToString());



        }
    }
}
