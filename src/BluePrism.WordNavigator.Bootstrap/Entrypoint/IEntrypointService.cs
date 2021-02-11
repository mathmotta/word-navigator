using BluePrism.WordNavigator.Common.Services;

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
        void Execute(string[] args);
    }
}