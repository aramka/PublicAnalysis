using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("{ticker}/CompanyFacts/{factName}/time-series")]
        public async Task<IActionResult> GetCompanyFactTimeSeries(string ticker, string factName)
        {
            var dataPoints = await this.companyFactsService.GetCompanyFactTimeSeries(ticker, factName);
            return Ok(dataPoints);
        }
    }
}
