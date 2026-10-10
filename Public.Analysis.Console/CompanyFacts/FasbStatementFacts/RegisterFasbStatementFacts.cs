using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.CompanyFacts.FasbStatementFacts
{
    public static class RegisterFasbStatementFacts
    {
        public static IServiceCollection RegisterFasbTaxonmiesServices(this IServiceCollection services)
        {
            services.AddSingleton<IFasbStatementsData, FasbStatementJsonFileData>();
            services.AddSingleton<IStatementFactsService, FasbStatementFactsService>();
            return services;
        }
    }
}
