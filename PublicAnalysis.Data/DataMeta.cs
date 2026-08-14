using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Data
{
    public class DataMeta
    {
        public string Name { get; }

        public IEnumerable<IEnumerable<string>> QueryMinExpectedPath { get; }

        public string ReturnType { get; }

        public DataMeta(string name, IEnumerable<KeyValuePair<string,Type>> queryMinExpectedPath, DataReturnTypesEnum dataReturnTypesEnum)
        {
            Name = name;
            QueryMinExpectedPath = queryMinExpectedPath.Select((kvp) => new string[] { kvp.Key, kvp.Value.FullName! });
            this.ReturnType = dataReturnTypesEnum.ToString();
        }
    }
}
