using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.Services.Models
{
    public record Range<T>(T Start, T End)
    {
        public static readonly Range<long> AllDates = new Range<long>(DateTimeOffset.MinValue.ToUnixTimeSeconds(), DateTimeOffset.MaxValue.ToUnixTimeSeconds());
    }
}
