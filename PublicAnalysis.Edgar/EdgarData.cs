using PublicAnalysis.Data;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;

namespace PublicAnalysis.Edgar
{
    public class EdgarData : IDataSet
    {
        private Dictionary<string, IData> dataSets;

        public string Name { get; } = nameof(EdgarData);

        public IEnumerable<DataMeta> MetaData => this.dataSets.Select(d => d.Value.Meta);

        public IData? this[string dataName]
        {
            get
            {
                this.dataSets.TryGetValue(dataName, out IData? value);

                return value;
            }
        }

        public EdgarData(RawFacts raw)
        {
            this.dataSets = new Dictionary<string, IData>
            {
                {
                    raw.Name,
                    raw
                }
            };
        }
    }
}
