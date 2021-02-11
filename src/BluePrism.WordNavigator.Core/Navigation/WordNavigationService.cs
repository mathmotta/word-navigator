using BluePrism.WordNavigator.Common.Concurrent;
using BluePrism.WordNavigator.Common.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }
    }
}