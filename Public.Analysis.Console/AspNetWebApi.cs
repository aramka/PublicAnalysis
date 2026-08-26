namespace Public.Analysis.Console
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Public.Frameworks.Initialization;
    using Public.Analysis.Data;
    using Public.Analysis.Edgar;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Json;

    internal class AspNetWebApi
    {
        internal static async Task StartWebApi(string[] args)
        {
            var serviceProvider = Startup();

            // --- start minimal Web API in background ---
            var webBuilder = WebApplication.CreateBuilder(args);
            webBuilder.Services.AddControllers();
            // optional: swagger for quick testing
            webBuilder.Services.AddEndpointsApiExplorer();
            webBuilder.Services.AddSwaggerGen();

            var app = webBuilder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.MapControllers();

             // --- existing startup work (unchanged) ---
            var mustBeLoaded = serviceProvider.GetRequiredService<IEnumerable<IMustBeLoaded>>();

            foreach (var iMustBeLoaded in mustBeLoaded)
            {
                await iMustBeLoaded.Load();
            }

            await app.RunAsync();

           

            // graceful shutdown of web host
            await app.StopAsync();

            serviceProvider.Dispose();
        }

        static ServiceProvider Startup()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();


            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddConfiguration(configuration.GetSection("Logging"));
            });

            services.AddSingleton<IConfiguration>(configuration);
            services.RegisterEdgarDataSet(configuration);
            services.AddSingleton(provider =>
            {
                var dataSets = provider.GetRequiredService<IEnumerable<IDataSet>>().ToDictionary(ds => ds.Name);
                return dataSets;
            });

            return services.BuildServiceProvider();
        }
    }
}
