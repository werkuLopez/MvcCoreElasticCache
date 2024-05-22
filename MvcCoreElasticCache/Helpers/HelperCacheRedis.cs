using StackExchange.Redis;

namespace MvcCoreElasticCache.Helpers
{
    public class HelperCacheRedis
    {
        private static Lazy<ConnectionMultiplexer>
            CreateConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                // cadena conexion
                string connection = "cache-coches.bawdmj.ng.0001.use1.cache.amazonaws.com:6379";
                return ConnectionMultiplexer.Connect(connection);
            });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return CreateConnection.Value;
            }
        }
    }
}
