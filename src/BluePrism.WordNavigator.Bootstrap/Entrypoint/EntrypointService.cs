using BluePrism.WordNavigator.Bootstrap.Command;
using BluePrism.WordNavigator.Common.Exceptions;
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
                await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(o => ExecuteApplication(o, cancellationToken));
            }
            catch (UnauthorizedAccessException uae)
            {
                _log.LogError("Error reading from the provided path. Path is either read-only, is a directory or the current user does not have access permission. See: {message}", uae.Message);
            }
            catch (SecurityException se)
            {
                _log.LogError("The current user does not have the required permission for this action.");
            }
            catch (Exception e)
            {
                _log.LogError("An unhandled exception has occurred: {message}", e.Message);
                throw;
            }

        }

        /// <summary>
        /// Executes the application
        /// </summary>
        /// <param name="options">The <see cref="Options"/> to be used in the execution</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/></param>
        /// <returns>A task that represents the execution of the application.</returns>
        private async Task ExecuteApplication(Options options, CancellationToken cancellationToken = default)
        {
            _log.LogInformation("Word Navigator v{version}", string.Format("{0}.{1}",
               Assembly.GetEntryAssembly().GetName().Version.Major,
               Assembly.GetEntryAssembly().GetName().Version.Minor));
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var wordLength = _config.GetSection("WordLength");
            if (wordLength != null && !options.Start.Length.Equals(int.Parse(wordLength.Value)))
            {
                throw new LengthNotPermittedException(string.Format("Length of word {0} is different than the allowed value of {1}", options.Start, wordLength.Value));
            }

            string sourcePath;
            if (options.Dictionary != null && !string.IsNullOrEmpty(options.Dictionary))
            {
                sourcePath = options.Dictionary;
            }
            else
            {
                sourcePath = @"Resources\words-english.txt";
                _log.LogInformation("No dictionary path were selected, using default: {sourcePath}", sourcePath);
            }

            IAsyncEnumerable<string> content = _fileNavigationService.ReadContentAsync(sourcePath, cancellationToken);

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
                _log.LogInformation("No output file path was selected, using default: {outputPath}", outputPath);
            }

            _log.LogDebug("Writing result paths to {outputPath}", outputPath);
            await _fileNavigationService.WriteContentAsync(outputPath, shortestPathsDto, cancellationToken);

            stopWatch.Stop();
            if (shortestPaths.Count == 0)
            {
                _log.LogInformation("There are no paths for this word combination.");
            }
            else
            {
                _log.LogInformation("All shortest paths:\r\n{shortestPathsDto}", shortestPathsDto.ToString());
            }
            _log.LogInformation("The operation took {seconds} seconds", Math.Round(stopWatch.Elapsed.TotalSeconds, 3));

        }
    }
}
