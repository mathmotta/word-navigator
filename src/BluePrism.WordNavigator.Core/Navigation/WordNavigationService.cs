using BluePrism.WordNavigator.Common.Concurrent;
using BluePrism.WordNavigator.Common.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Core.Navigation
{
    public class WordNavigationService : IWordNavigationService
    {
        private ILogger<WordNavigationService> _log;

        public WordNavigationService(ILogger<WordNavigationService> log)
        {
            _log = log;
        }

        public virtual async Task<Stack<ICollection<string>>> Seek(string start, string target, IEnumerable<string> source, CancellationToken cancellationToken = default)
        {
            ConcurrentHashSet<string> notKnown = source.ToConcurrentHashSet();
            return await Seek(start, target, notKnown, cancellationToken);
        }

        public virtual async Task<Stack<ICollection<string>>> Seek(string start, string target, IAsyncEnumerable<string> source, CancellationToken cancellationToken = default)
        {
            ConcurrentHashSet<string> notKnown = await source.ToConcurrentHashSet();
            return await Seek(start, target, notKnown, cancellationToken);
        }

        public virtual async Task<Stack<ICollection<string>>> Seek(string start, string target, ConcurrentHashSet<string> source, CancellationToken cancellationToken = default)
        {
            source.Remove(start);

            _log.LogDebug("Started seeking with start {start} and target {target}.", start, target);
            var similarityGroups = new ConcurrentDictionary<string, ICollection<string>>();
            bool foundTarget = await SeekTarget(start, target, source, similarityGroups);
            var researchResult = new Stack<ICollection<string>>();
            if (!foundTarget)
            {
                _log.LogDebug("Target was not found. No results to be shown.");
                source.Dispose();
                return researchResult;
            }
            _log.LogDebug("Target found. Getting all paths next.");
            var paths = new List<string> { start };
            SeekAllShortestPaths(start, target, similarityGroups.ToDictionary(kv => kv.Key, kv => kv.Value.ToList()), researchResult, paths);
            _log.LogDebug("Finalized seek.");
            source.Dispose();
            return researchResult;
        }

        /// <summary>
        /// Seeks a target string, starting from a start string, with a given grouped similarities that must be part of a given source
        /// </summary>
        /// <param name="start">The start string to search from</param>
        /// <param name="target">The target string to find</param>
        /// <param name="source">The allowed contents that the search can be done at</param>
        /// <param name="similarityGroups">The similarity groups that were already found</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/></param>
        /// <returns>True if the target is found</returns>
        public virtual async Task<bool> SeekTarget(string start, string target, ConcurrentHashSet<string> source, ConcurrentDictionary<string, ICollection<string>> similarityGroups, CancellationToken cancellationToken = default)
        {
            var pathsContent = new List<string>();
            pathsContent.Add(start);
            var foundTarget = false;
            while (pathsContent.Any() && !foundTarget && !cancellationToken.IsCancellationRequested)
            {
                var alreadyKnown = new ConcurrentHashSet<string>();
                var tasks = new List<Task<bool>>();
                var length = pathsContent.Count;
                _log.LogDebug("Iterating all paths seeking for target. Size is {length}", length);
                for (int i = 0; i < length; i++)
                {
                    var group = pathsContent.First();
                    pathsContent.Remove(group);

                    tasks.Add(Task.Run(() => IterateLetters(group, target, source, similarityGroups, alreadyKnown, pathsContent)));
                }
                bool[] foundInSeries = await Task.WhenAll(tasks);
                if (foundInSeries.Any(f => f))
                {
                    foundTarget = true;
                }

                source.RemoveWhere(alreadyKnown.Contains);
            }
            return foundTarget;
        }

        /// <summary>
        /// Iterate over the letters of a given word to find similarities
        /// </summary>
        /// <param name="group">The group where the similarity will belong to</param>
        /// <param name="target">The target word to be found</param>
        /// <param name="source">The allowed contents that the search can be done at</param>
        /// <param name="similarityGroups">The similarity groups that were already found</param>
        /// <param name="alreadyKnown">The already found words</param>
        /// <param name="pathsContent">List of valid paths</param>
        /// <returns>True if the target is found</returns>
        public virtual bool IterateLetters(string group, string target, ConcurrentHashSet<string> source, ConcurrentDictionary<string, ICollection<string>> similarityGroups, ConcurrentHashSet<string> alreadyKnown, ICollection<string> pathsContent)
        {
            if (group == null)
            {
                _log.LogDebug("Group is null, skipping word.");
                return false;
            }
            _log.LogDebug("Iterating letters in word: {group}", group);
            var found = false;
            for (int l = 0; l < group.Length; l++)
            {
                _log.LogDebug("Finding similarities for word {group} changing letter at position {l}", group, l);
                bool foundInCurrent = FindAndMatchSimilarities(l, group, target, source, similarityGroups, alreadyKnown, pathsContent);
                if (foundInCurrent)
                {
                    _log.LogDebug("Target was found!");
                    found = true;
                }
            }
            return found;
        }

        /// <summary>
        /// Match a similarity with a group, if applicable
        /// </summary>
        /// <param name="index">The index letter to change a word</param>
        /// <param name="group">The group where similarities will be added to</param>
        /// <param name="target">The target word to be found</param>
        /// <param name="source">The allowed contents that the search can be done at</param>
        /// <param name="similarityGroups">The similarity groups that were already found</param>
        /// <param name="alreadyKnown">The already found words</param>
        /// <param name="pathsContent">List of valid paths</param>
        /// <returns>True if target is found</returns>
        public virtual bool FindAndMatchSimilarities(int index, string group, string target, ConcurrentHashSet<string> source, ConcurrentDictionary<string, ICollection<string>> similarityGroups, ConcurrentHashSet<string> alreadyKnown, ICollection<string> pathsContent)
        {
            var targetFound = false;

            for (char k = 'a'; k <= 'z'; k++)
            {
                string similarity = SubstituteLetter(group, index, k);
                if (!source.Contains(similarity))
                {
                    _log.LogTrace("Similarity {similarity} does not exist in source, skipping.", similarity);
                    continue;
                }
                _log.LogDebug("Adding similarity {similarity} to the list of known elements.", similarity);
                AddSimilarityToGroup(group, similarity, similarityGroups);
                pathsContent.AddIf(similarity, !alreadyKnown.Contains(similarity));
                alreadyKnown.Add(similarity);
                if (similarity.Equals(target))
                {
                    _log.LogInformation("Target {target} was found from similarity of {group}!", target, group);
                    targetFound = true;
                }
            }
            return targetFound;
        }

        /// <summary>
        /// Substitue a letter in a string.
        /// </summary>
        /// <param name="str">The string to be modified</param>
        /// <param name="indexToChange">The index of the string to be substituted</param>
        /// <param name="letter">The letter to be used in the substitution</param>
        /// <returns>A string similar to the given one, with a single changed letter</returns>
        public virtual string SubstituteLetter(string str, int indexToChange, char letter)
        {
            var chars = str.ToCharArray();
            chars[indexToChange] = letter;
            return new string(chars);
        }

        /// <summary>
        /// Adds a similarity to a group
        /// </summary>
        /// <typeparam name="T">The type that is been worked with</typeparam>
        /// <param name="group">The group to add a similarity to</param>
        /// <param name="similarity">The similarity to be added</param>
        /// <param name="similarityGroups">The similarity groups that were already found</param>
        public virtual void AddSimilarityToGroup<T>(T group, T similarity, ConcurrentDictionary<T, ICollection<T>> similarityGroups)
        {
            ICollection<T> similarities = similarityGroups.GetValueOrDefault(group, new List<T>());
            similarities.Add(similarity);
            similarityGroups.TryAddIf(group, similarities, !similarityGroups.ContainsKey(group));
        }

        /// <summary>
        /// Recursivelly finds all shortest paths from a list of groups and its similarities
        /// </summary>
        /// <param name="start">The start word</param>
        /// <param name="target">The word to be found</param>
        /// <param name="similarityGroups">The similarity groups to find the shortest path from</param>
        /// <param name="researchResult">The result to write to</param>
        /// <param name="paths">The constructed paths to be added to the result</param>
        public virtual void SeekAllShortestPaths(string start, string target, Dictionary<string, List<string>> similarityGroups, Stack<ICollection<string>> researchResult, ICollection<string> paths)
        {
            if (start.Equals(target))
            {
                researchResult.Push(new List<string>(paths));
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