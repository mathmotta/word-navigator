using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BluePrism.WordNavigator.Core
{
    public class WordNavigationService
    {
        private ILogger<WordNavigationService> _log;

        public WordNavigationService(ILogger<WordNavigationService> log)
        {
            _log = log;
        }

        public ICollection<string> Seek(string start, string target, IEnumerable<string> lists)
        {
            throw new NotImplementedException();
        }
    }
}