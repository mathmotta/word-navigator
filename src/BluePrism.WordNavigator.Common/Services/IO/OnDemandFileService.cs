using BluePrism.WordNavigator.Common.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Threading;

namespace BluePrism.WordNavigator.Common.Services.IO
{
    /// <summary>
    /// <para>Provides functionality to load content in memory and returns an enumerable of lines that can be iterated.</para> 
    /// </summary>
    public class OnDemandFileService : IFileManagementService
    {
        private readonly ILogger<OnDemandFileService> _log;

        public OnDemandFileService(ILogger<OnDemandFileService> log)
        {
            _log = log;
        }

        /// <summary>
        /// Loads the content in memory synchronously and returns an enumerable of lines items from a provided content path.
        /// </summary>
        /// <exception cref="ArgumentException" />
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="PathTooLongException" />
        /// <exception cref="DirectoryNotFoundException" />
        /// <exception cref="IOException" />
        /// <exception cref="UnauthorizedAccessException" />
        /// <exception cref="FileNotFoundException" />
        /// <exception cref="NotSupportedException" />
        /// <exception cref="SecurityException" />
        /// <param name="path">The path to the file to be read.</param>
        /// <returns>An enumerable for iteration over the content.</returns>
        public IEnumerable<string> ReadContent(string path)
        {
            if (path == null || string.IsNullOrEmpty(path))
                throw new ArgumentNullException("Path to file is null or empty");
            _log.LogDebug("Reading file {filePath}", path);
            return File.ReadLines(path);
        }

        /// <summary>
        /// Loads the content in memory asynchronously and returns an enumerable of lines items from a provided content path that can be iterated asynchronously .
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <param name="path">The path to the file to be read</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/></param>
        /// <returns>An enumerable for asynchronous iteration over the content.</returns>
        public IAsyncEnumerable<string> ReadContentAsync(string path, CancellationToken cancellationToken = default)
        {
            if (path == null || string.IsNullOrEmpty(path))
                throw new ArgumentNullException("Path to file is null or empty");
            _log.LogDebug("Reading file {filePath}", path);
            return FileExtensions.ReadLinesAsync(path, cancellationToken);
        }
    }
}
