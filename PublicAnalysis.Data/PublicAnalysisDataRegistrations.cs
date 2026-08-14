using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Public.Analysis.Data
{
    public static class PublicAnalysisDataRegistrations
    {
        public static IServiceCollection RegisterPublicAnalysis(this IServiceCollection serviceProvider)
        {
            serviceProvider.AddSingleton<IDataQueryValidation, DataQueryValidation>();
            return serviceProvider;
        }
    }
}
