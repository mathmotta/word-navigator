using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;

namespace BluePrism.WordNavigator.Common.Concurrent
{
    /// <summary>
    /// An thread safe equivalent to <see cref="HashSet{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of the Set</typeparam>
    public class ConcurrentHashSet<T> : IDisposable, IEnumerable<T>, IDeserializationCallback, ISerializable
    {
        /// <summary>
        /// The lock to be used for thread safety
        /// </summary>
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        /// <summary>
        /// The base hashset to store data
        /// </summary>
        private readonly HashSet<T> _hashSet = new HashSet<T>();

        /// <summary>
        /// Adds the specified element to a set.
        /// </summary>
        /// <param name="item">The element to add to the set.</param>
        /// <returns>True if the element is added to the <see cref="ConcurrentHashSet{T}"/>. False if the element is already present</returns>
        public bool Add(T item)
        {
            _lock.EnterWriteLock();
            try
            {
                return _hashSet.Add(item);
            }
            finally
            {
                if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
            }
        }
        /// <summary>
        /// Removes the specified element from a <see cref="ConcurrentHashSet{T}" object />
        /// </summary>
        /// <param name="item">The element to remove.</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false. This method returns false if item is not found in the <see cref="ConcurrentHashSet{T}"/> object.</returns>
        public bool Remove(T item)
        {
            _lock.EnterWriteLock();
            try
            {
                return _hashSet.Remove(item);
            }
            finally
            {
                if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes all elements that match the conditions defined by the specified predicate from a <see cref="ConcurrentHashSet{T}"/>
        /// </summary>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate that defines the conditions of the elements to remove.</param>
        /// <returns>The number of elements that were removed from the <see cref="ConcurrentHashSet{T}" collection./></returns>
        public int RemoveWhere(Predicate<T> match)
        {
            _lock.EnterWriteLock();
            try
            {
                return _hashSet.RemoveWhere(match);
            }
            finally
            {
                if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a <see cref="ConcurrentHashSet{T}" /> object.
        /// </summary>
        /// <returns>A <see cref="ConcurrentHashSet{T}.Enumerator"/> object for the </returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            _lock.EnterWriteLock();
            try
            {
                return _hashSet.GetEnumerator();
            }
            finally
            {
                if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a <see cref="ConcurrentHashSet{T}" /> object.
        /// </summary>
        /// <returns>A <see cref="ConcurrentHashSet{T}.Enumerator"/> object for the </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            _lock.EnterWriteLock();
            try
            {
                return _hashSet.GetEnumerator();
            }
            finally
            {
                if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Removes all elements from a <see cref="ConcurrentHashSet{T}" />.
        /// </summary>
        public void Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                _hashSet.Clear();
            }
            finally
            {
                if (_lock.IsWriteLockHeld) _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Determines whether a <see cref="ConcurrentHashSet{T}" /> object contains the specified element.
        /// </summary>
        /// <param name="item">The element to locate in the <see cref="ConcurrentHashSet{T}" /> object.</param>
        /// <returns>true if the <see cref="ConcurrentHashSet{T}" /> object contains the specified element; otherwise, false.</returns>
        public bool Contains(T item)
        {
            _lock.EnterReadLock();
            try
            {
                return _hashSet.Contains(item);
            }
            finally
            {
                if (_lock.IsReadLockHeld) _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Gets the number of elements that are contained in a set.
        /// </summary>
        /// <returns>The number of elements that are contained in the set.</returns>
        public int Count
        {
            get
            {
                _lock.EnterReadLock();
                try
                {
                    return _hashSet.Count;
                }
                finally
                {
                    if (_lock.IsReadLockHeld) _lock.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Implements the <see cref="ISerializable"/> interface and returns the data needed to serialize a <see cref="ConcurrentHashSet{T}" /> object.
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> object that contains the information required to serialize the <see cref="ConcurrentHashSet{T}" /> object.</param>
        /// <param name="context"> A <see cref="StreamingContext"/> structure that contains the source and destination of the serialized stream associated with the <see cref="ConcurrentHashSet{T}" /> object.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            _lock.EnterReadLock();
            try
            {
                _hashSet.GetObjectData(info, context);
            }
            finally
            {
                if (_lock.IsReadLockHeld) _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Implements the <see cref="ISerializable "/> interface and raises the deserialization event when the deserialization is complete.
        /// </summary>
        /// <param name="sender">The source of the deserialization event.</param>
        /// <exception cref="SerializationException">The <see cref="SerializationInfo"/> object associated with the current <see cref="ConcurrentHashSet{T}" /> object is invalid.</exception>
        public void OnDeserialization(object? sender)
        {
            _lock.EnterReadLock();
            try
            {
                _hashSet.OnDeserialization(sender);
            }
            finally
            {
                if (_lock.IsReadLockHeld) _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// Release all resources currently being used by the <see cref="ConcurrentHashSet{T}"/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                if (_lock != null)
                    _lock.Dispose();
        }

        ~ConcurrentHashSet()
        {
            Dispose(false);
        }
    }
}
