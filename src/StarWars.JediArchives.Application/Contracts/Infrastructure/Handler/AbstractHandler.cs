namespace StarWars.JediArchives.Application.Contracts.Infrastructure.Handler
{
    public abstract class AbstractHandler<T> : ICacheable<T>
    {
        protected abstract object CacheKey { get; }

        private readonly IMemoryCache _cache;
        public AbstractHandler(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void AddToCache(object cacheKey, T cachedObject)
        {
            var option = SetMemoryCacheOptions();
            _cache.Set(cacheKey, cachedObject, option);
        }

        public bool TryGetFromCache(object cacheKey, out T cachedObject)
        {
            return _cache.TryGetValue(cacheKey, out cachedObject);
        }

        public void RemoveFromCache(object cacheKey)
        {
            _cache.Remove(cacheKey);
        }

        private MemoryCacheEntryOptions SetMemoryCacheOptions()
        {
            return new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(90))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(5400))
                .SetPriority(CacheItemPriority.Normal);
        }
    }
}