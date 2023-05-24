using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using SelectU.Contracts.Services;

namespace SelectU.Core.Services
{
    public class CachingService : ICachingService
    {
        private readonly IMemoryCache _memoryCache;

        public CachingService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T? GetItem<T>(string cacheKey)
        {
            if (_memoryCache.TryGetValue(cacheKey, out T item))
            {
                return item;
            }
            return default;
        }

        public void RemoveItem(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);

        }

        public T SetItem<T>(string cacheKey, T item)
        {
            return _memoryCache.Set(cacheKey, item, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1),
                SlidingExpiration = TimeSpan.FromDays(1)
            });
        }
    }
}
