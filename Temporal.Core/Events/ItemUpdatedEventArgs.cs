using System;

namespace Temporal.Core.Events
{
    public class ItemUpdatedEventArgs : EventArgs
    {
        public string CacheKey { get; set; }
    }
}
