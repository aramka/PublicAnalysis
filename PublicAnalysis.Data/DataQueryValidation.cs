namespace PublicAnalysis.Data
{
    public class DataQueryValidation : IDataQueryValidation
    {
        public void ThrowIfNotValid(DataQuery dataSetQuery, DataMeta meta)
        {
            if(meta is null)
            {
                throw new ArgumentNullException(nameof(meta));
            }
            if(meta.QueryMinExpectedPath is null)
            {
                throw new ArgumentNullException(nameof(meta.QueryMinExpectedPath));
            }
            if (dataSetQuery is null)
            {
                throw new ArgumentNullException(nameof(dataSetQuery));
            }
            if (dataSetQuery.Path is null)
            {
                throw new ArgumentNullException(nameof(dataSetQuery.Path));
            }

            if(dataSetQuery.Path.Length< meta.QueryMinExpectedPath.Count())
            {
                throw new ArgumentException($"{nameof(dataSetQuery.Path)} must have at least {meta.QueryMinExpectedPath.Count()} segments");
            }

        }
    }
}