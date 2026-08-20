using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    public class JsonQueryFilter : IJsonQueryFilterExpression
    {
        private readonly List<IJsonQueryExpression> nextExpressions;

        public JsonQueryFilter(string target, JsonQueryFilterOperators @operator, object value)
        {
            this.nextExpressions = new List<IJsonQueryExpression>();
            Target = target;
            Operator = @operator;
            Value = value;
        }

        public string Target { get; }
        public JsonQueryFilterOperators Operator { get; }
        public object Value { get; }

        public string AsQueryExpressionString()
        {
            return string.Join(" ", new string[] { $"@.{Target}{Operator}{Value}" }.Concat(this.nextExpressions.Select(e => e.AsQueryExpressionString())));
        }
    }
}
