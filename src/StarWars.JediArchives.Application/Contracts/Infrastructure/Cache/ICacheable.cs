namespace StarWars.JediArchives.Application.Contracts.Infrastructure.Cache
{
    public interface ICacheable<T>
    {
        bool TryGetFromCache(object cacheKey, out T cachedObject);
        void AddToCache(object cacheKey, T cachedObject);
        void RemoveFromCache(object cacheKey);
    }
}