using System.Collections;
using System.Runtime.CompilerServices;

namespace Public.Analysis.Data
{
    public interface IDataSet
    {
        public string Name { get; }
        IEnumerable<DataMeta> MetaData { get; }

        IData? this[string dataName] { get; }
    }
}
