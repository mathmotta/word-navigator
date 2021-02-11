using BluePrism.WordNavigator.Bootstrap.Command;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BluePrism.WordNavigator.Bootstrap
{
    public class EntrypointService : IEntrypointService
    {
        private readonly ILogger<EntrypointService> _log;
        private readonly IConfiguration _config;

        public EntrypointService(ILogger<EntrypointService> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        public async void Execute(string[] args)
        {
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
