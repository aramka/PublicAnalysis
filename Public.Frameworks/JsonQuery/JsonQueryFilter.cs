using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    public class JsonQueryFilter : IJsonQueryFilterExpression
    {

        public JsonQueryFilter(string target, JsonQueryFilterOperators @operator, object value)
        {
            Target = target;
            Operator = @operator;
            Value = value;
        }

        public string Target { get; }
        public JsonQueryFilterOperators Operator { get; }
        public object Value { get; }

        public string AsQueryExpressionString()
        {
            return $"@.{Target}{Operator}{Value switch { object item when double.TryParse(item.ToString(), out double v) => Value.ToString(), _ => $"'{Value}'" }}";
        }
    }
}
