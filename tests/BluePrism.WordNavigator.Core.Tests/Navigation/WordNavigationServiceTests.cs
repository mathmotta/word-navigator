using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BluePrism.WordNavigator.Common.Concurrent;
using BluePrism.WordNavigator.Common.Extensions;

namespace BluePrism.WordNavigator.Core.Tests
{
    public class WordNavigationServiceTests
    {
        private Mock<WordNavigationService> _wordNavigationService;

        [SetUp]
        public void Setup()
        {
            var mockLogger = new Mock<ILogger<WordNavigationService>>();
            _wordNavigationService = new Mock<WordNavigationService>(mockLogger.Object);
            _wordNavigationService.CallBase = true;
        }

        [Test]
        public async Task Seek_IEnumerable_Success()
        {
            var source = new List<string>() { "cost", "came", "same", "cast", "case" };
            var start = "same";
            var target = "cost";

            ICollection<ICollection<string>> expectedResult = new List<ICollection<string>>();
            ICollection<string> path = new List<string>() { "same", "came", "case", "cast", "cost" };
            expectedResult.Add(path);

            
            _wordNavigationService.Setup(w => w.Seek(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConcurrentHashSet<string>>()).Result).Returns(expectedResult);
            ICollection<ICollection<string>> result = await _wordNavigationService.Object.Seek(start, target, source);

            
            Assert.IsTrue(expectedResult.Count == 1);
            Assert.IsTrue(expectedResult.First().Count == 5);
            _wordNavigationService.Verify(m => m.Seek(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConcurrentHashSet<string>>()), Times.Once);
        }

        [Test]
        public async Task Seek_IAsyncEnumerable_Success()
        {
            var source = new List<string>() { "cost", "came", "same", "cast", "case" };
            var start = "same";
            var target = "cost";

            ICollection<ICollection<string>> expectedResult = new List<ICollection<string>>();
            ICollection<string> path = new List<string>() { "same", "came", "case", "cast", "cost" };
            expectedResult.Add(path);


            _wordNavigationService.Setup(w => w.Seek(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConcurrentHashSet<string>>()).Result).Returns(expectedResult);
            ICollection<ICollection<string>> result = await _wordNavigationService.Object.Seek(start, target, source.ToAsyncEnumerable());


            Assert.IsTrue(expectedResult.Count == 1);
            Assert.IsTrue(expectedResult.First().Count == 5);
            _wordNavigationService.Verify(m => m.Seek(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConcurrentHashSet<string>>()), Times.Once);
        }
    }
}