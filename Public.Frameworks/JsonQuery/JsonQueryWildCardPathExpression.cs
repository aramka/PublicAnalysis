using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    public class JsonQueryWildCardPathExpression : IJsonQueryWildCardPathExpression
    {
        public string AsQueryExpressionString()
        {
            return "*";
        }
    }
}
