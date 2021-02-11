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
    public class FileServiceTest
    {
        [Test]
        public void ReadContent_ReturnAllLines_Success()
        {
            var mockLogger = new Mock<ILogger<FileService>>();
            var fileService = new FileService(mockLogger.Object);
            IEnumerable<string> result = fileService.ReadContent(@"Resources\words-english.txt");
            Assert.IsTrue(result.Count().Equals(26880));
        }

        [Test]
        public void ReadContent_ReturnAllLines_FailNullPath()
        {
            var mockLogger = new Mock<ILogger<FileService>>();
            var fileService = new FileService(mockLogger.Object);
            Assert.Throws<ArgumentNullException>(() => fileService.ReadContent(null));
        }

        [Test]
        public void ReadContent_ReturnAllLines_FailEmptyPath()
        {
            var mockLogger = new Mock<ILogger<FileService>>();
            var fileService = new FileService(mockLogger.Object);
            Assert.Throws<ArgumentNullException>(() => fileService.ReadContent(string.Empty));
        }

        [Test]
        public void ReadContent_ReturnAllLines_FailFileNotFound()
        {
            var mockLogger = new Mock<ILogger<FileService>>();
            var fileService = new FileService(mockLogger.Object);
            Assert.Throws<FileNotFoundException>(() => fileService.ReadContent(@"Resources\non-existent.txt"));
        }

        [Test]
        public async Task ReadContentAsync_ReturnAllLines_Success()
        {
            var mockLogger = new Mock<ILogger<FileService>>();
            var fileService = new FileService(mockLogger.Object);
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