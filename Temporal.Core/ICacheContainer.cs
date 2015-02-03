using System.Runtime.Caching;

namespace Temporal.Core
{
    public interface ICacheContainer
    {
        CacheKeyGenerator CacheKeyGenerator { get; }
        event ItemAddedEventHandler ItemAdded;
        event ItemUpdatedEventHandler ItemUpdated;
        event ItemEvictedEventHandler ItemEvicted;
        bool TryAdd(string key, object toCache, CacheItemPolicy cacheItemPolicy);
        bool TryGet(string key, out object returnValue);
        void InvalidateAll();
    }
}