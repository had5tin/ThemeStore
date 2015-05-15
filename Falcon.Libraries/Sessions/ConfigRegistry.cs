using Falcon.Common;
using Falcon.Libraries.Sessions.Config;
using Falcon.Tasks;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Falcon.Libraries.Sessions
{
    public class ConfigRegistry : IStartupTask
    {
        public void Execute()
        {
            RedisConnectionConfig.GetSERedisServerConfig = (HttpContextBase context) =>
            {
                var config = ConfigurationOptions.Parse(Util.GetRedisConfig());
                config.AllowAdmin = true;
                config.AbortOnConnectFail = false;
                return new KeyValuePair<string, ConfigurationOptions>(
                    "DefaultConnection", config);
            };
        }
        public int Order
        {
            get { return 0; }
        }
    }
}
