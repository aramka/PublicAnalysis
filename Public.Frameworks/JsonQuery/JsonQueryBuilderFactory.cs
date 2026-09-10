using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    public class JsonQueryBuilderFactory : IJsonQueryBuilderFactory
    {
        public IJsonQueryBuilder Create()
        {
            return new JsonQueryBuilder();
        }
    }
}
