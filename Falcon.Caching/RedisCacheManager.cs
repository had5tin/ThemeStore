using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using StackExchange.Profiling;

namespace Falcon.Caching
{
    public class RedisCacheManager : ICacheManager
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;
        private readonly int _db = ConfigurationManager.AppSettings["RedisCacheDb"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["RedisCacheDb"]) : 1;

        private const int MaxMultipleItems = 100;
        private const int MinByteCompress = 860;

        public RedisCacheManager()
        {
            _connectionMultiplexer = RedisSingleton.GetInstance().GetConnectionMultiplexer();
            _database = _connectionMultiplexer.GetDatabase(_db);
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public T Get<T>(string key)
        {
            var data = (byte[])_database.StringGet(key);
            return GetValue<T>(data);
        }

        public Dictionary<string, T> MGet<T>(List<string> keys)
        {
            var result = new Dictionary<string, T>();

            int count = keys.Count;
            if (count == 0) return result;

            var profiler = MiniProfiler.Current; // it's ok if this is null
            using (profiler.Step("Multiple get cache"))
            {
                if (count <= MaxMultipleItems)
                {
                    result = _MGet(keys, result);
                }
                else
                {
                    int i = 0;
                    while (count - i * MaxMultipleItems > 0)
                    {
                        var temp = keys.Skip(i * MaxMultipleItems).Take(MaxMultipleItems).ToList();
                        result = _MGet(temp, result);
                        ++i;
                    }
                }
            }

            return result;
        }

        private Dictionary<string, T> _MGet<T>(List<string> keys, Dictionary<string, T> result)
        {
            var temp = keys.Select(item => (RedisKey)item).Distinct().ToArray();
            var data = _database.StringGet(temp);
            for (int i = 0; i < keys.Count; i++ )
            {
                result[keys[i]] = GetValue<T>(data[i]);
            }

            return result;
        }

        private T GetValue<T>(byte[] data)
        {
            if (data != null)
            {
                if (CacheUtil.IsGZipHeader(data))
                {
                    var unzip = CacheUtil.Decompress(data);
                    return CacheUtil.DeserializeProtobuf<T>(unzip);
                }
            
                return CacheUtil.DeserializeProtobuf<T>(data);
            }
            
            return default(T);
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

            byte[] bytes = CacheUtil.SerializeProtobuf(data);
            if (bytes.Length > MinByteCompress)
            {
                bytes = CacheUtil.Compress(bytes);
            }

            if (cacheTime > 0)
            {
                _database.StringSet(key, bytes, new TimeSpan(0, 0, 0, cacheTime, 0), When.Always, CommandFlags.FireAndForget);
            }
            else
            {
                _database.StringSet(key, bytes, null, When.Always, CommandFlags.FireAndForget);
            }
            
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
            return _database.KeyExists(key);
        }

        public IEnumerable<bool> MExists(List<string> keys)
        {
            string luaScript = @"local exists = {}
                                local existence
                                for _, key in ipairs(KEYS) do table.insert(exists, redis.call('exists', key)) end
                                return exists";
            var result = (bool[]) _database.ScriptEvaluate(luaScript, keys.Select(key => (RedisKey)key).ToArray());
            return result;
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public void Remove(string key)
        {
            _database.KeyDelete(key, CommandFlags.FireAndForget);
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public void RemoveByPattern(string pattern)
        {
            //foreach (var item in _connectionMultiplexer.GetEndPoints())
            //{
            //    var server = _connectionMultiplexer.GetServer(item);
            //    if (pattern.StartsWith("^"))
            //    {
            //        pattern = pattern.Substring(1);
            //    }
            //    if (pattern.EndsWith(@"\..*$"))
            //    {
            //        pattern = pattern.Substring(0, pattern.Length - 5) + "*";
            //    }
            //    foreach (var key in server.Keys(_db, pattern: pattern, pageSize: int.MaxValue))
            //    {
            //        _database.KeyDelete(key);
            //    }
            //}

            if (pattern.StartsWith("^"))
            {
                pattern = pattern.Substring(1);
            }
            if (pattern.EndsWith(@"\..*$"))
            {
                pattern = pattern.Substring(0, pattern.Length - 5) + "*";
            }
            foreach (var item in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(item);
                string luaScript = @"for _,k in ipairs(redis.call('keys', ARGV[1])) do redis.call('del', k) end";
                server.Multiplexer.GetDatabase(_db).ScriptEvaluate(luaScript, new[] { (RedisKey)pattern });
            }
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public void Clear()
        {
            foreach (var item in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(item);
                server.FlushDatabase(_db);
            }
            //string luaScript = @"redis.call('flushdb')";
            //_database.ScriptEvaluate(luaScript);
        }
    }
}
