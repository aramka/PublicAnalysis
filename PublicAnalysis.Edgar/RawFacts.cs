using PublicAnalysis.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PublicAnalysis.Edgar
{
    public class RawFacts : IData
    {
        public string Name => nameof(RawFacts);

        public DataMeta Meta => new DataMeta(this.Name, [new KeyValuePair<string, Type>("Ticker", typeof(string))], DataReturnTypesEnum.UserDefined);

        public Task<IEnumerable> Query(DataQuery dataSetQuery)
        {
            throw new NotImplementedException();
        }
    }
}
