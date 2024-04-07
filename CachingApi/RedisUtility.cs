using StackExchange.Redis;

namespace CachingApi
{
    public static class RedisUtility
    {
        public static ConnectionMultiplexer ConnectRedis(IConfiguration Configuration)
        {
            var redisOptions = ConfigurationOptions.Parse($"{Configuration["Redis:localhost"]}:{Configuration["Redis:6379"]}");
            //redisOptions.Password = Configuration["Redis:Password"];
            redisOptions.AbortOnConnectFail = false;
            var multiplexer = ConnectionMultiplexer.Connect(redisOptions);
            return multiplexer;
        }
    }
}
