using Public.Analysis.Console.CompanyFacts.Models.StatementFactsModels;
using Public.Analysis.Console.Services.Models;
using Public.Analysis.Console.Visuals.Models;

namespace Public.Analysis.Console.CompanyFacts
{
    public interface ICompanyFactsService
    {
        Task<ServiceResponse<IStatementTreeResult>> GetCompanyFactAsTree(string ticker, string statementName);
        Task<ServiceResponse<IEnumerable<TimeSeriesDataPoint>>> GetCompanyFactTimeSeries(string ticker, string factName);
    }
}