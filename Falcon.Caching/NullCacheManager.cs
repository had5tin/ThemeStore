using System.Collections.Generic;

namespace Falcon.Caching
{
    /// <summary>
    /// NullCache is used when you don't want to use Cache ;)
    /// </summary>
    public class NullCacheManager : ICacheManager
    {
        public T Get<T>(string key)
        {
            return default(T);
        }

        public Dictionary<string, T> MGet<T>(List<string> keys)
        {
            var result = new Dictionary<string, T>();
            foreach (var item in keys)
            {
                result[item] = default(T);
            }
            return result;
        }

        public void Set(string key, object data, int cacheTime)
        {
            //do nothing
        }

        public void Set(string key, object data)
        {
            //do nothing
        }

        public void MSet(Dictionary<string, object> data, int cacheTime)
        {
            //do nothing
        }

        public void MSet(Dictionary<string, object> data)
        {
            //do nothing
        }

        public bool Exists(string key)
        {
            return false;
        }

        public IEnumerable<bool> MExists(List<string> keys)
        {
            var result = new List<bool>();
            for (int i = 0; i < keys.Count; i++)
            {
                result.Add(false);
            }
            return result;
        }

        public void Remove(string key)
        {
            //do nothing
        }

        public void RemoveByPattern(string pattern)
        {
            //do nothing
        }

        public void Clear()
        {
            //do nothing
        }
    }
}
