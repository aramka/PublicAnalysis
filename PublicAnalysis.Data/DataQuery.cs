using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Data
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
