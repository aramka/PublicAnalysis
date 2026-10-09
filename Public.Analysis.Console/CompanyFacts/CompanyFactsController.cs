using Microsoft.AspNetCore.Mvc;
using Public.Analysis.Console.CompanyFacts.Tree.Models;
using Public.Analysis.Console.FinancialStatements;
using Public.Analysis.Console.Services.Models;
using Public.Analysis.Console.Visuals.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.CompanyFacts
{
    [ApiController]
    public class CompanyFactsController : ControllerBase
    {
        private readonly ICompanyFactsService companyFactsService;

        public CompanyFactsController(ICompanyFactsService companyFactsService)
        {
            this.companyFactsService = companyFactsService;
        }
        [HttpGet("{ticker}/{factName}/time-series")]
        public async Task<IActionResult> GetCompanyFactTimeSeries(string ticker, string factName)
        {
            ServiceResponse<IEnumerable<TimeSeriesDataPoint>> dataPoints = await this.companyFactsService.GetCompanyFactTimeSeries(ticker, factName);
            return Ok(dataPoints);
        }
        [HttpGet("{ticker}/{statementName}/facts-tree")]
        public async Task<IActionResult> GetFactsTree(string ticker, string statementName)
        {
            // TODO: Handle bad request where validation errors. Sometimes they should goto the UI but sometimes not. Should consider middleware to filter and send 400 when needed.
            ServiceResponse<IStatementTreeResult> data = await this.companyFactsService.GetCompanyFactAsTree(ticker, statementName);
            return Ok(data);
        }
    }
}
