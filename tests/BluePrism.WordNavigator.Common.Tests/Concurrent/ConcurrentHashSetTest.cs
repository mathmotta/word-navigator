using BluePrism.WordNavigator.Common.Concurrent;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BluePrism.WordNavigator.Common.Tests.Concurrent
{
    public class ConcurrentHashSetTest
    {
        [Test]
        public void Add_Success()
        {
            var hashSet1 = new HashSet<string>();
            var hashSet = new ConcurrentHashSet<string>();
            Parallel.For(0, 50000, i =>
            {
                hashSet.Add(i.ToString());
            });

            var count = 0;
            foreach (var index in hashSet)
            {
                count++;
            }

            Assert.AreEqual(50000, count);
        }

        [Test]
        public void Remove_Success()
        {
            var hashSet = new ConcurrentHashSet<string>();
            Parallel.For(0, 50000, i =>
            {
                hashSet.Add(i.ToString());
            });


            Parallel.For(0, 50000, i =>
            {
                if (i % 2 == 0)
                {
                    hashSet.Remove(i.ToString());
                }
            });

            var count = 0;
            foreach (var index in hashSet)
            {
                count++;
            }
            Assert.AreEqual(25000, count);

        }

        [Test]
        public void RemoveWhere_Success()
        {
            var hashSet = new ConcurrentHashSet<string>();
            Parallel.For(0, 50000, i =>
            {
                hashSet.Add(i.ToString());
            });


            Parallel.For(0, 50000, i =>
            {
                if (i % 2 == 0)
                {
                    hashSet.RemoveWhere(ind => ind.Equals(i.ToString()));
                }
            });

            var count = 0;
            foreach (var index in hashSet)
            {
                count++;
            }
            Assert.AreEqual(25000, count);

        }

        [Test]
        public void Clear_Success()
        {
            var hashSet = new ConcurrentHashSet<string>();
            Parallel.For(0, 10, i =>
            {
                hashSet.Add(i.ToString());
            });

            hashSet.Clear();

            var count = 0;
            foreach (var index in hashSet)
            {
                count++;
            }
            Assert.AreEqual(0, count);

        }


        [Test]
        public void Contains_Success()
        {
            var hashSet = new ConcurrentHashSet<string>();
            Parallel.For(0, 10, i =>
            {
                hashSet.Add(i.ToString());
            });

            bool result = hashSet.Contains(5.ToString());

            Assert.IsTrue(result);

        }

        [Test]
        public void Count_Success()
        {
            var hashSet = new ConcurrentHashSet<string>();
            Parallel.For(0, 10, i =>
            {
                hashSet.Add(i.ToString());
            });

            var count = 0;
            foreach (var index in hashSet)
            {
                count++;
            }
            Assert.AreEqual(hashSet.Count, count);

        }
    }
}
