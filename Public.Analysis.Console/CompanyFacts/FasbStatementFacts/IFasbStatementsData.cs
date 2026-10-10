using Public.Analysis.Console.CompanyFacts.FasbStatementFacts.Models;

namespace Public.Analysis.Console.CompanyFacts.FasbStatementFacts
{
    public interface IFasbStatementsData
    {
        Task<IEnumerable<StatementTaxonomyModel>> GetStatementTrees(string statementName);
    }
}