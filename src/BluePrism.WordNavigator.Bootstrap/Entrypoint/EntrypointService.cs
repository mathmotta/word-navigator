using BluePrism.WordNavigator.Bootstrap.Command;
using BluePrism.WordNavigator.Common;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BluePrism.WordNavigator.Bootstrap
{
    public class EntrypointService : IEntrypointService
    {
        private readonly ILogger<EntrypointService> _log;
        private readonly IConfiguration _config;
        private readonly IFileManagementService _fileManagementService;

        public EntrypointService(ILogger<EntrypointService> log, IConfiguration config, IFileManagementService fileManagementService)
        {
            _log = log;
            _config = config;
            _fileManagementService = fileManagementService;
        }

        public async void Execute(string[] args)
        {
            var file = _fileManagementService;

            //Parser.Default.ParseArguments<Options>(args)
            //    .WithParsedAsync<Options>(o =>
            //    {
            //        return Task.;
            //    })
            //    .WithNotParsedAsync<Options>(o =>
            //    {
            //        return;
            //    });
        }
    }
}
