using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    public class JsonQueryPath : IJsonQueryPathExpression
    {
        public JsonQueryPath(string path)
        {
            Path = path;
        }

        public string Path { get; }

        public string AsQueryExpressionString()
        {
            return $"'{this.Path}'";
        }
    }
}
