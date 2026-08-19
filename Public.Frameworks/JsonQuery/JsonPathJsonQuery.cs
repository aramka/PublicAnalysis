using Json.Path;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;

namespace Public.Frameworks.JsonQuery
{
    public class JsonPathJsonQuery : IJsonQuery
    {
        public IEnumerable<JsonNode> Query(JsonNode jsonNode, IEnumerable<string> path, IJsonQueryFilterExpression[] filter=null)
        {
            var pIs = path switch
            {
                List<string> l => throw new ArgumentNullException(nameof(path)),
                _ => "notknown"
            };
            string jsonPathQueryString = $"${string.Join(string.Empty, path.Select(p => $"['{p}']"))}";
            string filterQueryString = BuildQueryFilter(filter);  

            JsonPath jsonPath = JsonPath.Parse($"{jsonPathQueryString}{filterQueryString}");
            var pathResult = jsonPath.Evaluate(jsonNode);
            return pathResult?.Matches!.Select(m => m.Value!) ?? Enumerable.Empty<JsonNode>();
        }

        private string BuildQueryFilter(IJsonQueryFilterExpression[] filter)
        {
            filter = filter ?? Enumerable.Empty<IJsonQueryFilterExpression>().ToArray();

            var s = filter
                .Select(f => f switch
                {
                    JsonQueryFilter eq => $"@.{eq.Target}{eq.Operator}{eq.Value}",
                    _ => throw new NotImplementedException($"Filter type {f.GetType().Name} is not implemented.")
                });

            return s.Any() ? $"[? {string.Join(" && ", s)} ]" : string.Empty;
        }
    }
}
