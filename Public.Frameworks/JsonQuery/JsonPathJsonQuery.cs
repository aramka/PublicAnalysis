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
            throw new NotImplementedException();
        }
    }
}
