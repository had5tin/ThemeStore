namespace Falcon.Libraries.Sessions.Config
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Falcon.Libraries.Sessions.Serialization;

    public static class RedisSerializationConfig
    {
        static RedisSerializationConfig()
        {
            RedisSerializationConfig.SessionDataSerializer = new RedisJSONSerializer();
        }

        /// <summary>
        /// Gets or sets the serializer that Falcon.Libraries.Sessions uses to translate between
        ///     Redis-persisted string values and local Session object values
        /// </summary>
        public static IRedisSerializer SessionDataSerializer { get; set; }

        /// <summary>
        /// Gets or sets the method that is called when the SessionDataSerializer encounters an 
        ///     exception, helpful for tracking down incompatible types if they exist.
        /// </summary>
        public static Action<Exception> SerializerExceptionLoggingDel { get; set; }
    }
}
