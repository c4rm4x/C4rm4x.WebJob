#region Using

using C4rm4x.WebJob.Framework.Caching;
using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using MemCache = System.Runtime.Caching.MemoryCache;

#endregion

namespace C4rm4x.WebJob.Caching.MemoryCache
{
    /// <summary>
    /// Implementation of ICache using MemoryCache
    /// </summary>
    public class Cache : ICache
    {
        /// <summary>
        /// Retrieves an object previously stored by key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The previously stored object (if any); null, otherwise</returns>
        public Task<object> RetrieveAsync(string key)
        {
            return Task.FromResult(Retrieve(key));
        }

        /// <summary>
        /// Retrieves an object of type T previously stored by key
        /// </summary>
        /// <typeparam name="T">Type of the object to retrieve</typeparam>
        /// <param name="key">The key</param>
        /// <returns>The previously stored object (if any); null, otherwise</returns>
        /// <exception cref="InvalidCastException">When the object cannot be cast to the specified type</exception>
        public Task<T> RetrieveAsync<T>(string key)
        {
            return Task.FromResult((T)Retrieve(key));
        }

        /// <summary>
        /// Stores an object into the cache with the given key as identifier
        /// </summary>        
        /// <param name="key">The key</param>
        /// <param name="objectToStore">Object to store</param>
        /// <param name="expirationTime">How long the object will be stored (-1 if you do not want the object to expire)</param>
        public Task StoreAsync(string key, object objectToStore, int expirationTime = 60)
        {
            MemCache.Default.Set(
                new CacheItem(key, objectToStore),
                GetCacheItemPolicy(expirationTime));

            return Task.FromResult(false);
        }

        private object Retrieve(string key)
        {
            if (MemCache.Default.Contains(key))
                return MemCache.Default.GetCacheItem(key).Value;

            return null;
        }

        private CacheItemPolicy GetCacheItemPolicy(int expirationTime)
        {
            var policy = new CacheItemPolicy();

            policy.AbsoluteExpiration =
                expirationTime == -1
                ? ObjectCache.InfiniteAbsoluteExpiration
                : new DateTimeOffset(DateTime.Now.AddSeconds(expirationTime));

            return policy;
        }
    }
}
