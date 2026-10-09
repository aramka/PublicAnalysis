using Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels;
using Public.Analysis.Console.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.CompanyFacts
{
    public interface IStatementFactsService
    {
        Task<ServiceResponse<IStatementTreeResult>> GetStatementTree( string statementName, string entity);
    }
}
