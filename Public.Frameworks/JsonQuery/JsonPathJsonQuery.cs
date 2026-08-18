using Json.Path;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;

namespace Public.Frameworks.JsonQuery
{
    public class JsonPathJsonQuery : IJsonQuery
    {
        public IEnumerable<JsonNode> Query(JsonNode jsonNode, IEnumerable<string> path)
        {
            string jsonPathQueryString = $"${string.Join(string.Empty, path.Select(p => $"['{p}']"))}";
            JsonPath jsonPath = JsonPath.Parse(jsonPathQueryString);
            var pathResult = jsonPath.Evaluate(jsonNode);
            return pathResult.Matches.Select(m => m.Value);
        }
    }
}
