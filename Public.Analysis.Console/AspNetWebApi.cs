namespace Public.Analysis.Console
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Public.Frameworks.Initialization;
    using Public.Analysis.Data;
    using Public.Analysis.Edgar;
    using System.Collections.Generic;
    using Microsoft.OpenApi.Models;
    using Public.Analysis.Edgar.RawFacts;
    using Public.Analysis.Console.FasbTaxonomies;
    using Public.Analysis.Console.CompanyFacts;

    internal class AspNetWebApi
    {
        internal static async Task StartWebApi(string[] args)
        {
            // build configuration first so we can register the same services into the web host's DI
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // --- start minimal Web API in background ---
            var webBuilder = WebApplication.CreateBuilder(args);

            // make the configuration available to the web builder
            webBuilder.Configuration.AddConfiguration(configuration);

            webBuilder.Services.AddControllers();
            // TODO: lock down CORS
            webBuilder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader());
            });
            // optional: swagger for quick testing
            webBuilder.Services.AddEndpointsApiExplorer();
            webBuilder.Services.AddSwaggerGen();

            // register the application services into the same IServiceCollection used by the web host
            Startup(webBuilder.Services, configuration);

            var app = webBuilder.Build();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Public.Analysis API V1");
                c.RoutePrefix = string.Empty; // serve UI at "/"
            });
            // apply CORS policy before routing to controllers
            app.UseCors("AllowAll");
            app.MapControllers();

            // --- existing startup work (now resolved from the app's service provider) ---
            var mustBeLoaded = app.Services.GetRequiredService<IEnumerable<IMustBeLoaded>>();

            foreach (var iMustBeLoaded in mustBeLoaded)
            {
                await iMustBeLoaded.Load();
            }

            await app.RunAsync();

            // graceful shutdown of web host
            await app.StopAsync();
        }

        static void Startup(IServiceCollection services, IConfiguration configuration)
        {
            // bind StatementTreeControllerOptions from configuration so controllers can receive IOptions<>
            services.Configure<StatementTreeControllerOptions>(
                configuration.GetSection("Public.Analysis.Console.FasbTaxonomies.StatementTreeControllerOptions"));

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddConfiguration(configuration.GetSection("Logging"));
            });

            services.AddSingleton<IConfiguration>(configuration);
            services.RegisterEdgarDataSet(configuration);
            services.RegisterCompanyFacts();
            services.AddSingleton(provider =>
            {
                var dataSets = provider.GetRequiredService<IEnumerable<IDataSet>>().ToDictionary(ds => ds.Name);
                return dataSets;
            });
        }
    }
}
