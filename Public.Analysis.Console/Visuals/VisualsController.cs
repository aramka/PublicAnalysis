using Microsoft.AspNetCore.Mvc;
using Public.Analysis.Console.Visuals.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.Visuals
{
    
    public class StatementTreeController : ControllerBase { }
    [ApiController]
    [Route("visuals")]
    public class VisualsController : ControllerBase
    {
        [HttpGet("{visualType}/{dataSetName}/{datapointName}/{entity}")]
        public IActionResult GetTimeSeries(string visualType, string dataSetName,  string datapointName, string entity)
        {
            return Ok(new TimeSeriesDataPoint[] { 
                new TimeSeriesDataPoint { TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds(), Value = 30.0m }, 
                new TimeSeriesDataPoint { TimeStamp = DateTimeOffset.Now.AddMonths(6).ToUnixTimeSeconds(), Value = 110.0m },
                new TimeSeriesDataPoint { TimeStamp = DateTimeOffset.Now.AddMonths(9).ToUnixTimeSeconds(), Value = 25.0m }
            });
        }
    }
}
