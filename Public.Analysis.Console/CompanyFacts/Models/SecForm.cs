using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json.Serialization;

namespace Public.Analysis.Console.CompanyFacts.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter<SecForm>))]
    public enum SecForm
    {
        TenQ=1,
        TenK=2
    }
}
