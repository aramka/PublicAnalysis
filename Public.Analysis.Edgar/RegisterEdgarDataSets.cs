using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Public.Frameworks.Initialization;
using Public.Analysis.Data;
using Public.Analysis.Edgar.TickerToCIK;
using Public.Analysis.Edgar.RawFacts;
using Public.Analysis.Edgar.Models;
using Public.Frameworks.JsonQuery;

namespace Public.Analysis.Edgar
{
    public static class EdgarRegistrations
    {
        public const string PublicAnalysisEdgarOptionsPrefix = "Public.Analysis.Edgar";

        private static string GetSectionName(string optionsClassName) => $"{PublicAnalysisEdgarOptionsPrefix}.{optionsClassName}";
		public static IServiceCollection RegisterEdgarDataSet(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterPublicAnalysis();
            services.Configure<EdgarOptions>(configuration.GetSection(GetSectionName(nameof(EdgarOptions))));
            services.Configure<TickerToCIKDataOptions>(configuration.GetSection(GetSectionName(nameof(TickerToCIKDataOptions))));
            services.Configure<FactsDataOptions>(configuration.GetSection(GetSectionName(nameof(FactsDataOptions))));

            services.AddSingleton<HttpClient>();
            
            services.AddSingleton<RawFactsData>();
            services.AddSingleton<IEdgarData>((sp) => sp.GetRequiredService<RawFactsData>());

            services.AddSingleton<TickerToCIKData>();
            services.AddSingleton<ITickerToCIKData>((sp) => sp.GetRequiredService<TickerToCIKData>());
            services.AddSingleton<IEdgarData>((sp) => sp.GetRequiredService<TickerToCIKData>());
            services.AddSingleton((sp) => {
                return sp.GetRequiredService<TickerToCIKData>() as IMustBeLoaded;
            });

            services.AddSingleton<JsonQueryBuilder>();
            services.AddSingleton<IJsonQueryBuilder>((sp) => sp.GetRequiredService<JsonQueryBuilder>());

            services.AddSingleton<JsonPathJsonQuery>();
            services.AddSingleton<IJsonQuery>((sp) => sp.GetRequiredService<JsonPathJsonQuery>());

            services.AddTransient<IDataSet, EdgarDataSet>();
            return services;
        }
    }
}
