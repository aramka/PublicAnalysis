using Json.Path;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;

namespace Public.Frameworks.JsonQuery
{
    public class JsonPathJsonQuery : IJsonQuery
    {
        private readonly IJsonQueryBuilderFactory queryBuilderFactory;

        public JsonPathJsonQuery(IJsonQueryBuilderFactory queryBuilderFactory)
        {
            this.queryBuilderFactory = queryBuilderFactory;
        }

        public IEnumerable<JsonNode> Query(JsonNode jsonNode, IEnumerable<IJsonQueryExpression> jsonQueryExpressions)
        {
            if(jsonNode is null) throw new ArgumentNullException(nameof(jsonNode));

            var queryBuilder = queryBuilderFactory.Create();
            var queryString = queryBuilder.AddExpressions(jsonQueryExpressions).AsJsonPathQueryString();

            JsonPath jsonPath = JsonPath.Parse(queryString);

            var pathResult = jsonPath.Evaluate(jsonNode);
            return pathResult?.Matches!.Select(m => m.Value!) ?? Enumerable.Empty<JsonNode>();
        }
    }
}
