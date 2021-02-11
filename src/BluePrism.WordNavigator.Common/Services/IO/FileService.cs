using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Common
{
    public class FileService : IFileManagementService
    {
        private readonly ILogger<FileService> _log;

        public FileService(ILogger<FileService> log)
        {
            _log = log;
        }

        public IEnumerable<string> ReadContent(string path)
        {
            if (path == null || string.IsNullOrEmpty(path))
                throw new ArgumentNullException("Path to file is null or empty");
            _log.LogDebug("Reading file {filePath}", path);
            return File.ReadAllLines(path);

        }

        public async IAsyncEnumerable<string> ReadContentAsync(string path)
        {
            if (path == null || string.IsNullOrEmpty(path))
                throw new ArgumentNullException("Path to file is null or empty");

            var lines = await File.ReadAllLinesAsync(path);
            foreach (var line in lines)
            {
                yield return await Task.FromResult(line);
            }
        }
    }
}