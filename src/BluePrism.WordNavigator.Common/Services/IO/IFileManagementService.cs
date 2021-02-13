using BluePrism.WordNavigator.Common.Services;
using System.Collections.Generic;
using System.Threading;

namespace BluePrism.WordNavigator.Common
{
    /// <summary>
    /// Exposes the File Management Service, which specifies functionality to interact with content.
    /// </summary>
    public interface IFileManagementService : IService
    {
        /// <summary>
        /// Returns an enumerable of lines items from a provided content path.
        /// </summary>
        /// <param name="path">The path to the file to be read.</param>
        /// <returns>An enumerable for iteration over the content.</returns>
        IEnumerable<string> ReadContent(string path);
        /// <summary>
        /// Returns an enumerable of lines items from a provided content path that can be iterated asynchronously .
        /// </summary>
        /// <param name="path">The path to the file to be read</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/></param>
        /// <returns>An enumerable for asynchronous iteration over the content.</returns>
        IAsyncEnumerable<string> ReadContentAsync(string path, CancellationToken cancellationToken = default);
    }
}