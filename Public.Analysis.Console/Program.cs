
namespace Public.Analysis.Console
{ 
    using System.Collections;
    using System.Text.Json;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Public.Frameworks.Initialization;
    using Public.Analysis.Data;
    using Public.Analysis.Edgar;
    using Console = System.Console;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public class Program
    {

        public static async Task Main(string[] args)
        {
            if (args.Any())
            {

            }
            else
            {
                await InteractiveConsoleApp.StartConsole();
            }
        }

    }
}
