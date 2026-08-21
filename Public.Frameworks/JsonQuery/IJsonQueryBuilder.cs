namespace Public.Frameworks.JsonQuery
{
    public interface IJsonQueryBuilder
    {

        JsonQueryBuilder AddExpression(IJsonQueryExpression expression);
        JsonQueryBuilder AddExpressions(IEnumerable<IJsonQueryExpression> expressions);
        string AsJsonPathQueryString();
    }
}