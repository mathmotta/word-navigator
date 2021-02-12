using BluePrism.WordNavigator.Common.Extensions;
using NUnit.Framework;
using System.Collections.Generic;

namespace BluePrism.WordNavigator.Common.Tests.Extensions
{
    public class CollectionsExtesionsTests
    {
        [Test]
        public void AddIf_PredicateTrue()
        {
            var list = new List<int>() { 1, 2 };
            list.AddIf(3, e => !list.Contains(5));

            Assert.IsTrue(list.Contains(3));
        }

        [Test]
        public void AddIf_PredicateFalse ()
        {
            var list = new List<int>() { 1, 2 };
            list.AddIf(3, e => !list.Contains(2));

            Assert.IsFalse(list.Contains(3));
        }

        [Test]
        public void AddIf_ConditionTrue()
        {
            var list = new List<int>() { 1, 2 };
            list.AddIf(3, !list.Contains(5));

            Assert.IsTrue(list.Contains(3));
        }

        [Test]
        public void AddIf_ConditionFalse()
        {
            var list = new List<int>() { 1, 2 };
            list.AddIf(3, !list.Contains(2));

            Assert.IsFalse(list.Contains(3));
        }
    }
}
