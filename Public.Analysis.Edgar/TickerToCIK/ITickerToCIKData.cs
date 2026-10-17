using Public.Frameworks.Initialization;
using Public.Analysis.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Public.Analysis.Edgar.TickerToCIK;

namespace Public.Analysis.Edgar
{
    public interface ITickerToCIKData : IEdgarData
    {
        Task<TickerToCIKModel?> LookupTicker(string ticker);
    }
}
