using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.Visuals.Models
{
    public class TimeSeriesDataPoint
    {
        public long TimeStamp { get; set; } = DateTimeOffset.Now.ToUnixTimeSeconds();
        public decimal Value { get; set; } = 0m;
    }
}
