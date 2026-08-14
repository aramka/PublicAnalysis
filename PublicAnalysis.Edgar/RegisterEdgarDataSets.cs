using Microsoft.Extensions.DependencyInjection;
using Public.Frameworks.Initialization;
using PublicAnalysis.Data;
using PublicAnalysis.Edgar.TickerToCIK;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace PublicAnalysis.Edgar
{
    public static class EdgarRegistrations
    {
        public static IServiceCollection RegisterEdgarDataSet(this IServiceCollection services)
        {
            services.RegisterPublicAnalysis();

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
