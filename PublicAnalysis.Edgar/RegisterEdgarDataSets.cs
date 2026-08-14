using Microsoft.Extensions.DependencyInjection;
using PublicAnalysis.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicAnalysis.Edgar
{
    public static class EdgarRegistrations
    {
        public static IServiceCollection RegisterEdgarDataSet(this IServiceCollection services)
        {
            services.AddSingleton<RawFacts>();
            services.AddTransient<IDataSet, EdgarData>();
            return services;
        }
    }
}
