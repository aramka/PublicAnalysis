
namespace Public.Analysis.Console
{
    using System.Collections;
    using System.Text.Json;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Public.Frameworks.Initialization;
    using Public.Analysis.Data;
    using Public.Analysis.Edgar;
    using Console = System.Console;

    public class Program
    {

        public static async Task Main(string[] args)
        {
            var serviceProvider = Startup();

            var mustBeLoaded = serviceProvider.GetRequiredService<IEnumerable<IMustBeLoaded>>();

            foreach(var iMustBeLoaded in mustBeLoaded)
            {
                await iMustBeLoaded.Load();
            }

            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            var dataSets = serviceProvider.GetRequiredService<Dictionary<string, IDataSet>>();

            try
            {
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
                            var restOfPath = segments[3..];
                            var dataSetQuery = new DataQuery(restOfPath);
                            IEnumerable data = await dataSet[segments[2]]!.Query(dataSetQuery);

                            Console.WriteLine(JsonSerializer.Serialize(data));
                        }

                        Console.WriteLine(prompt);

                        path = Console.ReadLine();
                    }

                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred in the application");
            }
            finally
            {
                serviceProvider.Dispose();
            }
        }

        static ServiceProvider Startup()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();


            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddConfiguration(configuration.GetSection("Logging"));
            });

            services.AddSingleton<IConfiguration>(configuration);
            services.RegisterEdgarDataSet(configuration);
            services.AddSingleton(provider =>
            {
                var dataSets = provider.GetRequiredService<IEnumerable<IDataSet>>().ToDictionary(ds => ds.Name);
                return dataSets;
            });

            return services.BuildServiceProvider();
        }
    }
}
