using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BluePrism.WordNavigator.Common.Tests
{
    public class FileServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReadContent_ReturnAllLines()
        {
            var fileService = new FileService();
            IEnumerable<string> result = fileService.ReadContent(@"Resources\words-english.txt");
            Assert.IsTrue(result.Count().Equals(26881));
        }
    }
}