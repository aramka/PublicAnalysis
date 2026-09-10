using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    public interface IJsonQueryBuilderFactory
    {
        IJsonQueryBuilder Create();
    }
}
