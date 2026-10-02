using Public.Analysis.Console.JsonSerDes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Public.Analysis.Console.Visuals.Models
{
    [JsonConverter(typeof(ToStringJsonConverter<VisualType>))]
    public record VisualType
    {
        public static readonly VisualType TimeSeries = new VisualType("time-series");
        VisualType(string visual)
        {
            this.Name = visual;
        }

        public string Name { get; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
