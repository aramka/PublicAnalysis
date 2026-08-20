using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    public class JsonQueryLogicalOr : IJsonQueryLogicalExpression
    {
        public string AsQueryExpressionString()
        {
            return "||";
        }
    }
}
