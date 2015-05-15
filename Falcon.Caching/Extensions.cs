using System;

namespace Falcon.Caching
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class CacheExtensions
    {
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire) 
        {
            if (cacheManager.Exists(key))
            {
                return cacheManager.Get<T>(key);
            }
            
            var result = acquire();
            cacheManager.Set(key, result);
            return result;
        }
    }
}
