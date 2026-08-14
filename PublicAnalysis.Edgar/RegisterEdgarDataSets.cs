using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Public.Frameworks.Initialization;
using PublicAnalysis.Data;
using PublicAnalysis.Edgar.TickerToCIK;

namespace PublicAnalysis.Edgar
{
    public static class EdgarRegistrations
    {
        public static IServiceCollection RegisterEdgarDataSet(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterPublicAnalysis();
            services.Configure<EdgarOptions>(configuration.GetSection(nameof(EdgarOptions)));
            services.Configure<TickerToCIKDataOptions>(configuration.GetSection(nameof(TickerToCIKDataOptions)));
                

            services.AddSingleton<HttpClient>();
            services.AddSingleton<RawFacts>();
            services.AddSingleton<TickerToCIKData>();
            services.AddTransient((sp) => {
                return sp.GetRequiredService<TickerToCIKData>() as IMustBeLoaded;
            });
            services.AddSingleton<IEnumerable<IEdgarData>>((sp) => {

                return typeof(EdgarRegistrations)
                .Assembly
                .GetTypes()
                .Where(t => typeof(IEdgarData).IsAssignableFrom(t) && t.IsClass)
                .Select(t =>
                {
                    return sp.GetRequiredService(t) as IEdgarData;
                }).ToList()!;
            });
            services.AddTransient<IDataSet, EdgarDataSet>();
            return services;
        }
    }
}
