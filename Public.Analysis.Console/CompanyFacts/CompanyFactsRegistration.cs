using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Public.Analysis.Console.CompanyFacts
{
    public static class CompanyFactsRegistration
    {
        public static IServiceCollection RegisterCompanyFacts(this IServiceCollection services)
        {

            services.AddSingleton<ICompanyFactsService, CompanyFactsService>();
            services.AddSingleton<IFactsData, FactsDataJsonQuery>();
            
            return services;
        }
    }
}
