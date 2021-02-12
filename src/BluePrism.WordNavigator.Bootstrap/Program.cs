using BluePrism.WordNavigator.Common;
using BluePrism.WordNavigator.Common.Services.IO;
using BluePrism.WordNavigator.Core;
using BluePrism.WordNavigator.Core.Navigation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Bootstrap
{
    public class Program
    {
        static async Task Main(string[] args)
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
                    services.AddTransient<IFileNavigationService, FileNavigationService>();
                    services.AddTransient<IWordNavigationService, WordNavigationService>();

                }).UseSerilog()
                .Build();
            
            var entrypoint = ActivatorUtilities.CreateInstance<EntrypointService>(host.Services);

            // Setup a cancelation token to safely stop the process when CTRL+C is pressed
            var cancellationToken = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Log.Logger.Warning("The operation has been cancelled by the user.");
                cancellationToken.Cancel();
                eventArgs.Cancel = true;
            };

            args = new string[] { "fool", "sage", "-d", @"Resources\words-english.txt", "-o", @"C:\Users\mathewsmotta\Documents\WordNavigatorResults\resultpaths.txt" };

            // Starts the application
            await entrypoint.Execute(args, cancellationToken.Token);
        }

        private static void BuildConfiguration(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);

        }
    }
}
