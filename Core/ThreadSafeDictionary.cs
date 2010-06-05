namespace MultiTenancy.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Dictionary that has a "GetOrAdd" method that is thread-safe
    /// </summary>
    /// <typeparam name="TKey">Dictionary key</typeparam>
    /// <typeparam name="TValue">Dictionary value</typeparam>
    public class ThreadSafeDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        /// <summary>
        /// Lock for adding values
        /// </summary>
        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        /// <summary>
        /// Gets a value or adds the value in a thread-safe way
        /// </summary>
        /// <param name="key">Key in the dictionary</param>
        /// <param name="defaultValue">Delegate that will get the value</param>
        /// <returns>Value from the dictionary with given <paramref name="key"/></returns>
        public TValue GetOrAdd(TKey key, Func<TValue> defaultValue)
        {
            // enter read lock
            this.cacheLock.EnterReadLock();

            try
            {
                // test if value is in the dictionary
                if (this.ContainsKey(key))
                    return this[key];
            }
            finally
            {
                // exit read lock
                this.cacheLock.ExitReadLock();
            }

            // enter write lock
            this.cacheLock.EnterWriteLock();
            try
            {
                if (!ContainsKey(key))
                    this.Add(key, defaultValue());
                return this[key];
            }
            finally
            {
                // exit write lock
                this.cacheLock.ExitWriteLock();
            }
        }
    }
}