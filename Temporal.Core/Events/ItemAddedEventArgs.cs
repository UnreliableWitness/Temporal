using System;

namespace Temporal.Core.Events
{
    public class ItemAddedEventArgs : EventArgs
    {
        public string CacheKey { get; set; }
    }
}
