using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Nodes;

namespace Public.Frameworks.JsonQuery
{
    public interface IJsonQuery
    {
        IEnumerable<JsonNode> Query(JsonNode? jsonNode, IEnumerable<IJsonQueryExpression> jsonQueryExpressions);

        Task<IEnumerable<JsonNode>> Query(string jsonFilePath, IEnumerable<IJsonQueryExpression> jsonQueryExpressions);
    }
}
