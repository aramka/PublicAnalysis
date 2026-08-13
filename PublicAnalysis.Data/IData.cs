using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PublicAnalysis.Data
{
    public interface IData
    {

        string Name { get; }
        Task<IEnumerable> Query(DataQuery dataSetQuery);
        DataMeta Meta { get; }
    }
}
