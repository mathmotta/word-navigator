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
        private FileService _fileService;

        [SetUp]
        public void Setup()
        {
            var mockLogger = new Mock<ILogger<FileService>>();
            _fileService = new FileService(mockLogger.Object);
        }

        [Test]
        public void ReadContent_ReturnAllLines_Success()
        {

            IEnumerable<string> result = _fileService.ReadContent(@"Resources\words-english.txt");
            Assert.IsTrue(result.Count().Equals(26880));
        }

        [Test]
        public void ReadContent_ReturnAllLines_FailNullPath()
        {
            Assert.Throws<ArgumentNullException>(() => _fileService.ReadContent(null));
        }

        [Test]
        public void ReadContent_ReturnAllLines_FailEmptyPath()
        {
            Assert.Throws<ArgumentNullException>(() => _fileService.ReadContent(string.Empty));
        }

        [Test]
        public void ReadContent_ReturnAllLines_FailFileNotFound()
        {
            Assert.Throws<FileNotFoundException>(() => _fileService.ReadContent(@"Resources\non-existent.txt"));
        }

        [Test]
        public async Task ReadContentAsync_ReturnAllLines_Success()
        {
            IAsyncEnumerable<string> result = _fileService.ReadContentAsync(@"Resources\words-english.txt");

            int count = 0;
            await foreach (var str in result)
            {
                count++;
            }
            Assert.IsTrue(count.Equals(26880));
        }

    }
}