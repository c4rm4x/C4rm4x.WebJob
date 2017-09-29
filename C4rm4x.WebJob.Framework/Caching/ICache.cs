﻿#region Using

using System.Threading.Tasks;

#endregion

namespace C4rm4x.WebJob.Framework.Caching
{
    /// <summary>
    /// Service to cache information for a Job
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Retrieves an object from the cache based on specified key
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>The object based on specified key if exists. Otherwise null</returns>
        Task<object> RetrieveAsync(string key);

        /// <summary>
        /// Stores an object in the cache linked to specified key for a period of time
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="objectToStore">The object to store</param>
        /// <param name="expirationTime">The time for which the object will be stored (-1 if you want the object not to expire) in seconds</param>
        /// <remarks>
        /// If a previous object exists with the specified key
        /// it will be overwritten
        /// </remarks>
        Task StoreAsync(
            string key,
            object objectToStore,
            int expirationTime = 60);

        /// <summary>
        /// Retrieves an object of type T from the cache 
        /// based on specified key
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve</typeparam>
        /// <param name="key">The key</param>
        /// <returns>The object based on spefified key if exists. Otherwise null</returns>
        Task<T> RetrieveAsync<T>(string key);
    }
}
