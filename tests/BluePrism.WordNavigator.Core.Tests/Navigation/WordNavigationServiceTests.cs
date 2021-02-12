using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using BluePrism.WordNavigator.Common.Concurrent;
using BluePrism.WordNavigator.Common.Extensions;
using System.Collections.Concurrent;
using System;
using System.IO;
using BluePrism.WordNavigator.Core.Navigation;
using System.Threading;

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


            _wordNavigationService.Setup(w => w.Seek(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConcurrentHashSet<string>>(), CancellationToken.None).Result).Returns(expectedResult);
            ICollection<ICollection<string>> result = await _wordNavigationService.Object.Seek(start, target, source);


            Assert.IsTrue(expectedResult.Count == 1);
            Assert.IsTrue(expectedResult.First().Count == 5);
            _wordNavigationService.Verify(m => m.Seek(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConcurrentHashSet<string>>(), CancellationToken.None), Times.Once);
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


            _wordNavigationService.Setup(w => w.Seek(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConcurrentHashSet<string>>(), CancellationToken.None).Result).Returns(expectedResult);
            ICollection<ICollection<string>> result = await _wordNavigationService.Object.Seek(start, target, source.ToAsyncEnumerable());


            Assert.IsTrue(expectedResult.Count == 1);
            Assert.IsTrue(expectedResult.First().Count == 5);
            _wordNavigationService.Verify(m => m.Seek(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConcurrentHashSet<string>>(), CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task Seek_ConcurrentHashSet_FoundTrue()
        {
            var source = new List<string>();
            ConcurrentHashSet<string> hashSet = await source.ToAsyncEnumerable().ToConcurrentHashSet();

            _wordNavigationService.Setup(w => w.SeekTarget(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ConcurrentHashSet<string>>(),
                It.IsAny<ConcurrentDictionary<string, ICollection<string>>>(),
                CancellationToken.None).Result)
                .Returns(true);
            _wordNavigationService.Setup(w => w.SeekAllShortestPaths(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, List<string>>>(),
                It.IsAny<ICollection<ICollection<string>>>(),
                It.IsAny<ICollection<string>>()));

            ICollection<ICollection<string>> result = await _wordNavigationService.Object.Seek(It.IsAny<string>(), It.IsAny<string>(), hashSet);

            _wordNavigationService.Verify(w => w.SeekAllShortestPaths(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, List<string>>>(),
                It.IsAny<ICollection<ICollection<string>>>(),
                It.IsAny<ICollection<string>>()), Times.Once);
        }

        [Test]
        public async Task Seek_ConcurrentHashSet_FoundFalse()
        {
            var source = new List<string>();
            ConcurrentHashSet<string> hashSet = await source.ToAsyncEnumerable().ToConcurrentHashSet();

            _wordNavigationService.Setup(w => w.SeekTarget(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ConcurrentHashSet<string>>(),
                It.IsAny<ConcurrentDictionary<string, ICollection<string>>>(), 
                CancellationToken.None).Result)
                .Returns(false);
            _wordNavigationService.Setup(w => w.SeekAllShortestPaths(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, List<string>>>(),
                It.IsAny<ICollection<ICollection<string>>>(),
                It.IsAny<ICollection<string>>()));

            ICollection<ICollection<string>> result = await _wordNavigationService.Object.Seek(It.IsAny<string>(), It.IsAny<string>(), hashSet);

            _wordNavigationService.Verify(w => w.SeekAllShortestPaths(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, List<string>>>(),
                It.IsAny<ICollection<ICollection<string>>>(),
                It.IsAny<ICollection<string>>()), Times.Never);
        }

        [Test]
        public void IterateLetters_GroupNotNull_FoundInCurrentTrue()
        {
            _wordNavigationService.Setup(w => w.FindAndMatchSimilarities(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ConcurrentHashSet<string>>(),
                It.IsAny<ConcurrentDictionary<string, ICollection<string>>>(),
                It.IsAny<ConcurrentHashSet<string>>(),
                It.IsAny<ICollection<string>>()))
                .Returns(true);

            bool result = _wordNavigationService.Object.IterateLetters("a", It.IsAny<string>(), It.IsAny<ConcurrentHashSet<string>>(), It.IsAny<ConcurrentDictionary<string, ICollection<string>>>(), It.IsAny<ConcurrentHashSet<string>>(), It.IsAny<ICollection<string>>());

            Assert.IsTrue(result);
        }

        [Test]
        public void IterateLetters_GroupNotNull_FoundInCurrentFalse()
        {
            _wordNavigationService.Setup(w => w.FindAndMatchSimilarities(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ConcurrentHashSet<string>>(),
                It.IsAny<ConcurrentDictionary<string, ICollection<string>>>(),
                It.IsAny<ConcurrentHashSet<string>>(),
                It.IsAny<ICollection<string>>()))
                .Returns(false);

            bool result = _wordNavigationService.Object.IterateLetters("a", It.IsAny<string>(), It.IsAny<ConcurrentHashSet<string>>(), It.IsAny<ConcurrentDictionary<string, ICollection<string>>>(), It.IsAny<ConcurrentHashSet<string>>(), It.IsAny<ICollection<string>>());

            Assert.IsFalse(result);
        }

        [Test]
        public void IterateLetters_GroupEmpty()
        {
            bool result = _wordNavigationService.Object.IterateLetters("", It.IsAny<string>(), It.IsAny<ConcurrentHashSet<string>>(), It.IsAny<ConcurrentDictionary<string, ICollection<string>>>(), It.IsAny<ConcurrentHashSet<string>>(), It.IsAny<ICollection<string>>());

            Assert.IsFalse(result);
            _wordNavigationService.Verify(w => w.FindAndMatchSimilarities(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ConcurrentHashSet<string>>(),
                It.IsAny<ConcurrentDictionary<string, ICollection<string>>>(),
                It.IsAny<ConcurrentHashSet<string>>(),
                It.IsAny<ICollection<string>>()), Times.Never);
        }

        [Test]
        public void IterateLetters_GroupNull()
        {
            bool result = _wordNavigationService.Object.IterateLetters(null, It.IsAny<string>(), It.IsAny<ConcurrentHashSet<string>>(), It.IsAny<ConcurrentDictionary<string, ICollection<string>>>(), It.IsAny<ConcurrentHashSet<string>>(), It.IsAny<ICollection<string>>());

            Assert.IsFalse(result);
            _wordNavigationService.Verify(w => w.FindAndMatchSimilarities(
                It.IsAny<int>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ConcurrentHashSet<string>>(),
                It.IsAny<ConcurrentDictionary<string, ICollection<string>>>(),
                It.IsAny<ConcurrentHashSet<string>>(),
                It.IsAny<ICollection<string>>()), Times.Never);
        }


        [Test]
        public void FindAndMatchSimilarities_NotKnownContains_TargetFoundTrue()
        {
            _wordNavigationService.Setup(w => w.SubstituteLetter(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<char>()))
                .Returns("case");

            _wordNavigationService.Setup(w => w.AddSimilarityToGroup(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ConcurrentDictionary<string, ICollection<string>>>()));

            ConcurrentHashSet<string> notKnown = new ConcurrentHashSet<string>();
            notKnown.Add("case");

            bool result = _wordNavigationService.Object.FindAndMatchSimilarities(
                It.IsAny<int>(),
                "came",
                "case",
                notKnown,
                new ConcurrentDictionary<string, ICollection<string>>(),
                new ConcurrentHashSet<string>(),
                new List<string>());

            Assert.IsTrue(result);
        }

        [Test]
        public void FindAndMatchSimilarities_NotKnownDoesntContains()
        {
            _wordNavigationService.Setup(w => w.SubstituteLetter(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<char>()))
                .Returns(Guid.NewGuid().ToString());

            _wordNavigationService.Setup(w => w.AddSimilarityToGroup(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ConcurrentDictionary<string, ICollection<string>>>()));

            bool result = _wordNavigationService.Object.FindAndMatchSimilarities(
                It.IsAny<int>(),
                "came",
                "case",
                new ConcurrentHashSet<string>(),
                new ConcurrentDictionary<string, ICollection<string>>(),
                new ConcurrentHashSet<string>(),
                new List<string>());

            Assert.IsFalse(result);
            _wordNavigationService.Verify(w => w.AddSimilarityToGroup(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ConcurrentDictionary<string, ICollection<string>>>()),
                Times.Never);
        }

        [Test]
        public void FindAndMatchSimilarities_NotKnownContains_TargetFoundFalse()
        {
            _wordNavigationService.Setup(w => w.SubstituteLetter(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<char>()))
                .Returns("case");

            _wordNavigationService.Setup(w => w.AddSimilarityToGroup(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ConcurrentDictionary<string, ICollection<string>>>()));

            ConcurrentHashSet<string> notKnown = new ConcurrentHashSet<string>();
            notKnown.Add("case");

            bool result = _wordNavigationService.Object.FindAndMatchSimilarities(
                It.IsAny<int>(),
                "came",
                "cage",
                notKnown,
                new ConcurrentDictionary<string, ICollection<string>>(),
                new ConcurrentHashSet<string>(),
                new List<string>());

            Assert.IsFalse(result);
        }

        [Test]
        public void SubstituteLetter_Success()
        {
            var originalStr = "came";
            var indexToChange = 2;
            var letter = 's';

            string result = _wordNavigationService.Object.SubstituteLetter(originalStr, indexToChange, letter);

            Assert.AreEqual("case", result);
        }

        [Test]
        public void AddSimilarityToGroup_NewSimilarity_GroupNotExistent()
        {
            var group = "came";
            var similarity = "cage";
            var similarityGroups = new ConcurrentDictionary<string, ICollection<string>>();

            _wordNavigationService.Object.AddSimilarityToGroup(group, similarity, similarityGroups);

            Assert.IsTrue(similarityGroups.Count == 1);
            Assert.IsTrue(similarityGroups[group].Contains(similarity));
        }

        [Test]
        public void AddSimilarityToGroup_NewSimilarity_GroupExists()
        {
            var group = "came";
            var similarity = "cage";
            var similarityGroups = new ConcurrentDictionary<string, ICollection<string>>();
            similarityGroups.TryAdd(group, new List<string>() { "case" });

            _wordNavigationService.Object.AddSimilarityToGroup(group, similarity, similarityGroups);

            Assert.IsTrue(similarityGroups.Count == 1);
            Assert.IsTrue(similarityGroups[group].Count == 2);
            Assert.IsTrue(similarityGroups[group].Contains(similarity));
        }

        [Test]
        public void AddSimilarityToGroup_NewSimilarity_OtherTypes()
        {
            var group = new object();
            var similarity = new object();
            var similarityGroups = new ConcurrentDictionary<object, ICollection<object>>();

            _wordNavigationService.Object.AddSimilarityToGroup(group, similarity, similarityGroups);

            Assert.IsTrue(similarityGroups.Count == 1);
            Assert.IsTrue(similarityGroups[group].Contains(similarity));
        }

        public void SeekAllShortestPaths_StartEqualsTarget()
        {
            var group = "case";
            var target = "case";
            var researchResult = new List<ICollection<string>>();

            _wordNavigationService.Object.SeekAllShortestPaths(
                group, 
                target, 
                It.IsAny<Dictionary<string, List<string>>>(),
                researchResult,
                It.IsAny<ICollection<string>>());

            Assert.IsTrue(researchResult.Count == 1);
            Assert.IsTrue(researchResult.First().Count == 1);
        }

        public void SeekAllShortestPaths_ReturnEmptyResult()
        {
            var group = "case";
            var target = "cage";
            var researchResult = new List<ICollection<string>>();

            _wordNavigationService.Object.SeekAllShortestPaths(
                group,
                target,
                It.IsAny<Dictionary<string, List<string>>>(),
                researchResult,
                It.IsAny<ICollection<string>>());

            Assert.IsTrue(researchResult.Count == 0);
        }

        public void SeekAllShortestPaths_ReturnAllShortPaths()
        {
            var group = "same";
            var target = "cost";
            var similarityGroups = new Dictionary<string, List<string>>();
            similarityGroups.Add("case", new List<string>() { "cast" });
            similarityGroups.Add("cast", new List<string>() { "cost" });
            similarityGroups.Add("same", new List<string>() { "came" });
            similarityGroups.Add("came", new List<string>() { "case" });
            var researchResult = new List<ICollection<string>>();

            _wordNavigationService.Object.SeekAllShortestPaths(
                group,
                target,
                similarityGroups,
                new List<ICollection<string>>(),
                new List<string>());

            Assert.IsTrue(researchResult.Count == 1);
            Assert.IsTrue(researchResult.First().Count == 5);
        }


        [Test]
        public async Task Seek_IAsyncEnumerableasd_Success()
        {
            var source = new List<string>() { "cost", "came", "same", "cast", "case" };
            var start = "same";
            var target = "cost";

            ICollection<string> expectedResult = new List<string>() { "same", "came", "case", "cast", "cost" };


            //IEnumerable<string> source = File.ReadAllLines(@"Resources\words-english.txt");

            
            ICollection<ICollection<string>> result = await _wordNavigationService.Object.Seek(start, target, source);


            Assert.IsTrue(expectedResult.Count == 5);
            _wordNavigationService.Verify(m => m.Seek(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ConcurrentHashSet<string>>(), CancellationToken.None), Times.Once);
        }
    }
}