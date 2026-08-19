using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    public class JsonQueryLogicalOr : IJsonQueryFilterExpression
    {
        public string AsJsonPathQueryExpression()
        {
            return "||";
        }
    }
}
