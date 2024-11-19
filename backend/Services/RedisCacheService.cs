using StackExchange.Redis;

namespace PublicTransportNavigator.Services
{
    public class RedisCacheService(IConnectionMultiplexer redis)
    {
        private readonly IConnectionMultiplexer _redis = redis;

        public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, value, expiry);
        }

        public async Task<string?> GetAsync(string key)
        {
            var db = _redis.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task DeleteAsync(string key)
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(key);
        }

        public async Task ProlongKeyLifetime(string key, TimeSpan extension)
        {
            var db = _redis.GetDatabase();

            var currentTtl = await db.KeyTimeToLiveAsync(key);

            if (currentTtl.HasValue)
            {
                await db.KeyExpireAsync(key, currentTtl.Value.Add(extension));
            }
        }
    }
}
