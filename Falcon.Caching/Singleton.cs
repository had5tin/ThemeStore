using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Configuration;
using StackExchange.Redis;

namespace Falcon.Caching
{
    public class RedisSingleton
    {
        private static volatile RedisSingleton instance;
        private static object locker = new object();
        private ConnectionMultiplexer _connectionMultiplexer;
        private ConfigurationOptions _configOption;
        private RedisSingleton()
        {
            _configOption = ConfigurationOptions.Parse(_GetRedisConfig());
            _configOption.AllowAdmin = true;
            _configOption.AbortOnConnectFail = false;
        }

        public static RedisSingleton GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                        instance = new RedisSingleton();
                }
            }
            return instance;
        }

        public ConnectionMultiplexer GetConnectionMultiplexer()
        {
            if(_connectionMultiplexer == null || !_connectionMultiplexer.IsConnected)
            {
                _connectionMultiplexer = ConnectionMultiplexer.Connect(_configOption);
            }
            return _connectionMultiplexer;
        }

        private static string _GetRedisConfig()
        {
            return ConfigurationManager.AppSettings["RedisConfig"] != null ? ConfigurationManager.AppSettings["RedisConfig"] : "localhost";
        }
    }
}
