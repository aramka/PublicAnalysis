using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    public record JsonQueryFilter(string Target, JsonQueryFilterOperators Operator, object Value) : IJsonQueryFilterExpression;
}
