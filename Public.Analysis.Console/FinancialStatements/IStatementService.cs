using Public.Analysis.Console.CompanyFacts.Tree.Models;
using Public.Analysis.Console.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.FinancialStatements
{
    public interface IStatementService
    {
        Task<ServiceResponse<IStatementTreeResult>> GetStatementTree( string statementName, string entity);
    }
}
