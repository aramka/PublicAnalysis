using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    public class JsonQueryBuilder
    {
        private List<IJsonQueryExpression> expressions = new List<IJsonQueryExpression>();

        public IEnumerable<IJsonQueryFilterExpression> Filters => this.expressions.Where(e=>e is IJsonQueryFilterExpression).Cast<IJsonQueryFilterExpression>().ToArray();

        public JsonQueryBuilder AddExpression(IJsonQueryExpression expression)
        {
            if(expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
           
            this.expressions.Add(expression);
            return this;
        }

        //public JsonQueryBuilder AddPath(string v)
        //{
        //    paths.Add($"['{v}']");
        //    return this;
        //}

        public string AsJsonPathQueryString()
        {
            return $"${string.Join(string.Empty, this.expressions.Select(e => e.AsQueryExpressionString()))}";
        }
    }
}