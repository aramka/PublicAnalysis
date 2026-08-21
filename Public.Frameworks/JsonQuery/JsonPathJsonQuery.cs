using Json.Path;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;

namespace Public.Frameworks.JsonQuery
{
    public class JsonPathJsonQuery : IJsonQuery
    {
        private readonly IJsonQueryBuilder queryBuilder;

        public JsonPathJsonQuery(IJsonQueryBuilder queryBuilder)
        {
            this.queryBuilder = queryBuilder;
        }
        public IEnumerable<JsonNode> Query(JsonNode jsonNode, IEnumerable<string> path, IJsonQueryFilterExpression[] filter=null)
        {
            var jsonQueryExpressions = path.Select(p => new JsonQueryPath(p) as IJsonQueryExpression).Concat(filter ?? Enumerable.Empty<IJsonQueryFilterExpression>());
            var queryString = queryBuilder.AddExpressions(jsonQueryExpressions).AsJsonPathQueryString();

            JsonPath jsonPath = JsonPath.Parse(queryString);
            
            var pathResult = jsonPath.Evaluate(jsonNode);
            return pathResult?.Matches!.Select(m => m.Value!) ?? Enumerable.Empty<JsonNode>();
        }
    }
}
