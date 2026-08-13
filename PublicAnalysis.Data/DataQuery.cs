using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAnalysis.Data
{
    public class DataQuery
    {

        public DataQuery(string[] path)
        {
            this.Path = path;
        }

        public string[] Path { get; }
    }
}
