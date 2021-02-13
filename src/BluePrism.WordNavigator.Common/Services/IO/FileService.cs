using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Security;
using System.Threading;

namespace BluePrism.WordNavigator.Common.Services.IO
{
    /// <summary>
    /// <para>The File Service provides functionality to load all content in memory and returns an enumerable of lines that can be iterated.</para> 
    /// <para>This service is meant to work with smaller file sizes and is not scalable.</para>
    /// <para>To work with bigger files, see <see cref="OnDemandFileService"/> instead.</para>
    /// </summary>
    public class FileService : IFileManagementService
    {
        private readonly ILogger<FileService> _log;

        public FileService(ILogger<FileService> log)
        {
            _log = log;
        }

        /// <summary>
        /// Loads the full content in memory synchronously and returns an enumerable of lines items from a provided content path.
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
            {
                throw new ArgumentNullException("Path to file is null or empty");
            }
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The file does not exist at the provided file path.");
            }
            _log.LogDebug("Reading file {path}", path);
            return File.ReadAllLines(path);

        }

        /// <summary>
        /// Loads the full content in memory asynchronously and returns an enumerable of lines items from a provided content path that can be iterated asynchronously .
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        /// <param name="path">The path to the file to be read</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>An enumerable for asynchronous iteration over the content.</returns>
        public async IAsyncEnumerable<string> ReadContentAsync(string path, CancellationToken cancellationToken = default)
        {
            if (path == null || string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("Path to file is null or empty");
            }
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("The file does not exist at the provided file path.");
            }

            _log.LogDebug("Reading file {path}", path);
            var lines = await File.ReadAllLinesAsync(path);
            _log.LogDebug("Making AsyncEnumerable from result");
            foreach (var line in lines)
            {
                yield return await Task.FromResult(line);
            }
        }
    }
}