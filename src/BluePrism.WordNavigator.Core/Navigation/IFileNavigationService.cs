using BluePrism.WordNavigator.Core.DTO;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Security;
using BluePrism.WordNavigator.Common;

namespace BluePrism.WordNavigator.Core.Navigation
{
    /// <summary>
    /// Exposes functionality to save navigation results to text files.
    /// </summary>
    public interface IFileNavigationService : IFileManagementService
    {
        /// <summary>
        /// Writes navigation results from <see cref="ShortestPathsDTO"/> to a file located at the given path.
        /// </summary>
        /// <param name="path">The file to save the results.</param>
        /// <param name="shortestPaths">The navigation results.</param>
        /// <exception cref="ArgumentException">path is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.InvalidPathChars"/>.</exception>
        /// <exception cref="ArgumentNullException">path is null.</exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="SecurityException"></exception>
        void WriteContent(string path, ShortestPathsDTO shortestPaths)
        {
            File.WriteAllText(path, shortestPaths.ToString());
        }

        /// <summary>
        /// Asynchronously writes navigation results from <see cref="ShortestPathsDTO"/> to a file located at the given path.
        /// </summary>
        /// <param name="path">The file to save the results.</param>
        /// <param name="shortestPaths">The navigation results.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        async Task WriteContentAsync(string path, ShortestPathsDTO shortestPaths, CancellationToken cancellationToken = default)
        {
            await File.WriteAllTextAsync(path, shortestPaths.ToString(), default);
        }
    }
}
