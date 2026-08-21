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
            cases.Add((null, typeof(IJsonQueryFilterExpression)), true);
            cases.Add((null, typeof(IJsonQueryLogicalExpression)), false);
            cases.Add((null, typeof(IJsonQueryPathExpression)), true);
            cases.Add((typeof(IJsonQueryFilterExpression), null), false);
            cases.Add((typeof(IJsonQueryFilterExpression), typeof(IJsonQueryFilterExpression)), false);
            cases.Add((typeof(IJsonQueryFilterExpression), typeof(IJsonQueryLogicalExpression)), true);
            cases.Add((typeof(IJsonQueryFilterExpression), typeof(IJsonQueryPathExpression)), true);
            cases.Add((typeof(IJsonQueryLogicalExpression), null), false);
            cases.Add((typeof(IJsonQueryLogicalExpression), typeof(IJsonQueryFilterExpression)), true);
            cases.Add((typeof(IJsonQueryLogicalExpression), typeof(IJsonQueryLogicalExpression)), false);
            cases.Add((typeof(IJsonQueryLogicalExpression), typeof(IJsonQueryPathExpression)), false);
            cases.Add((typeof(IJsonQueryPathExpression), null), false);
            cases.Add((typeof(IJsonQueryPathExpression), typeof(IJsonQueryFilterExpression)), true);
            cases.Add((typeof(IJsonQueryPathExpression), typeof(IJsonQueryLogicalExpression)), false);
            cases.Add((typeof(IJsonQueryPathExpression), typeof(IJsonQueryPathExpression)), true);

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
                IJsonQueryFilterExpression => typeof(IJsonQueryFilterExpression),
                IJsonQueryLogicalExpression => typeof(IJsonQueryLogicalExpression),
                IJsonQueryPathExpression => typeof(IJsonQueryPathExpression),
                _ => throw new NotImplementedException($"Type {current.GetType()} is not implemented.")
            };
        }
    }
}
