using System.Collections.Generic;
using System.IO;

namespace BluePrism.WordNavigator.Common.Extensions
{
    public static class FileExtensions
    {
        public static async IAsyncEnumerable<string> ReadLinesAsync(string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read,
                FileShare.Read, 32768, FileOptions.Asynchronous | FileOptions.SequentialScan);
            using var sr = new StreamReader(fs);
            while (true)
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
