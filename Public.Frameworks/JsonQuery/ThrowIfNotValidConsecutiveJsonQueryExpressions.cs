using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Frameworks.JsonQuery
{
    public class ThrowIfNotValidConsecutiveJsonQueryExpressions : IThrowIfNotValidConsecutiveJsonQueryExpressions
    {
        Dictionary<(Type?, Type?), bool> cases = new Dictionary<(Type?, Type?), bool>();
        public ThrowIfNotValidConsecutiveJsonQueryExpressions() {

            cases.Add((null, null), false);
            cases.Add((null, typeof(JsonQueryFilter)), true);
            cases.Add((null, typeof(JsonQueryLogicalAnd)), false);
            cases.Add((null, typeof(JsonQueryLogicalOr)), false);
            cases.Add((null, typeof(JsonQueryPath)), true);
            cases.Add((typeof(JsonQueryFilter), null), false);
            cases.Add((typeof(JsonQueryFilter), typeof(JsonQueryFilter)), false);
            cases.Add((typeof(JsonQueryFilter), typeof(JsonQueryLogicalAnd)), true);
            cases.Add((typeof(JsonQueryFilter), typeof(JsonQueryLogicalOr)), true);
            cases.Add((typeof(JsonQueryFilter), typeof(JsonQueryPath)), true);
            cases.Add((typeof(JsonQueryLogicalAnd), null), false);
            cases.Add((typeof(JsonQueryLogicalAnd), typeof(JsonQueryFilter)), true);
            cases.Add((typeof(JsonQueryLogicalAnd), typeof(JsonQueryLogicalAnd)), false);
            cases.Add((typeof(JsonQueryLogicalAnd), typeof(JsonQueryLogicalOr)), false);
            cases.Add((typeof(JsonQueryLogicalAnd), typeof(JsonQueryPath)), false);
            cases.Add((typeof(JsonQueryLogicalOr), null), false);
            cases.Add((typeof(JsonQueryLogicalOr), typeof(JsonQueryFilter)), true);
            cases.Add((typeof(JsonQueryLogicalOr), typeof(JsonQueryLogicalAnd)), false);
            cases.Add((typeof(JsonQueryLogicalOr), typeof(JsonQueryLogicalOr)), false);
            cases.Add((typeof(JsonQueryLogicalOr), typeof(JsonQueryPath)), false);
            cases.Add((typeof(JsonQueryPath), null), false);
            cases.Add((typeof(JsonQueryPath), typeof(JsonQueryFilter)), true);
            cases.Add((typeof(JsonQueryPath), typeof(JsonQueryLogicalAnd)), false);
            cases.Add((typeof(JsonQueryPath), typeof(JsonQueryLogicalOr)), false);
            cases.Add((typeof(JsonQueryPath), typeof(JsonQueryPath)), true);

        }
        public void ThrowIfNotValid(IJsonQueryExpression? current, IJsonQueryExpression? next)
        {
            Type? t1 = GetExpressionType(current);
            Type? t2 = GetExpressionType(next);

            if (!cases.TryGetValue((t1, t2), out bool expected) || !expected)
            {
                throw new ArgumentException($"Consecutive JSON query expressions of types '{current?.GetType().Name}' and '{next?.GetType().Name}' are not valid.");
            }
        }

        private static Type? GetExpressionType(IJsonQueryExpression? current)
        {
            return current switch
            {
                null => null,
                IJsonQueryFilterExpression => typeof(JsonQueryFilter),
                IJsonQueryLogicalExpression => typeof(JsonQueryLogicalAnd),
                IJsonQueryPathExpression => typeof(JsonQueryPath),
                _ => throw new NotImplementedException($"Type {current.GetType()} is not implemented.")
            };
        }
    }
}
