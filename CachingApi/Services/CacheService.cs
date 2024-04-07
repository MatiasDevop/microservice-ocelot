
using StackExchange.Redis;
using System.Text.Json;

namespace CachingApi.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _cacheDb;
        private static IConnectionMultiplexer _connectionMultiplexer;
        public CacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            //Only for testing porpuse better set it on Configuraiton or appsettings
            _connectionMultiplexer = connectionMultiplexer ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
            // var redis = ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false");
            _cacheDb = _connectionMultiplexer.GetDatabase(0);
            //_cacheDb = redis.GetDatabase();
        }
        public T GetData<T>(string key)
        {
            var value = _cacheDb.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;// means a nullable type of that object
        }

        public object RemoveData(string key)
        {
            var _exist = _cacheDb.KeyExists(key);
            if (_exist)
            {
                return _cacheDb.KeyDelete(key);
            }

            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
            return isSet;
        }
    }
}
