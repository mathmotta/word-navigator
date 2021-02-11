using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;

namespace BluePrism.WordNavigator.Core.Tests
{
    public class WordNavigationServiceTests
    {
        private WordNavigationService _wordNavigationService;

        [SetUp]
        public void Setup()
        {
            var mockLogger = new Mock<ILogger<WordNavigationService>>();
            _wordNavigationService = new WordNavigationService(mockLogger.Object);
        }

        [Test]
        public void Seek_Success()
        {
            var source = new string[] { "cost", "came", "same", "cast", "case" };
            var start = "same";
            var target = "cost";

            ICollection<string> result = _wordNavigationService.Seek(start, target, source.ToList());

            
            var expected = new string[] { "same", "came", "case", "cast", "cost" };
            Assert.AreEqual(expected, result);
        }
    }
}