using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    public class JsonQueryBuilder
    {
        private readonly IThrowIfNotValidConsecutiveJsonQueryExpressions throwIfNotValid;
        private List<IJsonQueryExpression> expressions = new List<IJsonQueryExpression>();

        public JsonQueryBuilder(IThrowIfNotValidConsecutiveJsonQueryExpressions throwIfNotValidConsecutiveExpressions)
        {
            this.throwIfNotValid = throwIfNotValidConsecutiveExpressions;
        }

        public IEnumerable<IJsonQueryFilterExpression> Filters => this.expressions.Where(e => e is IJsonQueryFilterExpression).Cast<IJsonQueryFilterExpression>().ToArray();

        public JsonQueryBuilder AddExpression(IJsonQueryExpression expression)
        {
            this.throwIfNotValid.ThrowIfNotValid(this.expressions.LastOrDefault(), expression);
            this.expressions.Add(expression);
            return this;
        }

        public string AsJsonPathQueryString()
        {
            List<List<IJsonQueryExpression>> allGroups = new List<List<IJsonQueryExpression>>();
            List<IJsonQueryExpression> filterAndLogicalExpressionGroup = new List<IJsonQueryExpression>();
            foreach (var expression in this.expressions)
            {
                switch (expression)
                {
                    case IJsonQueryFilterExpression filter:
                        filterAndLogicalExpressionGroup.Add(filter);
                        break;
                    case IJsonQueryLogicalExpression logical:
                        filterAndLogicalExpressionGroup.Add(logical);
                        break;
                    case IJsonQueryPathExpression path:
                        RemoveLastIfIsLogicalThenAddToGroups(allGroups, filterAndLogicalExpressionGroup);
                        filterAndLogicalExpressionGroup = new List<IJsonQueryExpression>();
                        allGroups.Add(new List<IJsonQueryExpression> { path });
                        break;
                    default:
                        throw new InvalidOperationException($"Expression type {expression.GetType().Name} is not supported.");
                }
            }
            RemoveLastIfIsLogicalThenAddToGroups(allGroups, filterAndLogicalExpressionGroup);
            var segments = allGroups
                .Where(g => g.Any())
                .Select(g => g.First() switch
                {
                    IJsonQueryFilterExpression filter => 
                    $"[?{
                            g.Select(e => $" {e.AsQueryExpressionString()}")
                            .Aggregate(new StringBuilder(), (sb, current) => { sb.Append(current); return sb; })
                            .ToString()
                        }]",
                    _ => g.Select(e => $"[{e.AsQueryExpressionString()}]").Aggregate(new StringBuilder(), (sb, current) => { sb.Append(current); return sb; }).ToString()
                });
            var query = segments.Aggregate(new StringBuilder(), (sb, currentSegment) => { sb.Append(currentSegment); return sb; }).ToString();
            return $"${query}";
        }

        private static void RemoveLastIfIsLogicalThenAddToGroups(List<List<IJsonQueryExpression>> allGroups, List<IJsonQueryExpression> filterAndLogicalExpressionGroup)
        {
            if (!filterAndLogicalExpressionGroup.Any())
            {
                return;
            }
            if (filterAndLogicalExpressionGroup.Last() is IJsonQueryLogicalExpression)
            {
                filterAndLogicalExpressionGroup.RemoveAt(filterAndLogicalExpressionGroup.Count - 1);
            }
            allGroups.Add(filterAndLogicalExpressionGroup);

        }
    }
}