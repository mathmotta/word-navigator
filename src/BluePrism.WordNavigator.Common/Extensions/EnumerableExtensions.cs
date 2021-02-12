using BluePrism.WordNavigator.Common.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Common.Extensions
{
    /// <summary>
    /// Expose extension methods to <see cref="IEnumerable{T}"/> types.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Creates a <see cref="ConcurrentHashSet{T}"/> from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type to be used.</typeparam>
        /// <param name="source">The source with the original contents.</param>
        /// <returns>An equivalent <see cref="ConcurrentHashSet{T}"/>.</returns>
        public static ConcurrentHashSet<TSource> ToConcurrentHashSet<TSource>(this IEnumerable<TSource> source)
        {
            var hashSet = new ConcurrentHashSet<TSource>();
            foreach (TSource element in source)
            {
                hashSet.Add(element);
            }
            return hashSet;
        }

        /// <summary>
        /// Creates an <see cref="IAsyncEnumerable{T}"/> from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type to be used.</typeparam>
        /// <param name="source">The source with the original contents.</param>
        /// <returns>An equivalent <see cref="IAsyncEnumerable{T}"/>.</returns>
        public static async IAsyncEnumerable<TSource> ToAsyncEnumerable<TSource>(this IEnumerable<TSource> source)
        {
            foreach (var element in source)
            {
                yield return await Task.FromResult(element);
            }
        }
    }
}
