using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Public.Analysis.Console.CompanyFacts.DerivedFacts.TangibleBookValue;

namespace Public.Analysis.Console.CompanyFacts.DerivedFacts
{
    public static class RegisterDerivedFacts
    {
        public static IServiceCollection RegisterDerivedFactsServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterTangibleBookValueServices(configuration);
            services.AddSingleton<IDerivedFactsService, DerivedFactsService>();
            return services;
        }
    }
}
