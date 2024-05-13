using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using VideoManagerApi.Models;

namespace VideoManagerApi.Cache
{
    public class CacheService
    {
        private readonly IDistributedCache _cache;
        private readonly string _cacheKeyPrefix = "product_videos_";

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetFromCache<T>(string cacheKey)
        {
            var serializedData = await _cache.GetStringAsync(cacheKey);
            if (serializedData != null)
            {
                return JsonConvert.DeserializeObject<T>(serializedData);
            }
            return default;
        }

        public async Task SetInCache<T>(string cacheKey, T value, TimeSpan expiration)
        {
            var serializedData = JsonConvert.SerializeObject(value);
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTimeOffset.UtcNow.Add(expiration));
            await _cache.SetStringAsync(cacheKey, serializedData, cacheOptions);
        }

        public async Task RemoveFromCache(string cacheKey)
        {
            await _cache.RemoveAsync(cacheKey);
        }

        public string GetCacheKey(int productId)
        {
            return $"{_cacheKeyPrefix}{productId}";
        }
    }
}
