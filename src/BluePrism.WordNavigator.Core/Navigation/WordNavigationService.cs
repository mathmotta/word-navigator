using BluePrism.WordNavigator.Common.Concurrent;
using BluePrism.WordNavigator.Common.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Core
{
    public class WordNavigationService
    {
        private ILogger<WordNavigationService> _log;

        public WordNavigationService(ILogger<WordNavigationService> log)
        {
            _log = log;
        }

        public virtual async Task<ICollection<ICollection<string>>> Seek(string start, string target, IEnumerable<string> source)
        {
            ConcurrentHashSet<string> notKnown = source.ToConcurrentHashSet();
            notKnown.Remove(start);

            return await Seek(start, target, notKnown);
        }

        public virtual async Task<ICollection<ICollection<string>>> Seek(string start, string target, IAsyncEnumerable<string> source)
        {
            ConcurrentHashSet<string> notKnown = await source.ToConcurrentHashSet();
            notKnown.Remove(start);

            return await Seek(start, target, notKnown);
        }

        public virtual async Task<ICollection<ICollection<string>>> Seek(string start, string target, ConcurrentHashSet<string> notKnown)
        {
            var similarityGroups = new ConcurrentDictionary<string, ICollection<string>>();
            bool foundTarget = await SeekTarget(start, target, notKnown, similarityGroups);
            var researchResult = new List<ICollection<string>>();
            if (!foundTarget)
                return researchResult;
            var paths = new List<string> { start };
            SeekAllShortestPaths(start, target, similarityGroups.ToDictionary(kv => kv.Key, kv => kv.Value.ToList()), researchResult, paths);

            return researchResult;
        }

        public virtual async Task<bool> SeekTarget(string start, string target, ConcurrentHashSet<string> notKnown, ConcurrentDictionary<string, ICollection<string>> similarityGroups)
        {
            var pathsContent = new List<string>();
            pathsContent.Add(start);
            var foundTarget = false;
            while (pathsContent.Any() && !foundTarget)
            {
                var alreadyKnown = new ConcurrentHashSet<String>();
                var tasks = new List<Task<bool>>();
                var length = pathsContent.Count;
                for (int i = 0; i < length; i++)
                {
                    var group = pathsContent.First();
                    pathsContent.Remove(group);
                    tasks.Add(Task.Run(() => IterateLetters(group, target, notKnown, similarityGroups, alreadyKnown, pathsContent)));
                }
                bool[] foundInSeries = await Task.WhenAll(tasks);
                if (foundInSeries.Any(f => f))
                {
                    foundTarget = true;
                }

                notKnown.RemoveWhere(alreadyKnown.Contains);
            }
            return foundTarget;
        }

        public virtual bool IterateLetters(string group, string target, ConcurrentHashSet<string> notKnown, ConcurrentDictionary<string, ICollection<string>> similarityGroups, ConcurrentHashSet<string> alreadyKnown, ICollection<string> pathsContent)
        {
            if (group == null)
                return false;
            var found = false;
            for (int l = 0; l < group.Length; l++)
            {
                bool foundInCurrent = FindAndMatchSimilarities(l, group, target, notKnown, similarityGroups, alreadyKnown, pathsContent);
                if (foundInCurrent)
                {
                    found = true;
                }
            }
            return found;
        }

        public virtual bool FindAndMatchSimilarities(int index, string group, string target, ConcurrentHashSet<string> notKnown, ConcurrentDictionary<string, ICollection<string>> similarityGroups, ConcurrentHashSet<string> alreadyKnown, ICollection<string> pathsContent)
        {
            var targetFound = false;
            for (char k = 'a'; k <= 'z'; k++)
            {
                string similarity = SubstituteLetter(group, index, k);
                if (!notKnown.Contains(similarity))
                {
                    continue;
                }
                AddSimilarityToGroup(group, similarity, similarityGroups);
                pathsContent.AddIf(similarity, !alreadyKnown.Contains(similarity));
                alreadyKnown.Add(similarity);
                if (similarity.Equals(target))
                {
                    targetFound = true;
                }
            }
            return targetFound;
        }

        public virtual string SubstituteLetter(string str, int indexToChange, char letter)
        {
            var chars = str.ToCharArray();
            chars[indexToChange] = letter;
            return new string(chars);
        }

        public virtual void AddSimilarityToGroup<T>(T group, T similarity, ConcurrentDictionary<T, ICollection<T>> similarityGroups)
        {
            ICollection<T> similarities = similarityGroups.GetValueOrDefault(group, new List<T>());
            similarities.Add(similarity);
            similarityGroups.TryAddIf(group, similarities, !similarityGroups.ContainsKey(group));
        }

        public virtual void SeekAllShortestPaths(string start, string target, Dictionary<string, List<string>> similarityGroups, ICollection<ICollection<string>> researchResult, ICollection<string> paths)
        {
            if (start.Equals(target))
            {
                researchResult.Add(new List<string>(paths));
                return;
            }

            List<string> similarityForStart = similarityGroups.GetValueOrDefault(start);
            if (similarityForStart == null)
            {
                return;
            }
            foreach (var word in similarityForStart)
            {
                paths.Add(word);
                SeekAllShortestPaths(word, target, similarityGroups, researchResult, paths);
                paths.Remove(paths.Last());
            }
        }
    }
}