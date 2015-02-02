using System;

namespace Temporal.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DontCacheAttribute : CacheAttribute
    {

    }
}
