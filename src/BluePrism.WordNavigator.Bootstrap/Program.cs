using BluePrism.WordNavigator.Common;
using BluePrism.WordNavigator.Common.Services.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;

namespace BluePrism.WordNavigator.Bootstrap
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfiguration(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            var fileServiceDependency = builder.Build().GetSection("FileManagementService").Value;
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IEntrypointService, EntrypointService>();
                    switch (fileServiceDependency)
                    {
                        case "OnDemandFileService":
                            services.AddTransient<IFileManagementService, OnDemandFileService>();
                            break;
                        default:
                            services.AddTransient<IFileManagementService, FileService>();
                            break;
                    }
                    
                }).UseSerilog()
                .Build();
            
            var entrypoint = ActivatorUtilities.CreateInstance<EntrypointService>(host.Services);

            // Starts the application
            entrypoint.Execute(args);
        }

        private static void BuildConfiguration(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

        }
    }
}
