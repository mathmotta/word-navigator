using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace BluePrism.WordNavigator.Common.Extensions
{
    /// <summary>
    /// <para>The FileExtensions is an unorthodox extension class for <see cref="File"/> objects. It does not actually extend it as the original class is static.</para>
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// <para>Reads the lines of a file asynchronously.</para>
        /// <para>An asynchronous equivalent to <see cref="File.ReadLines"/></para>
        /// <para>This method should be deprecated in a future version as there are official implementation plans in a future .NET version.</para>
        /// See <a href="https://github.com/dotnet/runtime/issues/2214">this proposal</a> for more information
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<string> ReadLinesAsync(string path, CancellationToken cancellationToken = default)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read,
                FileShare.Read, 32768, FileOptions.Asynchronous | FileOptions.SequentialScan);
            using var sr = new StreamReader(fs);
            while (!cancellationToken.IsCancellationRequested)
            {
                var line = await sr.ReadLineAsync().ConfigureAwait(false);
                if (line == null)
                {
                    break;
                }
                yield return line;
            }
        }
    }
}
