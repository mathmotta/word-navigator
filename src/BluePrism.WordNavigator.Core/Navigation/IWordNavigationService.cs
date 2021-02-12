using BluePrism.WordNavigator.Common.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Core
{
    /// <summary>
    /// <para>Exposes the default Word Navigation Service which specifies functionality to seek and find a <b>target</b> string from phased modifications of a <b>start</b> string.</para>
    /// <para>Each phase may only change one char in the string, creating a similarity, and such similarity must exist in the provided <b>source</b></para>
    /// <para>E.g. start is 'same', target is 'cost', should result in all shortest paths, one of them being [same -> came -> case -> cast -> cost]</para>
    /// </summary>
    public interface IWordNavigationService
    {
        /// <summary>
        /// Seeks for a given target string, running from a given start string. All similarities must be part of the given source.
        /// </summary>
        /// <param name="start">The start string</param>
        /// <param name="target">The target string</param>
        /// <param name="source">A concurrent hash set source, where all strings should be present</param>
        /// <returns>A collection of shortest paths.</returns>
        Task<ICollection<ICollection<string>>> Seek(string start, string target, ConcurrentHashSet<string> source);
        /// <summary>
        /// Seeks for a given target string, running from a given start string. All similarities must be part of the given source.
        /// </summary>
        /// <param name="start">The start string</param>
        /// <param name="target">The target string</param>
        /// <param name="source">An asynchronous enumerable source, where all strings should be present</param>
        /// <returns>A collection of shortest paths.</returns>
        Task<ICollection<ICollection<string>>> Seek(string start, string target, IAsyncEnumerable<string> source);
        /// <summary>
        /// Seeks for a given target string, running from a given start string. All similarities must be part of the given source.
        /// </summary>
        /// <param name="start">The start string</param>
        /// <param name="target">The target string</param>
        /// <param name="source">An enumerable source, where all strings should be present</param>
        /// <returns>A collection of shortest paths.</returns>
        Task<ICollection<ICollection<string>>> Seek(string start, string target, IEnumerable<string> source);
    }
}