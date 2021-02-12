﻿using BluePrism.WordNavigator.Common.Services.IO;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Core.Tests.Navigation
{
    public class WordNavigationServiceIT
    {
        private WordNavigationService _wordNavigationService;
        private OnDemandFileService _fileService;

        [SetUp]
        public void Setup()
        {
            var mockLoggerNavigation = new Mock<ILogger<WordNavigationService>>();
            _wordNavigationService = new WordNavigationService(mockLoggerNavigation.Object);

            var mockLoggerOnDemanFileService = new Mock<ILogger<OnDemandFileService>>();
            _fileService = new OnDemandFileService(mockLoggerOnDemanFileService.Object);
        }

        [Test]
        public async Task IntegrationTest_Same_To_Cost()
        {
            var start = "spin";
            var target = "spot";
            IAsyncEnumerable<string> source = _fileService.ReadContentAsync(@"Resources\words-english.txt");

            ICollection<ICollection<string>> result = await _wordNavigationService.Seek(start, target, source);

            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.First().Count == 5);
        }
    }
}