using Public.Analysis.Data;
using Public.Analysis.Edgar.TickerToCIK;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;

namespace Public.Analysis.Edgar
{
    public class EdgarDataSet : IDataSet
    {
        private Dictionary<string, IEdgarData> dataSets;

        public string Name { get; } = nameof(EdgarDataSet);

        public IEnumerable<DataMeta> MetaData => this.dataSets.Select(d => d.Value.Meta);

        public IData? this[string dataName]
        {
            get
            {
                this.dataSets.TryGetValue(dataName, out IEdgarData? value);

                return value;
            }
        }

        public EdgarDataSet(IEnumerable<IEdgarData> edgarDataSets)
        {
            this.dataSets = edgarDataSets.ToDictionary(ds => ds.Name);
        }
    }
}
