using BluePrism.WordNavigator.Common.Services.IO;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Common.Tests
{
    public class OnDemandFileServiceTests
    {
        [Test]
        public void ReadContent_ReturnLines_Success()
        {
            var mockLogger = new Mock<ILogger<FileService>>();
            var fileService = new OnDemandFileService(mockLogger.Object);
            IEnumerable<string> result = fileService.ReadContent(@"Resources\words-english.txt");
            Assert.IsTrue(result.Count().Equals(26880));
        }

        [Test]
        public void ReadContent_ReturnLines_FailNullPath()
        {
            var mockLogger = new Mock<ILogger<FileService>>();
            var fileService = new OnDemandFileService(mockLogger.Object);
            Assert.Throws<ArgumentNullException>(() => fileService.ReadContent(null));
        }

        [Test]
        public void ReadContent_ReturnLines_FailEmptyPath()
        {
            var mockLogger = new Mock<ILogger<FileService>>();
            var fileService = new OnDemandFileService(mockLogger.Object);
            Assert.Throws<ArgumentNullException>(() => fileService.ReadContent(string.Empty));
        }

        [Test]
        public void ReadContent_ReturnLines_FailFileNotFound()
        {
            var mockLogger = new Mock<ILogger<FileService>>();
            var fileService = new OnDemandFileService(mockLogger.Object);
            Assert.Throws<FileNotFoundException>(() => fileService.ReadContent(@"Resources\non-existent.txt"));
        }

        [Test]
        public async Task ReadContentAsync_ReturnLines_Success()
        {
            var mockLogger = new Mock<ILogger<FileService>>();
            var fileService = new OnDemandFileService(mockLogger.Object);
            IAsyncEnumerable<string> result = fileService.ReadContentAsync(@"Resources\words-english.txt");

            int count = 0;
            await foreach (var str in result)
            {
                count++;
            }
            Assert.IsTrue(count.Equals(26880));
        }

    }
}