using System.Collections.Generic;
namespace Falcon.Caching
{
    /// <summary>
    /// Cache manager interface
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Get multiple value of caches
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">list of key</param>
        /// <returns></returns>
        Dictionary<string, T> MGet<T>(List<string> keys);

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        /// Adds the specified key and object to the cache. Set cache time = 0 (never invalid)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        void Set(string key, object data);

        /// <summary>
        /// Add list of key & object to the cache
        /// </summary>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        void MSet(Dictionary<string, object> data, int cacheTime);

        /// <summary>
        /// Add list of key & object to the cache
        /// </summary>
        /// <param name="data"></param>
        void MSet(Dictionary<string, object> data);

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        bool Exists(string key);

        /// <summary>
        /// return List of values indicating wherther the value in list keys exist
        /// example ["key1", "key2", "key3"] => [0, 1, 1] mean that key1 not exists, key2 and key3 exist
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        IEnumerable<bool> MExists(List<string> keys);

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        void Remove(string key);

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// Clear all cache data
        /// </summary>
        void Clear();
    }
}
