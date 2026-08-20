using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    public class JsonQueryLogicalAnd : IJsonQueryLogicalExpression
    {
        public string AsQueryExpressionString()
        {
            return "&&";
        }
    }
}
