using BluePrism.WordNavigator.Core.DTO;
using BluePrism.WordNavigator.Core.Navigation;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Core.Tests.Navigation
{
    public class FileNavigationServiceTests
    {
        private IFileNavigationService _fileNavigationService;

        [SetUp]
        public void Setup()
        {
            var mockLogger = new Mock<ILogger<FileNavigationService>>();
            _fileNavigationService = new FileNavigationService(mockLogger.Object);
        }

        [Test]
        public void WriteContent_Success()
        {
            var input = new Stack<ICollection<string>>();
            var inputResult = new List<string>();
            inputResult.Add("spin");
            inputResult.Add("spit");
            inputResult.Add("spot");
            input.Push(inputResult);
            var shortestPaths = ShortestPathsDTO.CreateFrom(input);

            _fileNavigationService.WriteContent(@"Resources\result.txt", shortestPaths);
            Assert.IsTrue(File.Exists(@"Resources\result.txt"));
        }

        [Test]
        public async Task WriteContentAsync_Success()
        {
            var input = new Stack<ICollection<string>>();
            var inputResult = new List<string>();
            inputResult.Add("spin");
            inputResult.Add("spit");
            inputResult.Add("spot");
            input.Push(inputResult);
            var shortestPaths = ShortestPathsDTO.CreateFrom(input);

            await _fileNavigationService.WriteContentAsync(@"Resources\result.txt", shortestPaths);
            Assert.IsTrue(File.Exists(@"Resources\result.txt"));
        }

        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(@"Resources\result.txt"))
            {
                File.Delete(@"Resources\result.txt");
            }
        }
    }
}
