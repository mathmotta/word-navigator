using BluePrism.WordNavigator.Common.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace BluePrism.WordNavigator.Common.Services.IO
{
    public class OnDemandFileService : IFileManagementService
    {
        private readonly ILogger<FileService> _log;

        public OnDemandFileService(ILogger<FileService> log)
        {
            _log = log;
        }

        public IEnumerable<string> ReadContent(string path)
        {
            if (path == null || string.IsNullOrEmpty(path))
                throw new ArgumentNullException("Path to file is null or empty");
            return File.ReadLines(path);
        }

        public IAsyncEnumerable<string> ReadContentAsync(string path)
        {
            if (path == null || string.IsNullOrEmpty(path))
                throw new ArgumentNullException("Path to file is null or empty");
            return FileExtensions.ReadLinesAsync(path);
        }
    }
}
