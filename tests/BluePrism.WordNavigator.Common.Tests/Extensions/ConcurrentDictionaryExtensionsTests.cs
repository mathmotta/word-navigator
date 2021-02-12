using BluePrism.WordNavigator.Common.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace BluePrism.WordNavigator.Common.Tests.Extensions
{
    public class ConcurrentDictionaryExtensionsTests
    {
        [Test]
        public void TryAddIf_PredicateTrue()
        {
            var dictionary = new ConcurrentDictionary<int, int>() { [1] = 1, [2] = 2 };
            dictionary.TryAddIf(3, 3, (k, v) => !dictionary.ContainsKey(5)); // Add if there's no key with value 5

            Assert.IsTrue(dictionary.ContainsKey(3));
        }

        [Test]
        public void TryAddIf_PredicateFalse()
        {
            var dictionary = new ConcurrentDictionary<int, int>() { [1] = 1, [2] = 2 };
            dictionary.TryAddIf(3, 3, (k, v) => !dictionary.ContainsKey(2)); // Add if there's no key with value 2

            Assert.IsFalse(dictionary.ContainsKey(3));
        }

        [Test]
        public void TryAddIf_ConditionTrue()
        {
            var dictionary = new ConcurrentDictionary<int, int>() { [1] = 1, [2] = 2 };
            dictionary.TryAddIf(3, 3, !dictionary.ContainsKey(5)); // Add if there's no key with value 5

            Assert.IsTrue(dictionary.ContainsKey(3));
        }

        [Test]
        public void TryAddIf_ConditionFalse()
        {
            var dictionary = new ConcurrentDictionary<int, int>() { [1] = 1, [2] = 2 };
            dictionary.TryAddIf(3, 3, !dictionary.ContainsKey(2)); // Add if there's no key with value 2

            Assert.IsFalse(dictionary.ContainsKey(3));
        }
    }
}
