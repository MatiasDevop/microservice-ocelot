using StackExchange.Redis;

namespace CachingApi
{
    public static class RedisUtility
    {
        public static ConnectionMultiplexer ConnectRedis(IConfiguration Configuration)
        {
            var host = Configuration["Redis:Host"] ?? "localhost";
            var port = Configuration["Redis:Port"] ?? "6379";
            var redisOptions = ConfigurationOptions.Parse($"{host}:{port}");
            var password = Configuration["Redis:Password"];
            if (!string.IsNullOrEmpty(password))
            {
                redisOptions.Password = password;
            }
            redisOptions.AbortOnConnectFail = false;
            var multiplexer = ConnectionMultiplexer.Connect(redisOptions);
            return multiplexer;
        }
    }
}
