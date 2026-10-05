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
        [HttpGet("{ticker}/CompanyFacts/{factName}/time-series")]
        public async Task<IActionResult> GetCompanyFactTimeSeries(string ticker, string factName)
        {
            // Placeholder for actual logic to retrieve time series data for the specified company fact
            // For now, return a sample response
            var sampleData = new List<TimeSeriesDataPoint>
            {
                new TimeSeriesDataPoint { TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds(), Value = 50m },
                new TimeSeriesDataPoint { TimeStamp = DateTimeOffset.Now.AddDays(-1).ToUnixTimeSeconds(), Value = 100m },
                new TimeSeriesDataPoint { TimeStamp = DateTimeOffset.Now.AddDays(-2).ToUnixTimeSeconds(), Value =75m },
                new TimeSeriesDataPoint { TimeStamp = DateTimeOffset.Now.AddDays(-2).ToUnixTimeSeconds(), Value =150m }
            };
            return Ok(sampleData);
        }
    }
}
