using Microsoft.Extensions.Logging;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Infrastructure;
using SelectU.Contracts.Services;

namespace SelectU.Core.Infrastructure
{
    public class InMemoryCache : ICache
    {
        private readonly ICachingService _cache;
        private readonly ILogger<InMemoryCache> _logger;
        //private readonly string CACHE_NAME = "item_";

        public InMemoryCache(ILogger<InMemoryCache> logger,
            ICachingService cache
            )
        {
            _cache = cache;
            _logger = logger;
        }

        //public async Task<List<CategoryDTO>> GetAsync()
        //{
        //    string cacheName = CACHE_NAME;

        //    var cachedItem = _cache.GetItem<List<CategoryDTO>>(cacheName);
        //    if (cachedItem != null)
        //    {
        //        _logger.LogInformation($"{cacheName} Item Exists in Cache. Return CachedItem.");
        //        return cachedItem;
        //    }

        //    _logger.LogInformation($"{cacheName} Item doesn't exist in Cache.");
        //    var items = await _service.GetAsync();
        //    _logger.LogInformation($"Add Item to Cache and return.");
        //    var result = _cache.SetItem($"item_", items);
        //    return result;
        //}

        public void ClearCache()
        {
            //_cache.RemoveItem(CACHE_NAME);
        }
    }
}
