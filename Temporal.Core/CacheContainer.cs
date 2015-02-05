using System;
using System.Linq;
using System.Runtime.Caching;
using Temporal.Core.Events;

namespace Temporal.Core
{
    public delegate void ItemAddedEventHandler(object sender, ItemAddedEventArgs e);
    public delegate void ItemUpdatedEventHandler(object sender, ItemUpdatedEventArgs e);
    public delegate void ItemEvictedEventHandler(object sender, ItemEvictedEventArgs e);

    public class CacheContainer : ICacheContainer
    {
        private readonly object _mutex = new object();
        private readonly ObjectCache _cache;

        public CacheKeyGenerator CacheKeyGenerator { get; private set; }

        public event ItemAddedEventHandler ItemAdded;
        public event ItemUpdatedEventHandler ItemUpdated;
        public event ItemEvictedEventHandler ItemEvicted;
        
        public CacheContainer()
        {
            CacheKeyGenerator = new CacheKeyGenerator();
            _cache = MemoryCache.Default;
        }

        public bool TryAdd(string key, object toCache, CacheItemPolicy cacheItemPolicy)
        {
            lock(_mutex)
            { 
                if (string.IsNullOrEmpty(key) || toCache == null)
                    return false;
                if (_cache[key] != null)
                {
                    _cache[key] = toCache;
                    if (ItemUpdated != null)
                        ItemUpdated(this, new ItemUpdatedEventArgs {CacheKey = key});
                    return true;
                }

                _cache.Add(key, toCache, cacheItemPolicy);

                if (ItemAdded != null)
                    ItemAdded(this, new ItemAddedEventArgs {CacheKey = key});
            }
            return true;
        }

        public bool TryGet(string key, out object returnValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                returnValue = null;
                return false;
            }

            returnValue = _cache[key];
            if (returnValue == null)
                return false;
            return true;
        }

        public void InvalidateAll()
        {
            var allKeys = _cache.Select(o => o.Key);
            foreach (var allKey in allKeys)
            {
                _cache.Remove(allKey);
            }
        }
    }
}
