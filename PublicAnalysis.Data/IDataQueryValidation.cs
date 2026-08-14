using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Data
{
    public interface IDataQueryValidation
    {
        void ThrowIfNotValid(DataQuery dataSetQuery, DataMeta meta);
    }
}
