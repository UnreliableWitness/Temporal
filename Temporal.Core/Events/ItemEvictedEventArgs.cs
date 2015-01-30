using System;

namespace Temporal.Core.Events
{
    public class ItemEvictedEventArgs : EventArgs
    {
        public string CacheKey { get; set; }
    }
}
