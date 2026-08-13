namespace Public.Analysis.Console
{
    using PublicAnalysis.Data;
    using PublicAnalysis.Edgar;
    using System;
    using System.Collections;
    using System.Text.Json;

    internal class Program
    {
        static async Task Main(string[] args)
        {
            var dataSets = new Dictionary<string, IDataSet>();
            var edgarRawFacts = new EdgarData();
            dataSets.Add(edgarRawFacts.Name, edgarRawFacts);
            string prompt = $"Enter a path or type quit to stop:";
            Console.WriteLine(prompt);

            string? path = Console.ReadLine();
            IDataSet? dataSet = null;

            while (path?.ToLower() is not null && path is not "quit")
            {
                var segments = path.Split('/');

                if (!segments.Any())
                {

                }
                else if (segments[0] == "datasets")
                {
                    if (segments.Length == 1) //show me all the datasets
                    {
                        Console.WriteLine($"{JsonSerializer.Serialize(dataSets.Select(kvp => kvp.Key))}");
                    }
                    else if (segments.Length == 2 && dataSets.TryGetValue(segments[1], out dataSet)) //show info about the datas available in a particular dataset
                    {
                        Console.WriteLine(JsonSerializer.Serialize(dataSet.MetaData));
                    }
                    else if (dataSet is not null && dataSet[segments[2]] is not null) //show me the data for a particular datasets data
                    {
                        var restOfPath = segments[2..];
                        var dataSetQuery = new DataQuery(restOfPath);
                        IEnumerable data = await dataSet[segments[2]]!.Query(dataSetQuery);

                        Console.WriteLine(JsonSerializer.Serialize(data));
                    }

                    Console.WriteLine(prompt);

                    path = Console.ReadLine();
                }

            }
        }
    }
}
