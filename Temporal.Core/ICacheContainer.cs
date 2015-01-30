using System;

namespace Temporal.Core
{
    public interface ICacheContainer
    {
        CacheKeyGenerator CacheKeyGenerator { get; }
        string[] MethodsToCacheStartWith { get; set; }
        string[] MethodsThatInvalidateStartWith { get; set; }
        event ItemAddedEventHandler ItemAdded;
        event ItemUpdatedEventHandler ItemUpdated;
        event ItemEvictedEventHandler ItemEvicted;
        bool TryAdd(string key, object toCache, TimeSpan expiration);
        bool TryGet(string key, out object returnValue);
        void InvalidateIfNeeded(string cacheKey);
    }
}