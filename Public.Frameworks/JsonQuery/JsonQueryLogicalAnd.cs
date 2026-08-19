using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    public class JsonQueryLogicalAnd : IJsonQueryExpression
    {
        public string AsQueryExpressionString()
        {
            return "&&";
        }
    }
}
