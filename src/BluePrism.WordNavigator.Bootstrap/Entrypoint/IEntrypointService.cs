using BluePrism.WordNavigator.Common.Services;
using System.Threading;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Bootstrap
{
    /// <summary>
    /// Exposes the entrypoint service use to run the main application
    /// </summary>
    public interface IEntrypointService : IService
    {
        /// <summary>
        /// Executes the main application streamline
        /// </summary>
        /// <param name="args">args to be used in the application</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/></param>
        Task Execute(string[] args, CancellationToken cancellationToken = default);
    }
}