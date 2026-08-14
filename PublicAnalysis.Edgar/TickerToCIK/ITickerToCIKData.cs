using PublicAnalysis.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAnalysis.Edgar
{
    public interface ITickerToCIKData : IEdgarData
    {
        Task Load(bool reload=false);
    }
}
