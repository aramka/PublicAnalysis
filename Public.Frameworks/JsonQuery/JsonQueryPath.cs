using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    public class JsonQueryPath : IJsonQueryPathExpression
    {
        public JsonQueryPath(string path)
        {
            if(string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            Path = path;
        }

        public string Path { get; }

        public string AsQueryExpressionString()
        {
            return $"'{this.Path}'";
        }
    }
}
