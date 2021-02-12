using BluePrism.WordNavigator.Bootstrap.Command;
using BluePrism.WordNavigator.Common;
using BluePrism.WordNavigator.Core.DTO;
using BluePrism.WordNavigator.Core.Navigation;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Bootstrap
{
    public class EntrypointService : IEntrypointService
    {
        private readonly ILogger<EntrypointService> _log;
        private readonly IConfiguration _config;
        private readonly IFileNavigationService _fileNavigationService;
        private readonly IWordNavigationService _wordNavigationService;

        public EntrypointService(ILogger<EntrypointService> log, IConfiguration config, IFileNavigationService fileNavigationService, IWordNavigationService wordNavigationService)
        {
            _log = log;
            _config = config;
            _fileNavigationService = fileNavigationService;
            _wordNavigationService = wordNavigationService;
        }

        public async Task Execute(string[] args, CancellationToken cancellationToken = default)
        {
            try
            {
                _log.LogInformation("Word Navigator v{major}.{minor}", 
                    Assembly.GetEntryAssembly().GetName().Version.Major,
                    Assembly.GetEntryAssembly().GetName().Version.Minor);
                await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(o => ExecuteApplication(o, cancellationToken));
            }
            catch (UnauthorizedAccessException uae)
            {
                _log.LogError("Error reading from the provided path. Path is either read-only, is a directory or the current user does not have access permission. See: {message}", uae.Message);
                throw;
            }
            catch (SecurityException se)
            {
                _log.LogError("The current user does not have the required permission for this action.");
                throw;
            }
            catch (AggregateException e)
            {
                _log.LogError("An unhandled exception has occurred: {message}", e.Message);
                throw;
            }

        }

        private async Task ExecuteApplication(Options options, CancellationToken cancellationToken = default)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            string sourcePath;
            if (options.Dictionary != null && !string.IsNullOrEmpty(options.Dictionary))
            {
                sourcePath = options.Dictionary;
            }
            else
            {
                sourcePath = @"Resources\words-english.txt";
            }

            IAsyncEnumerable<string> content = _fileNavigationService.ReadContentAsync(options.Dictionary, cancellationToken);

            //_log.LogDebug("Seeking for {target}", options.Target);
            ICollection<ICollection<string>> shortestPaths = await _wordNavigationService.Seek(options.Start, options.Target, content, cancellationToken);
            var shortestPathsDto = ShortestPathsDTO.CreateFrom(shortestPaths);

            string outputPath;
            if (options.Output != null && !string.IsNullOrEmpty(options.Output))
            {
                outputPath = options.Output;
            }
            else
            {
                outputPath = @"results.txt";
            }

            _log.LogDebug("Writing result paths to {outputPath}", outputPath);
            await _fileNavigationService.WriteContentAsync(outputPath, shortestPathsDto, cancellationToken);

            stopWatch.Stop();
            _log.LogInformation("All shortest paths:\r\n{shortestPathsDto}", shortestPathsDto.ToString());
            _log.LogInformation("The operation took {seconds} seconds", Math.Round(stopWatch.Elapsed.TotalSeconds, 3));

        }
    }
}
