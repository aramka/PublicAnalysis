using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Edgar.Models
{
    public class EdgarOptions
    {
        public int CIKMaxLen { get; set; }
        public string? UserAgent { get; set; }
        public double TimeOutSeconds { get; set; }
    }
}
