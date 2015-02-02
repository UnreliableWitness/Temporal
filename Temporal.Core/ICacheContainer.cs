using System;

namespace Temporal.Core
{
    public interface ICacheContainer
    {
        CacheKeyGenerator CacheKeyGenerator { get; }
        event ItemAddedEventHandler ItemAdded;
        event ItemUpdatedEventHandler ItemUpdated;
        event ItemEvictedEventHandler ItemEvicted;
        bool TryAdd(string key, object toCache, TimeSpan expiration);
        bool TryGet(string key, out object returnValue);
    }
}