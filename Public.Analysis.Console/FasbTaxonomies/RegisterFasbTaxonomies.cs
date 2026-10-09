using Microsoft.Extensions.DependencyInjection;
using Public.Analysis.Console.FinancialStatements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Console.FasbTaxonomies
{
    public static class RegisterFasbTaxonomies
    {
        public static IServiceCollection RegisterFasbTaxonmiesServices(this IServiceCollection services)
        {
            services.AddSingleton<IStatementService, StatementService>();
            return services;
        }
    }
}
