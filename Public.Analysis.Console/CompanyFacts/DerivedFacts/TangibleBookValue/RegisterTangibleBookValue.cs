using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Public.Analysis.Console.CompanyFacts.DerivedFacts.TangibleBookValue
{
    public static class RegisterTangibleBookValue
    {
        public static IServiceCollection RegisterTangibleBookValueServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TangibleBookValueOptions>(configuration.GetSection(TangibleBookValueOptions.ConfigSectionName));
            services.AddSingleton<IDerivedFact, TangibleBookValue>();
            return services;
        }
    }
}
