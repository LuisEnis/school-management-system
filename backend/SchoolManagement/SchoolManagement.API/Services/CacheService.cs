using Microsoft.Extensions.Caching.Memory;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheService> _logger;

        public CacheService(IMemoryCache cache, ILogger<CacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public bool TryGetValue<T>(string key, out T? value)
        {
            var found =
                _cache.TryGetValue(key, out value);

            if (found)
            {
                _logger.LogDebug(
                    "Cache hit for key {CacheKey}.",
                    key);
            }
            else
            {
                _logger.LogDebug(
                    "Cache miss for key {CacheKey}.",
                    key);
            }

            return found;
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            _cache.Set(
                key,
                value,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        expiration
                });

            _logger.LogDebug(
                "Cache entry set for key {CacheKey}.",
                key);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);

            _logger.LogDebug(
                "Cache entry removed for key {CacheKey}.",
                key);
        }
    }
}
