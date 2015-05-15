using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace Falcon.Caching
{
    /// <summary>
    /// Represents a MemoryCacheCache
    /// </summary>
    public class MemoryCacheManager : ICacheManager
    {
        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }
        
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        public Dictionary<string, T> MGet<T>(List<string> keys)
        {
            return keys.Distinct().ToDictionary(key => key, Get<T>);
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time in minutes</param>
        public void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy();
            if (cacheTime == 0)
            {
                policy.AbsoluteExpiration = DateTimeOffset.MaxValue;
            }
            else
            {
                policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(cacheTime);
            }
                        
            Cache.Set(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        public void Set(string key, object data)
        {
            Set(key, data, 0);
        }

        public void MSet(Dictionary<string, object> data, int cacheTime)
        {
            foreach (var item in data)
            {
                Set(item.Key, item.Value, cacheTime);
            }
        }

        public void MSet(Dictionary<string, object> data)
        {
            MSet(data, 0);
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public bool Exists(string key)
        {
            return (Cache.Contains(key));
        }

        public IEnumerable<bool> MExists(List<string> keys)
        {
            return keys.Select(item => Cache.Contains(item));
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = Cache.Where(item => regex.IsMatch(item.Key)).Select(item => item.Key).ToList();
            //foreach (var item in Cache)
            //    if (regex.IsMatch(item.Key))
            //        keysToRemove.Add(item.Key);

            foreach (string key in keysToRemove)
            {
                Remove(key);
            }
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }
    }
}