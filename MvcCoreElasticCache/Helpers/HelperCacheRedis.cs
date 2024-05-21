using StackExchange.Redis;

namespace MvcCoreElasticCache.Helpers
{
    public class HelperCacheRedis
    {
        private static Lazy<ConnectionMultiplexer>
            CreateConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                // cadena conexion
                return ConnectionMultiplexer.Connect("");
            });

        public ConnectionMultiplexer Connection
        {
            get
            {
                return CreateConnection.Value;
            }
        }
    }
}
